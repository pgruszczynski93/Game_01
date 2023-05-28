namespace PG.Game.Features {
    public interface IModifyTimeSpeedMultiplier {
        void RequestTimeSpeedModification();
        void SetTimeSpeedModifier(float timeSpeedModifier, float progress = 1f);
    }
}