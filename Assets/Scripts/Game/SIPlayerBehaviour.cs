using UnityEngine;

namespace SpaceInvaders
{
    public class SIPlayerBehaviour : SIGenericSingleton<SIPlayerBehaviour>
    {
        const int MIN_HEALTH = 0;
        const int MAX_HEALTH = 6;

        [SerializeField] SIPlayerMovement _playerMovement;

        [SerializeField] SIStatistics _playerStatistics;
        [SerializeField] SIPlayerShootBehaviour _playerShoot;
        [SerializeField] SIPlayerProjectilesController _playerProjectileController;
        [SerializeField] SIVFXManager _shieldVfxBehaviour;
        [SerializeField] SITimeBonusesManager _timeBonusesManager;

        public SIPlayerMovement PlayerMovement => _playerMovement;

        public SIPlayerShootBehaviour PlayerShoot => _playerShoot;

        public SIPlayerProjectilesController ProjectileController => _playerProjectileController;

        public SIStatistics PlayerStatistics { get => _playerStatistics; set => _playerStatistics = value; }

        public SIVFXManager ShieldVfxBehaviour => _shieldVfxBehaviour;

        protected override void Awake()
        {
            base.Awake();
            SetInitialReferences();
        }

        void OnEnable()
        {
            SIEventsHandler.OnBonusCollision += UpdatePlayerStatistics;
        }

        void OnDisable()
        {
            SIEventsHandler.OnBonusCollision -= UpdatePlayerStatistics;
        }

        void SetInitialReferences()
        {
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


        void ParseBonus(SIBonusInfo bonusInfo)
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

        void ModifyStatistics(SIBonusInfo bonusInfo)
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


        void EnableShield()
        {
            if (ShieldVfxBehaviour == null)
            {
                SIHelpers.SISimpleLogger(this, "Behaviour is not assigned.", SimpleLoggerTypes.Error);
                return;
            }

            ShieldVfxBehaviour.TryToEnableVFX(true);
            SIHelpers.SISimpleLogger(this, "Shield enabled.", SimpleLoggerTypes.Log);
        }

        void ModifyCurrentWeapon(CollectibleLevel collectibleLevel)
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
