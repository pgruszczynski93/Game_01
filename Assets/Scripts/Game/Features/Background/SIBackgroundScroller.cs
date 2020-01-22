using UnityEngine;

namespace SpaceInvaders
{
    [RequireComponent(typeof(MeshRenderer))]
    public class SIBackgroundScroller : MonoBehaviour, ICanMove
    {
        [Range(-1f, 1f)] [SerializeField] private float _xScalingFactor;
        [Range(-1f, 1f)] [SerializeField] private float _yScalingFactor;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Vector2 _scrollOffset;

        private bool _initialised;
        private bool _isScrollingPossible;
        private float _dt;
        private float _horizontalScrollValue;

        void Initialise()
        {
            if (_meshRenderer == null)
            {
                Debug.LogError("No mesh renderer!", this);
                return;
            }

            if (_initialised)
                return;

            _initialised = true;
            _isScrollingPossible = true;
        }

        void Start()
        {
            Initialise();
        }

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
            SIEventsHandler.OnNonPlayableUpdate += MoveObject;
            SIEventsHandler.OnAxesInputReceived += HandleAxesInputReceived;
        }

        private void RemoveEvents()
        {
            SIEventsHandler.OnNonPlayableUpdate -= MoveObject;
            SIEventsHandler.OnAxesInputReceived -= HandleAxesInputReceived;
        }

        private void HandleAxesInputReceived(Vector3 inputVector)
        {
            _horizontalScrollValue = inputVector.x;
        }

        public void MoveObject()
        {
            if(_isScrollingPossible == false)
                return;
                
            UpdateScrolling();
        }

        private void UpdateScrolling()
        {
            _dt = Time.deltaTime;
            _meshRenderer.material.mainTextureOffset +=
                (new Vector2(_horizontalScrollValue * _xScalingFactor, _scrollOffset.y * _yScalingFactor) * _dt);
        }

        public void StopObject()
        {
            if (_isScrollingPossible)
                return;
        }
    }
}