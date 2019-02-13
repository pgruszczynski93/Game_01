using UnityEngine;

namespace SpaceInvaders
{
    [RequireComponent(typeof(SIBonusColliderBehaviour))]
    public class SIBonus : MonoBehaviour
    {
        [SerializeField] private SIBonusInfo _bonusInfo;

        public SIBonusInfo BonusInfo { get => _bonusInfo; set => _bonusInfo = value; }


    }

}
