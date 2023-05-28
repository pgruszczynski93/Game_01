using System;
using PG.Game.Bonuses;
using PG.Game.Helpers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PG.Game.Configs {
    public class ScriptableBonusDropLookup : ScriptableObject {
        public BonusesDropRatesLookup dropRatesLookup;

        [Button]
        void CreateLookupTable() {
            dropRatesLookup = new BonusesDropRatesLookup();
            var bonusTypes = Enum.GetValues(typeof(BonusType));
            foreach (var bonus in bonusTypes) {
                if (dropRatesLookup.ContainsKey((BonusType)bonus))
                    continue;

                dropRatesLookup.Add((BonusType)bonus, new BonusDropInfo());
            }
        }
    }
}