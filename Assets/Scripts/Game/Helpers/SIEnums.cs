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
        PlayerBonusCollider
    }

    public enum ProjectileOwnerTag {
        Player, 
        Enemy
    }
    
    public enum BonusType
    {
        Health,
        Projectile,
        ShieldSystem,
        LaserBeam,
        EnergyBoost,
        //Time Slow/Fast All - Slow All (slows every object except player) / Fast All (oposite)
        TimeModSlowAll, 
        TimeModeFastAll
        // Teleportation,
        // Hacking,
        // MassiveKill, 
    }
}