using UnityEngine;

namespace PG.Game.Features.LaserBeam {
    public class LaserBeamVfxController : MonoBehaviour {
        [SerializeField] float _offsetFromPlayerCollider;
        [SerializeField] LineRenderer _lineRenderer;
        [SerializeField] GameObject _laserMainVfx;
        [SerializeField] GameObject _laserExtraEnergyVfx;

        Vector3 _lineEndPos;
        Vector3[] _lineStartPositions;

        public GameObject LaserMainVfx => _laserMainVfx;

        void Start() => Initialise();

        void Initialise() {
            _lineStartPositions = new Vector3[_lineRenderer.positionCount];
            _lineRenderer.GetPositions(_lineStartPositions);
        }

        public void SetLineRendererEndPosY(float endPointY) {
            _lineEndPos.y = endPointY + _offsetFromPlayerCollider;
            _lineRenderer.SetPosition(1, _lineEndPos);
        }

        public void SetDefaultLineRendererEndPosY() {
            _lineRenderer.SetPosition(1, _lineStartPositions[1]);
        }

        public void EnableExtraEnergyVfx(bool isEnabled) {
            _laserExtraEnergyVfx.SetActive(isEnabled);
        }
    }
}