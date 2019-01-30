using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyShootBehaviour : SIShootBehaviour<SIProjectileBehaviour>
    {
        [SerializeField] private bool _isAbleToShoot;

        protected override void OnEnable()
        {

        }

        protected override void OnDisable()
        {

        }

        private void Start()
        {
            SetInitialReferences();
        }

        private void SetInitialReferences()
        {
            _isAbleToShoot = true;
        }

        protected override void Shoot()
        {
            if (_projectileController == null)
            {
                Debug.LogError("Enemy's projectile isn't assigned.");
                return;
            }

            _projectileController.gameObject.SetActive(true);
            _projectileController.MoveObj();
        }

        public void InvokeShoot()
        {
            Shoot();
            _isAbleToShoot = false;
        }
    }

}
