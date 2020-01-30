namespace SpaceInvaders
{
    public class SIWeaponBonus : SIBonus, IBonus
    {
        public override BonusSettings GetBonusSettings()
        {
            return _bonusSettings;
        }
    }
}