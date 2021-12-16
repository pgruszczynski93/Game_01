using System.Collections.Generic;
using Configs;
using SpaceInvaders;
using UnityEngine;

namespace Game.Features.LaserBeam {
    public class SILaserBeamDamage : MonoBehaviour {

        [SerializeField] LaserBeamDamageSettings _damageSettings;
        [SerializeField] Vector3 _offsetFromPlayerCollider;
        [SerializeField] LayerMask _collisionLayerMask;
        [SerializeField] CollisionTag[] _tagsOfObjectsToApplyDamage;
        [SerializeField] SILaserBeamVfxController _laserVfxController;

        float _currentDamage;
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
            _currentDamage = _damageSettings.basicDamage;
            _currentDamageInfo = new DamageInfo(_currentDamage);
            _collisionCache = new Dictionary<Collider, CollisionInfo>();
        }
        void OnEnable() => SubscribeEvents();

        void OnDisable() => UnsubscribeEvents(); 
        
        void SubscribeEvents() {
            SIEventsHandler.OnUpdate += HandleOnUpdate;
            SIBonusesEvents.OnBonusEnabled += HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled += HandleOnBonusDisabled;
        }

        void UnsubscribeEvents() {
            SIEventsHandler.OnUpdate -= HandleOnUpdate;
            SIBonusesEvents.OnBonusEnabled -= HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled -= HandleOnBonusDisabled;
        }

        void HandleOnUpdate() {
            DetectLaserHit();
        }
        
        void HandleOnBonusEnabled(BonusSettings bonusSettings) {
            switch(bonusSettings.bonusType) {
                case BonusType.Health:
                    break;
                case BonusType.Projectile:
                    break;
                case BonusType.ShieldSystem:
                    break;
                case BonusType.LaserBeam:
                    break;
                case BonusType.ExtraEnergy:
                    SetNewDamage(_damageSettings.extraDamage);
                    _laserVfxController.EnableExtraEnergyVfx(true);
                    break;
                case BonusType.TimeSlowDown:
                    break;
            }
        }

        void HandleOnBonusDisabled(BonusSettings bonusSettings) {
            switch(bonusSettings.bonusType) {
                case BonusType.Health:
                    break;
                case BonusType.Projectile:
                    break;
                case BonusType.ShieldSystem:
                    break;
                case BonusType.LaserBeam:
                    break;
                case BonusType.ExtraEnergy:
                    SetNewDamage(_damageSettings.basicDamage);
                    _laserVfxController.EnableExtraEnergyVfx(false);
                    break;
                case BonusType.TimeSlowDown:
                    break;
            }
        }

        void SetNewDamage(float newDamage) {
            _currentDamage = newDamage;
            _currentDamageInfo.SetDamage(_currentDamage);
        }

        void DetectLaserHit() {
            if (!_laserVfxController.LaserMainVfx.activeInHierarchy)
                return;
            
            if (!Physics.Raycast(_thisTransform.position + _offsetFromPlayerCollider, _thisTransform.up,
                out _currentHit, _damageSettings.collisionCheckDistance, _collisionLayerMask)) {
                _laserVfxController.SetDefaultLineRendererEndPosY();
                return;
            }

            // Debug.DrawRay(_thisTransform.position + _offsetFromPlayerCollider , _thisTransform.up * _collisionCheckDistance, Color.green);
            
            _lastHitCollider = _currentHit.collider;
            _laserVfxController.SetLineRendererEndPosY(_currentHit.point.y);
            
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
            if (!IsCollisionDetected(_lastHitObjectInfo.collisionTag)) {
                _laserVfxController.SetDefaultLineRendererEndPosY();
                return;
            }
            
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