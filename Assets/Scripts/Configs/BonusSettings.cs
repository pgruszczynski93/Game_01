namespace SpaceInvaders
{
    [System.Serializable]
    public class BonusSettings
    {
        public BonusType bonusType;
        public int gainedHealth;
        public int gainedScore;
        public float durationTime;
        public float releaseForceMultiplier;
    }
}