using UnityEngine;

namespace SpaceInvaders
{
    public class SIBonusDropper : MonoBehaviour
    {
//        [SerializeField] SIBonusSelectorSettings _selectorSettings;
        [SerializeField] SIBonus _bonusBehaviour;

        float _bonusDropChance;
        
        void Initialise()
        {
//            _bonusDropChance = _selectorSettings.dropChance;
        }
        void OnEnable()
        {
            AssignEvents();
        }

        void OnDisable()
        {
            RemoveEvents();
        }

        void AssignEvents()
        {
            SIEventsHandler.OnEnemyDeath += HandleOnEnemyDeath;
        }

        void RemoveEvents()
        {
            SIEventsHandler.OnEnemyDeath -= HandleOnEnemyDeath;
        }

        void HandleOnEnemyDeath(SIEnemyBehaviour enemyBehaviours)
        {
            
        }
    }
}