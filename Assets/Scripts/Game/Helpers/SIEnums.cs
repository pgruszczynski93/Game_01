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

    public enum ProjectileOwnerTag {
        Player, 
        Enemy
    }
    
    public enum BonusType
    {
        Health,
        Weapon,
        ShieldSystem,
        LaserBeam,
        RapidFire,
        TimeSlowDown, 
        // Teleportation,
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