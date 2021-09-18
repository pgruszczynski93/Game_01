namespace SpaceInvaders {
    public class SIPlayerHealth : SIHealth {
        public void ToggleImmortality() {
            _isImmortal = !_isImmortal;
        }
    }
}