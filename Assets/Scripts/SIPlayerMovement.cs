using UnityEngine;

namespace SpaceInvaders
{
    public class SIPlayerMovement : SIMovement
    {

        [SerializeField] private Transform _cachedTransform;

        private Vector2 _startPosition;
        private Vector2 _currentTranslation;

        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();

            _cachedTransform = transform;
            _startPosition = _cachedTransform.position;
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

            float horizontalAxisValue = Input.GetAxis("Horizontal");

            float dt = Time.deltaTime;
            float horizontalMoveDelta = horizontalAxisValue * _currentMovementSpeed * dt;

            _cachedTransform.Translate(horizontalMoveDelta,0,0);

            //_cachedTransform.Translate(horizontalMoveDelta, 0, 0);
            //_currentTranslation += new Vector2(horizontalMoveDelta, _startPosition.y);

            //_cachedTransform.position = new Vector2(Vector2.Lerp(_cachedTransform.position, _currentTranslation, dt).x, _startPosition.y);
        }
    }
}