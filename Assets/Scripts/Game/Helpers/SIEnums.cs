﻿namespace SpaceInvaders
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
        ExtraEnergy,
        TimeSlowDown, 
        // Teleportation,
        // Hacking,
        // MassiveKill, 
    }
}