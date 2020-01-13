using System;

namespace SpaceInvaders
{
    public class SILifeBonus : SIBonus, IBonus
    {
        public Action OnBonusStarted { get; set; }
        public Action OnBonusActive { get; set; }
        public Action OnBonusFinished { get; set; }
    }
}