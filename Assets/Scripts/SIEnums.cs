﻿namespace SpaceInvaders
{
    public enum ShaderProperties
    {
        EmissionPower,
    }

    public enum VFXActions
    {
        EnableVFX,
        EnableAndDetachVFX,
        EnableAndAttachVFX
    }

    public enum SimpleLoggerTypes
    {
        Log, 
        Warning,
        Error
    }

    public enum MovementDirection
    {
        Up,
        Down,
        Left, 
        Right
    }

    public enum BonusType
    {
        Life,
        Shield,
        Weapon,
    }

    public enum MovementType
    {
        Basic,
        Fast,
        Slow
    }

    public enum EnemyType
    {
        Basic, 
        Special
    }

    public enum CollectibleLevel
    {
        Zero,
        First, 
        Second,
        Third,
        Fourth,
        Fifth,
        Sixth,
        Seventh,
        Eight,
        Nineth,
        Tenth
    }

    public enum AddedTags
    {

    }

    public enum Events
    {
        OnPlayerMove, 
        OnPlayerShoot
    }


}