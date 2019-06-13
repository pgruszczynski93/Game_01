using UnityEngine;

namespace SpaceInvaders
{
    [RequireComponent(typeof(MeshRenderer))]
    public class SIBackgroundScroller : MonoBehaviour, IMoveable
    {
        [Range(-1f, 1f)][SerializeField] private float _xScalingFactor;
        [Range(-1f, 1f)][SerializeField] private float _yScalingFactor;

        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Vector2 _scrollOffset;

        private float _dt;

        protected void OnEnable()
        {
            AssignEvents();
        }
        
        protected void OnDisable()
        {
            RemoveEvents();
        }


        private void AssignEvents()
        {
            SIEventsHandler.OnGameIndependentObjectsMovement += MoveObj;
        }

        private void RemoveEvents()
        {
            SIEventsHandler.OnGameIndependentObjectsMovement -= MoveObj;
        }

        public void MoveObj()
        {
            if (_meshRenderer == null)
            {
                return;
            }

            _dt = Time.deltaTime;
            _scrollOffset.x = SIPlayerBehaviour.Instance.PlayerMovemnt.InputMovementValue;
            _meshRenderer.material.mainTextureOffset += (new Vector2(_scrollOffset.x * _xScalingFactor, _scrollOffset.y * _yScalingFactor) * _dt);
        }

        public void StopObj()
        {
            throw new System.NotImplementedException();
        }
    }
}

