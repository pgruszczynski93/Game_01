using PG.Game.Configs;
using PG.Game.Helpers;

namespace PG.Game.Bonuses {
    [System.Serializable]
    public class BonusesDictionary : SerializableDictionary<BonusType, BonusData> { }

    [System.Serializable]
    public class BonusesDropRatesLookup : SerializableDictionary<BonusType, BonusDropInfo> { }
}