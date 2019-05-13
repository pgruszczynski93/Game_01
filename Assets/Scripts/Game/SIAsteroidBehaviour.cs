using UnityEngine;

namespace SpaceInvaders
{
    public class SIAsteroidBehaviour : MonoBehaviour, IMoveable
    {

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
                return;
            }
        }
        
        public void MoveObj()
        {
            _rigidbody.AddForce(Vector3.forward * 100);
        }
        public void StopObj()
        {
            _rigidbody.velocity = SIHelpers.VectorZero;
        }
    }
}