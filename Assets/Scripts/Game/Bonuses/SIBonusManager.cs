﻿using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SpaceInvaders {
    public class SIBonusManager : MonoBehaviour {
        [SerializeField] int _maxBonusToSpawn;
        [Range(0, 100), SerializeField] int _bonusDropPropability;
        
        [SerializeField] ScriptableBonusDropLookup _scriptableLookup;
        [SerializeField] SIBonus _bonusPrefab;
        [SerializeField] List<SIBonus> _bonusesPool;
        
        int _poolIndex;
        int _bonusTypesCount;
        Vector3 _currentDropPosition;
        SIBonusDropRatesLookup _loadedLookup;

        void Start() => Initialise();

        void Initialise() {
            _loadedLookup = _scriptableLookup.dropRatesLookup;
            _bonusTypesCount = _loadedLookup.Count;
        }

        void OnEnable() {
            SubscribeEvents();
        }

        void OnDisable() {
            UnsubscribeEvents();
        }

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
        
        [Button]
        void TryToDropBonus() {
            float probability = Random.Range(0, 100);
            if (CanSelectBonusToDrop(probability))
                return;

            BonusType selectedBonus = (BonusType) Random.Range(0, _bonusTypesCount);
            BonusDropInfo dropInfo = _loadedLookup[selectedBonus];
            probability = Random.Range(0, 100);
            if (!CanBeDropped(probability, dropInfo)) 
                return;
            
            ManageBonusesPool(selectedBonus);
        }

        void ManageBonusesPool(BonusType bonusType) {
            _bonusesPool[_poolIndex].SetAndReleaseBonusVariant(_currentDropPosition, bonusType);
            ++_poolIndex;
            if (_poolIndex > _maxBonusToSpawn - 1)
                _poolIndex = 0;
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
            for (var i = 0; i < _bonusesPool.Count; i++) {
                DestroyImmediate(_bonusesPool[i]?.gameObject);
            }

            _bonusesPool = new List<SIBonus>();
            SIBonus bonusInstance;
            UnityEditor.Undo.RegisterFullObjectHierarchyUndo(gameObject, "Bonus hierarchy changed");
            for (var i = 0; i < _maxBonusToSpawn; i++) {
                bonusInstance = Instantiate(_bonusPrefab, transform);
                UnityEditor.Undo.RegisterCreatedObjectUndo(bonusInstance.gameObject, "Bonus Instantiaton");
                bonusInstance.Parent = transform;
                bonusInstance.transform.localPosition = SIScreenUtils.HiddenObjectPosition;
                _bonusesPool.Add(bonusInstance);
            }
        }
#endif
    }
}