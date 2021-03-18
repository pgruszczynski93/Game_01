namespace SpaceInvaders {
    [System.Serializable]
    public class SIBonusDictionary : SerializableDictionary<BonusType, SIBonusData> { }

    [System.Serializable]
    public class SIBonusDropRatesLookup : SerializableDictionary<BonusType, BonusDropInfo> { }
}