using System;

namespace SpaceInvaders
{
    public class SILifeBonus : SIBonus, IBonus
    {
        public override BonusSettings GetBonusSettings()
        {
            return _bonusSettings;
        }
    }
}