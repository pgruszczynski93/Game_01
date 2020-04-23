namespace SpaceInvaders
{
    [System.Serializable]
    public class GridShootingSettings
    {
        public int maxEnemiesInRow;
        public int startShootingThresholdIndex;
        public int gridLevel;     //to update when grid will have random models
        public float minShootingInterval;
        public float maxShootingInterval;
    }
}