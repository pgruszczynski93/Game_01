using PG.Game.Systems;
using PG.Game.VFX;
using UnityEngine;

namespace PG.Game.AsteroidsField {
    public class AsteroidsFieldVFX : VFXBehaviour {
        Vector3 _vfxSize;
        Vector3 _vfxHalfSize;
        ParticleSystem.ShapeModule _shapeModule;
        public Vector3 VfxSize => _vfxSize;

        protected override void Initialise() {
            base.Initialise();
            _shapeModule = _particles.shape;
            _vfxSize = _shapeModule.scale;
            _vfxHalfSize = new Vector3(_vfxSize.x * 0.5f, _vfxSize.y * 0.5f, _vfxSize.z * 0.5f);
        }

        public override void ManageScreenVisibility() {
            if (_thisTransform && ScreenUtils.IsInVerticalWorldScreenLimit(_thisTransform.position.y, _vfxHalfSize.y, -_vfxHalfSize.y))
                return;
        }
    }
}