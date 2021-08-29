namespace SpaceInvaders
{
    public enum GameStates {
        GameOpened,
        GameInMenu, 
        GameWaitsForStart, 
        GameStarted,
        GamePaused,
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
        // Teleportation,
        // TimeSlowDown, 
        // Hacking,
        // MassiveKill, 
    }

    public enum WeaponTier
    {
        Projectile_1 = 0, 
        Projectile_2 = 1,
        Projectile_3 = 2,
        LaserBeam_1 = 3, 
    }
}