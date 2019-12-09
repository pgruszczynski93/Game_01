using UnityEngine;

namespace SpaceInvaders
{
    public abstract class SIColliderBehaviour : MonoBehaviour
    {
        [SerializeField] protected CollisionTag _collisionTag;
        [SerializeField] protected CollisionTag[] _collidesWithTags;
        protected ICanCollide _colliderBehaviour;

        bool _initialised;
        int _collidesWithCount;
        
        protected virtual void Initialise()
        {
            if (_initialised)
                return;

            _initialised = true;
            _collidesWithCount = _collidesWithTags.Length;
        }

        protected virtual void Start()
        {
            Initialise();
        }
        protected abstract void HandleOnCollisionDetected();
        protected virtual void AssignEvents() { }
        protected virtual void RemoveEvents() { }
        
        protected virtual void OnEnable()
        {
            AssignEvents();
        }

        protected virtual void OnDisable()
        {
            RemoveEvents();
        }

        protected virtual void OnTriggerEnter(Collider col)
        {
            _colliderBehaviour = col.GetComponent<ICanCollide>();

            if (_colliderBehaviour == null)
                return;

            bool isCollisionDetected = IsCollisionDetected(_colliderBehaviour.GetCollisionTag());
            if (!isCollisionDetected)
                return;
            
            _colliderBehaviour.OnCollisionDetected?.Invoke();
        }

        bool IsCollisionDetected(CollisionTag collisionTag)
        {
            for (int i = 0; i < _collidesWithCount; i++)
            {
                if (_collidesWithTags[i] == collisionTag)
                    return true;
            }

            return false;
        }
    }
}