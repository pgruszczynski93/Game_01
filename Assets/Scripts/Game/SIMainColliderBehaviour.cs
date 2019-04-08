using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIMainColliderBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private int _objectTagsCount;
        [SerializeField] protected string[] _objectTags;
        [SerializeField] protected T _colliderParentBehaviour;

        protected delegate void CustomCollisonDelegate(MonoBehaviour collisionBehaviour = null);

        protected Dictionary<string, CustomCollisonDelegate> _onCollisionActions;

        protected virtual void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _objectTagsCount = _objectTags.Length;

            if (AreAllTagsCorrect() == false)
            {
                SIHelpers.SISimpleLogger(this, "Enter collision tags first", SimpleLoggerTypes.Error);
                return;
            }

            _onCollisionActions = new Dictionary<string, CustomCollisonDelegate>()
            {
                {SIStringTags.PLAYER, delegate { }},
                {SIStringTags.ENEMY, delegate { }},
                {SIStringTags.PROJECTILE, delegate { }},
                {SIStringTags.ENEMY_PROJECTILE, delegate { }},
                {SIStringTags.BONUS, delegate { }}
            };
        }

        private bool AreAllTagsCorrect()
        {
            for (int i = 0; i < _objectTagsCount; i++)
            {
                if (string.IsNullOrEmpty(_objectTags[i]))
                {
                    return false;
                }
            }

            return true;
        }

        protected virtual void OnEnable()
        {
            if (_colliderParentBehaviour == null)
            {
                Debug.LogError("No root behaviour assigned.");
            }
        }

        protected virtual void OnDisable(){}

        protected virtual void OnTriggerEnter(Collider triggerCollider)
        {
            
            // ZDEBUGPOWAC TO!!!!!!!!!!!!!!!!!!!
            // problem z colliderami
            bool hasHittedObjectGivenTag = false;

            GameObject hittedObject = triggerCollider.gameObject;

            if(hittedObject.CompareTag(SIStringTags.BONUS))
            {
                _onCollisionActions[SIStringTags.BONUS]?.Invoke(hittedObject.GetComponent<SIBonus>());
                return;
            }

            for (int i = 0; i < _objectTagsCount; i++)
            {
                hasHittedObjectGivenTag = hittedObject.CompareTag(_objectTags[i]);

                if (hasHittedObjectGivenTag)
                {
                    _onCollisionActions[_objectTags[i]]?.Invoke();
                    return;
                }
            }
        }

    }
}
