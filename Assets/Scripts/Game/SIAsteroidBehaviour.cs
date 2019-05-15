using UnityEngine;

namespace SpaceInvaders
{
    public class SIAsteroidBehaviour : MonoBehaviour, IMoveable
    {

        private bool _isMoving;
        
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
        }
        
        public void MoveObj()
        {
            if (_isMoving != false)
            {
                return;
            }
            _rigidbody.AddForce(Vector3.left * 100);
            _isMoving = true;
        }
        public void StopObj()
        {
            _rigidbody.velocity = SIHelpers.VectorZero;
        }
    }
}