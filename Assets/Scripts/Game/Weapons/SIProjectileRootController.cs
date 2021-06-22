using Sirenix.OdinInspector;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIProjectileRootController : MonoBehaviour {
        [SerializeField] Transform[] projectileSlotPositionsPositions;
        public Transform[] ProjectilesSlotsPositions => projectileSlotPositionsPositions;

#if UNITY_EDITOR
        [Button]
        //Note: All slots have to be in world pos.
        void SetProjectileSlotPositions() {
            var childCount = transform.childCount;
            projectileSlotPositionsPositions = new Transform[childCount];
            for (int i = 0; i < childCount; i++) {
                projectileSlotPositionsPositions[i] = transform.GetChild(i);;
            }
        }
        #endif
    }
}