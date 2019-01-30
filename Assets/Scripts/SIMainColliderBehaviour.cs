using System;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIMainColliderBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private int _objectTagsCount;
        [SerializeField] protected string[] _objectTags;
        [SerializeField] protected T _colliderParentBehaviour;

        protected Action onCollisionCallback = delegate { };    // 

        protected virtual void Start()
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

        protected virtual void OnTriggerEnter2D(Collider2D collider2D)
        {
            bool hasHittedObjectGivenTag = false;

            for (int i = 0; i < _objectTagsCount; i++)
            {
                hasHittedObjectGivenTag = collider2D.gameObject.CompareTag(_objectTags[i]);
                if (hasHittedObjectGivenTag)
                {
                    onCollisionCallback?.Invoke();
                    return;
                }
            }
        }

    }
}
