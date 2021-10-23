namespace SpaceInvaders {
    public interface IModifyTimeSpeedMultiplier {
        void RequestTimeSpeedModification();
        void SetTimeSpeedModifier(float modifier, float progress = 1f);
    }
}