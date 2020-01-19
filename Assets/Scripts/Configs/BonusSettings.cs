using System.Net.Security;

namespace SpaceInvaders
{
    [System.Serializable]
    public class BonusSettings
    {
        public BonusType bonusType;
        public BonusProperties bonusProperties;
    }

    [System.Serializable]
    public class BonusProperties
    {
        public int gainedHealth;
        public int gainedScore;
        public float durationTime;
        public float releaseForceMultiplier;
    }
}