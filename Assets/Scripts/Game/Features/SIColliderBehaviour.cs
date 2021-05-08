using UnityEngine;

namespace SpaceInvaders
{
    public abstract class SIColliderBehaviour : MonoBehaviour
    {
        [SerializeField] protected CollisionInfo _thisCollisionInfo;
        [SerializeField] protected CollisionTag[] _collidableObjects;
        
        protected ICanCollide _triggeringBehaviour;

        bool _initialised;
        int _collidesWithCount;
        CollisionInfo _triggeredCollisionInfo;
        
        protected virtual void Initialise()
        {
            if (_initialised)
                return;

            _initialised = true;
            _collidesWithCount = _collidableObjects.Length;
        }

        protected virtual void Start()
        {
            Initialise();
        }
        protected abstract void HandleOnCollisionDetected(CollisionInfo collisionInfo);
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

        protected virtual void OnTriggerEnter(Collider triggerCol)
        {
            _triggeringBehaviour = triggerCol.GetComponent<ICanCollide>();
            _triggeredCollisionInfo = _triggeringBehaviour.GetCollisionInfo();
            CollisionTag collisionTag = _triggeredCollisionInfo.collisionTag;
            
            if (_triggeringBehaviour == null || !IsCollisionDetected(collisionTag))
                return;
            
            _triggeringBehaviour.
                OnCollisionDetected?.Invoke(_thisCollisionInfo);
        }

        bool IsCollisionDetected(CollisionTag collisionTag)
        {
            for (int i = 0; i < _collidesWithCount; i++)
            {
                if (_collidableObjects[i] == collisionTag)
                    return true;
            }

            return false;
        }

        protected void TryDetectExplosiveHit(CollisionTag collisionTag) {
            //Don't remove: Only Enemy & EnemyWeapon are explosive so that they will spawn explosion at some position. 
            if (collisionTag == CollisionTag.Bonus)
                return;
            
            SIGameplayEvents.BroadcastOnExplosiveObjectHit(transform.position);
        }
    }
}