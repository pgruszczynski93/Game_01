using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyBehaviour : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private void OnTriggerEnter2D(Collider2D collider2D)
        {
            if (collider2D.gameObject.CompareTag("Projectile"))
            {
                gameObject.SetActive(false);
            }
        }
    }
}
