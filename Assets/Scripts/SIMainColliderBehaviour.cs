using System;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIMainColliderBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] protected string _objectTag;
        [SerializeField] protected T _colliderParentBehaviour;

        protected Action onCollisionCallback = delegate { };

        protected virtual void OnEnable()
        {
            if (_colliderParentBehaviour == null)
            {
                Debug.LogError("No root behaviour assigned.");
            }
        }

        protected virtual void OnDisable(){}

        protected void OnTriggerEnter2D(Collider2D collider2D)
        {
            if (String.IsNullOrEmpty(_objectTag))
            {
                Debug.LogError("Specify collision tag first.");
                return;
            }
            if (collider2D.gameObject.CompareTag(_objectTag))
            {
                onCollisionCallback?.Invoke();
            }
        }

    }
}
