using System.Collections.Generic;
using Sirenix.OdinInspector;
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

        void Start() => Initialise();

        void Initialise() {
            _loadedLookup = _scriptableLookup.dropRatesLookup;
            _bonusVariantsCount = _loadedLookup.Count;
        }

        void OnEnable() => SubscribeEvents();
        void OnDisable() => UnsubscribeEvents();

        void SubscribeEvents() {
            SIGameplayEvents.OnEnemyDeath += HandleOnEnemyDeath;
        }

        void UnsubscribeEvents() {
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
            
            UpdatePool();
        }
        
        //to refactor:
        protected override void ManagePooledObject() {
            _objectsPool[_poolIndex].SetSpawnPosition(_currentDropPosition);
            _objectsPool[_poolIndex].SetBonusVariant(_currentBonusType);
            _objectsPool[_poolIndex].UseObjectFromPool();
        }

        bool CanSelectBonusToDrop(float probability) {
            return _loadedLookup == null || probability > _bonusDropPropability;
        }

        static bool CanBeDropped(float probability, BonusDropInfo dropInfo) {
            return probability > dropInfo.minDropRate && probability < dropInfo.maxDropRate;
        }

#if UNITY_EDITOR
        [Button]
        void AssignBonuses() {
            for (var i = 0; i < _objectsPool.Count; i++) {
                DestroyImmediate(_objectsPool[i]?.gameObject);
            }

            _objectsPool = new List<SIBonus>();
            SIBonus bonusInstance;
            UnityEditor.Undo.RegisterFullObjectHierarchyUndo(gameObject, "Bonus hierarchy changed");
            for (var i = 0; i < _poolCapacity; i++) {
                bonusInstance = Instantiate(_prefabToSpawn, transform);
                UnityEditor.Undo.RegisterCreatedObjectUndo(bonusInstance.gameObject, "Bonus Instantiaton");
                bonusInstance.Parent = transform;
                bonusInstance.transform.localPosition = SIScreenUtils.HiddenObjectPosition;
                _objectsPool.Add(bonusInstance);
            }
        }
#endif
    }
}