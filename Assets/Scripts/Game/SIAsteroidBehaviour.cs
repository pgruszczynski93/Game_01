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
        }

        public void StopObj()
        {
            
        }
    }
}