using DG.Tweening;
using UnityEngine;

namespace SpaceInvaders
{
    public abstract class SIMovementBehaviour : MonoBehaviour
    {
        protected bool _canMove;
        protected bool _initialised;

        [SerializeField] protected float _initialMovementSpeed;
        [SerializeField] protected float _currentMovementSpeed;

        protected float _dt;
        protected Transform _thisTransform;
        protected Tweener _tweener;

        protected abstract void UpdatePosition();
        protected abstract void UpdateRotation();
        protected virtual void TryToMoveObject() { }
        protected virtual void TryToStopObject() { }

        protected virtual void Initialise()
        {
            if (_initialised)
                return;

            _initialised = true;
            _thisTransform = transform;
//                        
//            _tweener = _thisTransform
//                .DOMove(Vector3.zero, _playerMovementSettings.easeTime)
//                .SetEase(_playerMovementSettings.easeType)
//                .SetAutoKill(false)
//                .Pause();
        }

        protected void Start()
        {
            Initialise();
        }

        protected virtual void OnEnable()
        {
            AssignEvents();
        }

        protected virtual void OnDisable()
        {
            RemoveEvents();
        }

        protected virtual void AssignEvents() { }

        protected virtual void RemoveEvents() { }

        protected virtual void ResetMovement() { }

        protected void PlayTween(Vector3 newTargetPos)
        {
            _tweener
                .ChangeEndValue(newTargetPos, true)
                .Restart();
        }
    }
}