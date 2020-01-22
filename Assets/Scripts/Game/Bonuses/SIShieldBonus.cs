using System;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIShieldBonus : SIBonus
    {
        public override BonusSettings GetBonusSettings()
        {
            return _bonusSettings;
        }
    }
}