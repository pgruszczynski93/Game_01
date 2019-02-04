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

        protected Action onCollisionCallback = delegate { };    // obsolete - remove later

        protected Dictionary<string, CustomCollisonDelegate> _onCollisionActions;

        protected virtual void Awake()
        {
            SetInitialReferences();
        }

        private void SetInitialReferences()
        {
            _objectTagsCount = _objectTags.Length;

            if (AreAllTagsCorrect() == false)
            {
                Debug.LogError("Specify collision tags first.");
                return;
            }

            _onCollisionActions = new Dictionary<string, CustomCollisonDelegate>()
            {
                {"Player", delegate { }},
                {"Enemy", delegate { }},
                {"Projectile", delegate { }},
                {"EnemyProjectile", delegate { }},
                {"Bonus", delegate { }}
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

        protected virtual void OnTriggerEnter(Collider collider)
        {
            bool hasHittedObjectGivenTag = false;

            if(collider.gameObject.CompareTag("Bonus"))
            {
                _onCollisionActions["Bonus"]?.Invoke(collider.gameObject.GetComponent<SIBonus>());
                return;
            }

            for (int i = 0; i < _objectTagsCount; i++)
            {
                hasHittedObjectGivenTag = collider.gameObject.CompareTag(_objectTags[i]);

                if (hasHittedObjectGivenTag)
                {
                    _onCollisionActions[_objectTags[i]]?.Invoke();
                    return;
                }
            }
        }

    }
}
