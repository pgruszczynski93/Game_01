using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SpaceInvaders {
    public class ScriptableBonusDropLookup : ScriptableObject{
        
        public SIBonusDropRatesLookup dropRatesLookup;

        [Button]
        void CreateLookupTable() {
            dropRatesLookup = new SIBonusDropRatesLookup();
            var bonusTypes = Enum.GetValues(typeof(BonusType));
            foreach (var bonus in bonusTypes) {
                if(dropRatesLookup.ContainsKey((BonusType)bonus))
                    continue;
                
                dropRatesLookup.Add((BonusType)bonus, new BonusDropInfo());
            }
        }
    }
}