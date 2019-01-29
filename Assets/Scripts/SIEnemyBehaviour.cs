using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyBehaviour : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public bool IsEnemyDead { get; set; } = false;

        public void Death()
        {
            if (IsEnemyDead == false)
            {
                IsEnemyDead = true;
                gameObject.SetActive(false);
            }
        }
    }
}
