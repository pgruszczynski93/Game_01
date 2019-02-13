using UnityEngine;

namespace MinesEvaderMinimal
{
    
    /// <summary>
    /// The mines in the complete package could be somewhat challenging to understand
    /// if you are new to Unity because they use inheritance, so this can be a simplified
    /// version of it No explosions, events or collisions, just finding the player and
    /// its distance and damaging the player with an explosion.
    /// </summary>
    public class MineMini : MonoBehaviour
    {

        public GameObject ExplosionObject;

        public float StartSpeed;
        public float EndSpeed;

        public float DetonationTime;

        public float DetectionRadius;
        public float DetonationRadius;
        public float ExplosionRadius;
 
        float distance;
        float speed;

        Vector3 target;
        Rigidbody2D rb;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            ExplosionObject.SetActive(false);
        }

        void FixedUpdate()
        {
            if (GameManagerMini.PlayerGO == null)
            {
                return;
            }
            

            target = (GameManagerMini.PlayerGO.transform.position - transform.position);
            distance = target.magnitude;

            speed = Mathf.Lerp(EndSpeed, StartSpeed, (distance/DetectionRadius));
            rb.velocity = target.normalized * speed;

            if (distance < DetonationRadius)
            {
                ExplosionObject.SetActive(true);
                Invoke("Destroy", DetonationTime);
            }
        }

        void Destroy()
        {
            if (distance < ExplosionRadius)
            {
                GameManagerMini.DamagePlayer();
            } 
            Destroy(gameObject);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, DetectionRadius);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, DetonationRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, ExplosionRadius);
        }
    }
}

