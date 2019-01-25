using UnityEngine;

namespace SpaceInvaders
{
    [RequireComponent(typeof(MeshRenderer))]
    public class SIBackgroundScroller : MonoBehaviour, IMoveable
    {
        [Range(0.01f, 0.5f)][SerializeField] private float _xScalingFactor;

        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Vector2 _scrollOffset;

        protected void OnEnable()
        {
            SIEventsHandler.OnPlayerMove += MoveObj;
        }

        protected void OnDisable()
        {
            SIEventsHandler.OnPlayerMove -= MoveObj;
        }

        public void MoveObj()
        {
            if (_meshRenderer == null)
            {
                return;
            }
            _scrollOffset.x = SIPlayerBehaviour.Instance.PlayerMovemnt.InputMovementValue;
            _meshRenderer.material.mainTextureOffset += new Vector2(_scrollOffset.x * _xScalingFactor, _scrollOffset.y * Time.deltaTime);
        }
    }
}

