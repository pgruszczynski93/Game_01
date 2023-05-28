using Sirenix.OdinInspector;
using UnityEngine;

namespace PG.Game.Weapons.Projectile
{
    public class ProjectileSlotsParentController : MonoBehaviour {
        [SerializeField] Transform[] projectileSlotTransforms;
        public Transform[] ProjectilesSlotsTransforms => projectileSlotTransforms;

#if UNITY_EDITOR
        [Button]
        //Note: All slots have to be in world pos.
        void SetProjectileSlotTransforms() {
            var childCount = transform.childCount;
            projectileSlotTransforms = new Transform[childCount];
            for (int i = 0; i < childCount; i++) {
                projectileSlotTransforms[i] = transform.GetChild(i);;
            }
        }
        #endif
    }
}