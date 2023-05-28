using PG.Game.Helpers;
using UnityEngine;

namespace PG.Game.Configs {
    [System.Serializable]
    public class SIProjectileSettings {
        public float movementLimitOffset; //Value, when weapon hides from the screen.
        public float projectileDamage;
        public float launchForceMultiplier;
        public Vector3 parentRelativePos;
        public Vector3 parentRelativeRotation;
        public Vector3 scaleValues;
        public ProjectileOwnerTag ownerTag;
        public GameObject projectileObject;
    }
}