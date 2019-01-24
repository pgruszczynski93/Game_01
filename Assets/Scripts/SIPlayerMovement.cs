using UnityEngine;

namespace SpaceInvaders
{
    public class SIPlayerMovement : SIMovement
    {
        [Range(0,1)] [SerializeField] private float _lerpStep;

        private Transform _cachedTransform;
        private Vector2 _startPosition;
        private Vector2 _currentPosition;

        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();

            _cachedTransform = transform;
            _startPosition = _cachedTransform.position;
            _currentPosition = _startPosition;
            _currentMovementSpeed = BASIC_SPEED;
        }

        protected override void OnEnable()
        {
            EventsHandler.OnPlayerMove += MoveObject;
        }

        protected override void OnDisable()
        {
            EventsHandler.OnPlayerMove -= MoveObject;
        }

        protected override void MoveObject()
        {
            base.MoveObject();

            float dt = Time.deltaTime;
            float horizontalMoveDelta = Input.GetAxis("Horizontal") * _currentMovementSpeed * dt;

            _currentPosition += new Vector2(horizontalMoveDelta, 0f);

            _cachedTransform.position = Vector2.Lerp(_cachedTransform.position, _currentPosition , _lerpStep);
        }
    }
}