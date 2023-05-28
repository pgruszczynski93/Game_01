using PG.Game.Systems.WaveSystem;
using PG.Game.EventSystem;
using PG.Game.Weapons;
using UnityEngine;

namespace PG.Game.Entities.Enemies {
    public class EnemyShootController : ShootController {
        [SerializeField] SIShootBehaviourSetup shootBehaviourSetup;

        bool _canShoot;

        public SIShootBehaviourSetup ShootBehaviourSetup {
            get => shootBehaviourSetup;
            set => shootBehaviourSetup = value;
        }

        public bool CanShoot {
            get => _canShoot;
            set => _canShoot = value;
        }

        protected override void SubscribeEvents() {
            GameplayEvents.OnWaveEnd += HandleOnWaveEnd;
            GameplayEvents.OnEnemyProjectilesCountChanged += HandleOnEnemyProjectilesCountChanged;
        }

        protected override void UnsubscribeEvents() {
            GameplayEvents.OnWaveEnd -= HandleOnWaveEnd;
            GameplayEvents.OnEnemyProjectilesCountChanged -= HandleOnEnemyProjectilesCountChanged;
        }

        void HandleOnWaveEnd(WaveType waveType) {
            if (waveType != WaveType.Grid)
                return;

            EnemyGridEvents.BroadcastOnReadyToShoot(this);
        }

        void HandleOnEnemyProjectilesCountChanged(int availableProjectiles) {
            _availableProjectilesCount = availableProjectiles;
        }

        public void TryToSelectNextShootingBehaviour() {
            _canShoot = false;

            EnemyBehaviour closerRowCandidate = shootBehaviourSetup.neighbours[Neighbour.Back];
            if (closerRowCandidate == null)
                return;

            EnemyBehaviour furtherRowCandidate =
                closerRowCandidate.EnemyShootController.ShootBehaviourSetup.neighbours[Neighbour.Back];

            EnemyShootController nextSelected = closerRowCandidate.IsEnemyAlive()
                ? closerRowCandidate.EnemyShootController
                : furtherRowCandidate != null && furtherRowCandidate.IsEnemyAlive()
                    ? furtherRowCandidate.EnemyShootController
                    : null;

            if (nextSelected == null)
                return;

            nextSelected.CanShoot = true;
            EnemyGridEvents.BroadcastOnReadyToShoot(nextSelected);
        }
    }
}