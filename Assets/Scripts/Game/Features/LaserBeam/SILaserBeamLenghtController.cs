using UnityEngine;

namespace Game.Features.LaserBeam {
    public class SILaserBeamLenghtController : MonoBehaviour {

        [SerializeField] float _lineImpactOffset;
        [SerializeField] LineRenderer _lineRenderer;

        Vector3 _lineEndPos;
        Vector3[] _lineStartPositions;

        void Start() => Initialise();

        void Initialise() {
            _lineStartPositions = new Vector3[_lineRenderer.positionCount]; 
            _lineRenderer.GetPositions(_lineStartPositions);
        }

        public void SetLineRendererEndPosY(float endPointY) {
            _lineEndPos.y = endPointY + _lineImpactOffset;
            _lineRenderer.SetPosition(1, _lineEndPos);
        }

        public void SetDefaultLineRendererEndPosY() {
            _lineRenderer.SetPosition(1, _lineStartPositions[1]);
        }
    }
}