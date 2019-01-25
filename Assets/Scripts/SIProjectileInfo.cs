using UnityEngine;

namespace SpaceInvaders
{
    [System.Serializable]
    public class SIProjectileInfo
    {
        public WeaponType projectileType;
        public GameObject[] projectilePrefabs;
        public Transform[] projectileParents;
    }
}