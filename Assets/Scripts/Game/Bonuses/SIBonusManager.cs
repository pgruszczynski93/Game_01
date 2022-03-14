using SpaceInvaders.ObjectsPool;
using UnityEngine;

namespace SpaceInvaders {
    public class SIBonusManager : SIObjectPool<SIBonus> {
        [Range(0, 100), SerializeField] int _bonusDropPropability;
        [SerializeField] ScriptableBonusDropLookup _scriptableLookup;

        int _bonusVariantsCount;
        BonusType _currentBonusType;
        Vector3 _currentDropPosition;
        
        SIBonusDropRatesLookup _loadedLookup;

        protected override void Initialise() {
            base.Initialise();
            
            _loadedLookup = _scriptableLookup.dropRatesLookup;
            _bonusVariantsCount = _loadedLookup.Count;
        }

        protected override void SubscribeEvents() {
            base.SubscribeEvents();
            SIGameplayEvents.OnEnemyDeath += HandleOnEnemyDeath;
        }

        protected override void UnsubscribeEvents() {
            base.UnsubscribeEvents();
            SIGameplayEvents.OnEnemyDeath -= HandleOnEnemyDeath;
        }

        void HandleOnEnemyDeath(SIEnemyBehaviour enemy) {
            _currentDropPosition = enemy.transform.position;
            TryToDropBonus();
        }
        
        void TryToDropBonus() {
            float probability = Random.Range(0, 100);
            if (CanSelectBonusToDrop(probability))
                return;

            _currentBonusType = (BonusType) Random.Range(0, _bonusVariantsCount);
            BonusDropInfo dropInfo = _loadedLookup[_currentBonusType];
            probability = Random.Range(0, 100);
            if (!CanBeDropped(probability, dropInfo)) 
                return;
            
            SetNextObjectFromPool();
        }
        
        protected override void ManagePooledObject() {
            _currentObjectFromPool.SetSpawnPosition(_currentDropPosition);
            _currentObjectFromPool.SetBonusVariant(_currentBonusType);
            _currentObjectFromPool.UseObjectFromPool();
        }

        bool CanSelectBonusToDrop(float probability) {
            return _loadedLookup == null || probability > _bonusDropPropability;
        }

        static bool CanBeDropped(float probability, BonusDropInfo dropInfo) {
            return probability > dropInfo.minDropRate && probability < dropInfo.maxDropRate;
        }
    }
}