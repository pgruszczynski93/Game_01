using UnityEngine;

namespace SpaceInvaders
{
    public class SIAsteroidBehaviour : MonoBehaviour, IMoveable
    {

        private bool _isMoving;
        private Transform _cachedTransform;
        
        private SIPlayerBehaviour _player;
        [SerializeField] private Rigidbody _rigidbody;

        private void Start()
        {
            Initialize();
        }
        
        private void Initialize()
        {
            if (_rigidbody == null)
            {
                Debug.LogError("No rigidbody attached.", this);
            }

            _cachedTransform = transform;
            _player = SIGameMasterBehaviour.Instance.Player;
            Vector3 toPlayerDirection = (_player.transform.position - _cachedTransform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(toPlayerDirection, Vector3.up);
            _cachedTransform.rotation = lookRotation;
        }
        
        public void MoveObj()
        {
            if (_isMoving != false)
            {
                return;
            }
            _rigidbody.AddForce(_cachedTransform.forward * 5f, ForceMode.Impulse);
            _isMoving = true;
        }
        public void StopObj()
        {
            _rigidbody.velocity = SIHelpers.VectorZero;
        }
    }
}