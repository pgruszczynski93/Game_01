using System;
using PG.Game.Configs;

namespace PG.Game.EventSystem {
    public class BonusesEvents {
        public static event Action<BonusSettings> OnBonusCollected;
        public static event Action<BonusSettings> OnBonusEnabled;
        public static event Action<BonusSettings> OnBonusDisabled;

        public static void BroadcastOnBonusEnabled(BonusSettings bonusSettings) {
            OnBonusEnabled?.Invoke(bonusSettings);
        }

        public static void BroadcastOnBonusDisabled(BonusSettings bonusSettings) {
            OnBonusDisabled?.Invoke(bonusSettings);
        }

        public static void BroadcastOnBonusCollected(BonusSettings bonusSettings) {
            OnBonusCollected?.Invoke(bonusSettings);
        }
    }
}