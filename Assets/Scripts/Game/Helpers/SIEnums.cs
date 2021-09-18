namespace SpaceInvaders
{
    public enum GameStates {
        GameOpened,
        GameStarted,
        GamePaused,
        GameFinished,
        GameQuit
    }
    public enum AsteroidState
    {
        ReadyToMove,
        OnScreen,
    }

    public enum CollisionTag
    {
        Player, 
        Enemy, 
        PlayerWeapon,
        EnemyWeapon,
        Bonus,
    }
    public enum BonusType
    {
        Life,
        Weapon,
        ShieldSystem,
        LaserBeam,
        // Teleportation,
        // TimeSlowDown, 
        // Hacking,
        // MassiveKill, 
    }

    public enum WeaponTier
    {
        Projectile_1,
        Projectile_2,
        Projectile_3,
        LaserBeam_1
    }
}