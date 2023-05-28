using PG.Game.Configs;
using PG.Game.Entities.Enemies;
using PG.Game.EventSystem;
using PG.Game.Features.ObjectsPool;
using PG.Game.Helpers;
using UnityEngine;

namespace PG.Game.Bonuses {
    public class BonusesManager : ObjectsPool<BonusBehaviour> {
        [Range(0, 100), SerializeField] int _bonusDropPropability;
        [SerializeField] ScriptableBonusDropLookup _scriptableLookup;

        int _bonusVariantsCount;
        BonusType _currentBonusType;
        Vector3 _currentDropPosition;

        BonusesDropRatesLookup _loadedLookup;

        protected override void Initialise() {
            base.Initialise();

            _loadedLookup = _scriptableLookup.dropRatesLookup;
            _bonusVariantsCount = _loadedLookup.Count;
        }

        protected override void SubscribeEvents() {
            base.SubscribeEvents();
            GameplayEvents.OnEnemyDeath += HandleOnEnemyDeath;
        }

        protected override void UnsubscribeEvents() {
            base.UnsubscribeEvents();
            GameplayEvents.OnEnemyDeath -= HandleOnEnemyDeath;
        }

        void HandleOnEnemyDeath(EnemyBehaviour enemy) {
            _currentDropPosition = enemy.transform.position;
            TryToDropBonus();
        }

        void TryToDropBonus() {
            float probability = Random.Range(0, 100);
            if (CanSelectBonusToDrop(probability))
                return;

            _currentBonusType = (BonusType)Random.Range(0, _bonusVariantsCount);
            BonusDropInfo dropInfo = _loadedLookup[_currentBonusType];
            probability = Random.Range(0, 100);
            if (!CanBeDropped(probability, dropInfo))
                return;

            SetNextObjectFromPool();
        }

        protected override void ManagePoolableObject() {
            _currentlyPooledObject.SetSpawnPosition(_currentDropPosition);
            _currentlyPooledObject.SetBonusVariant(_currentBonusType);
            _currentlyPooledObject.PerformOnPoolActions();
        }

        bool CanSelectBonusToDrop(float probability) {
            return _loadedLookup == null || probability > _bonusDropPropability;
        }

        static bool CanBeDropped(float probability, BonusDropInfo dropInfo) {
            return probability > dropInfo.minDropRate && probability < dropInfo.maxDropRate;
        }
    }
}