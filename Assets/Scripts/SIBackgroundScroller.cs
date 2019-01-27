using UnityEngine;

namespace SpaceInvaders
{
    [RequireComponent(typeof(MeshRenderer))]
    public class SIBackgroundScroller : MonoBehaviour, IMoveable
    {
        [Range(0.01f, 0.5f)][SerializeField] private float _xScalingFactor;

        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Vector2 _scrollOffset;

        private float _dt;

        protected void OnEnable()
        {
            SIEventsHandler.OnObjectMovement += MoveObj;
        }

        protected void OnDisable()
        {
            SIEventsHandler.OnObjectMovement -= MoveObj;
        }

        public void MoveObj()
        {
            if (_meshRenderer == null)
            {
                return;
            }

            _dt = Time.deltaTime;
            _scrollOffset.x = SIPlayerBehaviour.Instance.PlayerMovemnt.InputMovementValue;
            _meshRenderer.material.mainTextureOffset += (new Vector2(_scrollOffset.x * _xScalingFactor, _scrollOffset.y) * _dt);
        }
    }
}

