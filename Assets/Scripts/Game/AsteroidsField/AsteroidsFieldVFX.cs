using SpaceInvaders;

namespace Game.AsteroidsField {
    public class AsteroidsFieldVFX : VFXBehaviour{
        
        //przerobić vfxbehaviour tak, by klasy korzystajace z efektow mialy ich swoje instancje
        
        
        public override void ManageScreenVisibility() {
            if (_thisTransform && SIScreenUtils.IsInVerticalWorldScreenLimit(_thisTransform.position.y))
                return;
        }
    }
}