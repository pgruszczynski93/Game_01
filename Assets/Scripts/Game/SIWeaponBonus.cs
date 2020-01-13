using System;

namespace SpaceInvaders
{
    public class SIWeaponBonus : SIBonus, IBonus
    {
        public Action OnBonusStarted { get; set; }
        public Action OnBonusActive { get; set; }
        public Action OnBonusFinished { get; set; }
    }
}