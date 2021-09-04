using System.Collections.Generic;
using SpaceInvaders;
using UnityEngine;

namespace Game.Features.LaserBeam {
    public class SILaserBeamDamage : MonoBehaviour {

        [SerializeField] int _collisionCheckDistance;
        [SerializeField] float _damage;     // to do - read it from serializefield
        [SerializeField] Vector3 _offsetFromPlayerCollider;
        [SerializeField] LayerMask _collisionLayerMask;
        [SerializeField] CollisionTag[] _tagsOfObjectsToApplyDamage;

        DamageInfo _currentDamageInfo;
        RaycastHit _currentHit;
        Transform _thisTransform;
        CollisionInfo _lastHitObjectInfo;
        Collider _lastHitCollider;

        List<Collider> _collidersHit;
        Dictionary<Collider, CollisionInfo> _collisionCache;
        
        void Start() => Initialise();

        void Initialise() {
            _thisTransform = transform;
            _currentDamageInfo = new DamageInfo(_damage);
            _collisionCache = new Dictionary<Collider, CollisionInfo>();
        }
        void OnEnable() => SubscribeEvents();

        void OnDisable() => UnsubscribeEvents(); 
        
        void SubscribeEvents() {
            SIEventsHandler.OnUpdate += HandleOnUpdate;
        }

        void UnsubscribeEvents() {
            SIEventsHandler.OnUpdate -= HandleOnUpdate;
        }

        void HandleOnUpdate() {
            DetectLaserHit();
        }

        void DetectLaserHit() {
            if (!Physics.Raycast(_thisTransform.position + _offsetFromPlayerCollider, _thisTransform.up,
                out _currentHit, _collisionCheckDistance,_collisionLayerMask))
                return;

            // Debug.DrawRay(_thisTransform.position + _offsetFromPlayerCollider , _thisTransform.up * _collisionCheckDistance, Color.green);
            
            _lastHitCollider = _currentHit.collider;
            if (!_collisionCache.ContainsKey(_lastHitCollider)) {
                CacheLaserHit();
            }
            TryToApplyDamage();
        }

        void CacheLaserHit() {
            _lastHitObjectInfo = _lastHitCollider.gameObject.GetComponent<ICanCollide>().GetCollisionInfo();
            _collisionCache.Add(_lastHitCollider, _lastHitObjectInfo);
        }
        
        void TryToApplyDamage() {
            _lastHitObjectInfo = _collisionCache[_lastHitCollider];
            if (!IsCollisionDetected(_lastHitObjectInfo.collisionTag))
                return;
            
            _currentDamageInfo.ObjectToDamage = _lastHitObjectInfo.collisionSource;
            SIGameplayEvents.BroadcastOnDamage(_currentDamageInfo);
        }
        
        bool IsCollisionDetected(CollisionTag collisionTag)
        {
            for (int i = 0; i < _tagsOfObjectsToApplyDamage.Length; i++)
            {
                if (_tagsOfObjectsToApplyDamage[i] == collisionTag)
                    return true;
            }
            return false;
        }
    }
}