using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyBehaviour : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private bool _isEnemyDead = false;

        private void OnTriggerEnter2D(Collider2D collider2D)
        {
            if (collider2D.gameObject.CompareTag("Projectile") && _isEnemyDead == false)
            {
                Death();
                SIEventsHandler.OnEnemyDeath?.Invoke();
            }
        }

        private void Death()
        {
            _isEnemyDead = true;
            gameObject.SetActive(false);
        }
    }
}
