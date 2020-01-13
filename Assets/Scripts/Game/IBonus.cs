using System;

namespace SpaceInvaders
{
    public interface IBonus
    {
        Action OnBonusStarted{ get; set; }
        Action OnBonusActive { get; set; }
        Action OnBonusFinished { get; set; }
    }
}