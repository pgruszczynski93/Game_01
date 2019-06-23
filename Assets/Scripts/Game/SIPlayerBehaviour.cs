using UnityEngine;

namespace SpaceInvaders
{
    public class SIPlayerBehaviour : SIGenericSingleton<SIPlayerBehaviour>
    {
        private const int MIN_HEALTH = 0;
        private const int MAX_HEALTH = 6;

        [SerializeField] private SIStatistics _playerStatistics;
        [SerializeField] private SIPlayerMovement _playerMovement;
        [SerializeField] private SIPlayerShootBehaviour _playerShoot;
        [SerializeField] private SIPlayerProjectilesController _playerProjectileController;
        [SerializeField] private SIVFXManager _shieldVfxBehaviour;
        [SerializeField] private SITimeBonusesManager _timeBonusesManager;

        public SIPlayerMovement PlayerMovemnt
        {
            get { return _playerMovement; }
        }

        public SIPlayerShootBehaviour PlayerShoot
        {
            get { return _playerShoot; }
        }

        public SIPlayerProjectilesController ProjectileController
        {
            get { return _playerProjectileController; }
        }

        public SIStatistics PlayerStatistics { get => _playerStatistics; set => _playerStatistics = value; }

        public SIVFXManager ShieldVfxBehaviour
        {
            get { return _shieldVfxBehaviour; }
        }
        

        protected override void Awake()
        {
            base.Awake();
            Initialise();
        }

        private void OnEnable()
        {
            SIEventsHandler.OnBonusCollision += UpdatePlayerStatistics;
        }

        private void OnDisable()
        {
            SIEventsHandler.OnBonusCollision -= UpdatePlayerStatistics;
        }

        private void Initialise()
        {
            if (_playerStatistics == null ||
                _playerMovement == null ||
                _playerShoot == null ||
                _playerProjectileController == null ||
                ShieldVfxBehaviour == null ||
                _timeBonusesManager == null)
            {
                SIHelpers.SISimpleLogger(this, "Assign references in editor first. ", SimpleLoggerTypes.Error);
                return;
            }

            _playerStatistics = new SIStatistics
            {
                currentHealth = 3,
                currentWave = 1,
                currentCollectibleLevel = CollectibleLevel.First
            };

        }

        public void UpdatePlayerStatistics(SIBonusInfo bonusInfo)
        {
            if (_playerStatistics == null)
            {
                SIHelpers.SISimpleLogger(this, "Can't update statistics.", SimpleLoggerTypes.Warning);
                return;
            }

            ParseBonus(bonusInfo);
        }


        private void ParseBonus(SIBonusInfo bonusInfo)
        {
            if (_timeBonusesManager == null)
            {
                SIHelpers.SISimpleLogger(this, "Behaviour is not assigned.", SimpleLoggerTypes.Error);
                return;
            }

            _timeBonusesManager.ManageTimeScheduledBonuses(bonusInfo);

            switch (bonusInfo.bonusType)
            {
                case BonusType.Life:
                    ModifyStatistics(bonusInfo);
                    break;
                case BonusType.Shield:
                    EnableShield();
                    break;
                case BonusType.Weapon:
                    ModifyCurrentWeapon(bonusInfo.bonusStatistics.gainedCollectibleLevel);
                    break;
                default:
                    break;
            }
        }

        private void ModifyStatistics(SIBonusInfo bonusInfo)
        {
            if (bonusInfo == null)
            {
                SIHelpers.SISimpleLogger(this, "BonusInfo object is null", SimpleLoggerTypes.Error);
                return;
            }

            SIHelpers.SISimpleLogger(this, "Statistics update for "+bonusInfo.bonusType, SimpleLoggerTypes.Log);

            _playerStatistics.currentHealth += bonusInfo.bonusStatistics.gainedHealth;
            _playerStatistics.currentHealth = Mathf.Clamp(_playerStatistics.currentHealth, MIN_HEALTH, MAX_HEALTH);

            SIHelpers.SISimpleLogger(this, PlayerStatisticsText() , SimpleLoggerTypes.Log);
        }


        private void EnableShield()
        {
            if (ShieldVfxBehaviour == null)
            {
                SIHelpers.SISimpleLogger(this, "Behaviour is not assigned.", SimpleLoggerTypes.Error);
                return;
            }

            ShieldVfxBehaviour.OnEnableVFXCallback(true);
            SIHelpers.SISimpleLogger(this, "Shield enabled.", SimpleLoggerTypes.Log);
        }

        private void ModifyCurrentWeapon(CollectibleLevel collectibleLevel)
        {
            // to do: add new projectile prefabs, then the rest of code will be working
           // _playerProjectileController.SetCurrentProjectile(weaponType);
        }

        public string PlayerStatisticsText()
        {
            return string.Format("Current statistics:\nhealth: {0}\nweapon: {1}\nscore: {2}\nwave: {3}", _playerStatistics.currentHealth,
                _playerStatistics.currentScore, _playerStatistics.currentScore, _playerStatistics.currentWave);
        }
    }
}
