using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpaceInvaders {
    public class SIBonusManager : MonoBehaviour {
        [Range(0, 99), SerializeField] int _bonusDropThreshold;
        [SerializeField] int _bonusTypesCount;
        [SerializeField] int _bonusesInPoolCount;

        [SerializeField] SIBonus _bonusPrefab;
        [SerializeField] SIBonusDropRatesLookup _dropRatesLookup;
        [SerializeField] List<SIBonus> _bonusesPool;


        void Start() {
            Initialise();
        }

        void Initialise() { }

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
            Vector3 enemyWorldPos = enemy.transform.position;
        }

        [Button]
        void TryToDropBonus() {
            float probability = Random.Range(0, 100);
            if (probability > _bonusDropThreshold)
                return;

            BonusType selectedBonus = (BonusType) Random.Range(0, _bonusTypesCount);
            BonusDropInfo dropInfo = _dropRatesLookup[selectedBonus];
            probability = Random.Range(0, 100);
            if (probability > dropInfo.minDropRate && probability < dropInfo.maxDropRate) {
                _bonusesPool[(int) selectedBonus].SetBonus(selectedBonus);
                Debug.Log(selectedBonus.ToString());
            }


        }

#if UNITY_EDITOR
        [Button]
        void AssignBonuses() {
            _bonusesPool = new List<SIBonus>();
            SIBonus bonusInstance;
            for (var i = 0; i < _bonusesInPoolCount; i++) {
                bonusInstance = Instantiate(_bonusPrefab, transform);
                bonusInstance.SetBonus(BonusType.Undefined);
                _bonusesPool.Add(bonusInstance);
            }
        }

        [Button]
        void CreateLookupTable() {
            _dropRatesLookup = new SIBonusDropRatesLookup();
            var bonusTypes = Enum.GetValues(typeof(BonusType));
            _bonusTypesCount = bonusTypes.Length;
            foreach (var bonus in bonusTypes) {
                if(_dropRatesLookup.ContainsKey((BonusType)bonus))
                    continue;
                
                _dropRatesLookup.Add((BonusType)bonus, new BonusDropInfo());
            }
        }
#endif
    }
}