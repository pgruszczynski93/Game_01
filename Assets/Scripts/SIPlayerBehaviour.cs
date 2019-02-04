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

        protected override void Awake()
        {
            base.Awake();
            SetInitialReferences();
        }

        private void OnEnable()
        {
            SIEventsHandler.OnStatisticsUpdate += UpdatePlayerStatistics;
        }

        private void OnDisable()
        {
            SIEventsHandler.OnStatisticsUpdate -= UpdatePlayerStatistics;
        }

        private void SetInitialReferences()
        {
            _playerStatistics = new SIStatistics
            {
                currentHealth = 3,
                currentWave = 1,
                currentWeaponType = WeaponType.Projectile
            };
        }

        public void UpdatePlayerStatistics(SIBonusInfo bonusInfo)
        {
            if (_playerStatistics == null)
            {
                SIHelpers.SISimpleLogger(this, "Can't update statistics.", SimpleLoggerTypes.Warning);
                return;
            }

            ModifyStatistics(bonusInfo);
        }

        private void ModifyStatistics(SIBonusInfo bonusInfo)
        {
            SIHelpers.SISimpleLogger(this, "Statistics update.", SimpleLoggerTypes.Log);

            _playerStatistics.currentHealth += bonusInfo.bonusStatistics.gainedHealth;
            _playerStatistics.currentHealth = Mathf.Clamp(_playerStatistics.currentHealth, MIN_HEALTH, MAX_HEALTH);

            SIHelpers.SISimpleLogger(this, PlayerStatisticsText() , SimpleLoggerTypes.Log);

        }

        public string PlayerStatisticsText()
        {
            return string.Format("Current statistics:\nhealth: {0}\nweapon: {1}\nscore: {2}\nwave: {3}", _playerStatistics.currentHealth,
                _playerStatistics.currentScore, _playerStatistics.currentScore, _playerStatistics.currentWave);
        }
    }
}
