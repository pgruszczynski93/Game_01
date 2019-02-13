using UnityEngine;
using UnityEngine.Events;

namespace MinesEvader
{
    
    /// <summary>
    /// The partial class trick is only to make the MineBase class invisible when you add
    /// a component from the menu. Otherwise you could mistakingly add the Base class instead
    /// of using the Track or Chain Mine.
    /// </summary>
    public partial class NotComponent
    {

        [RequireComponent(typeof(Rigidbody2D))]

        /// <summary>
        /// This is the base class for all mines, many components are shared between the
        /// two different mines so we added this class so we do not have to repeat anything.
        /// </summary>
        public class MineBase : MonoBehaviour
        {
            
            #region Declarations

            public static int NumberOfMines;

            public UnityEvent DamagePlayer;
            public UnityEvent OnDestroy;

            [Tooltip("Raised when the player has entered the mine detonation radius.")]
            public UnityEvent StartDetonation;

            public float StartSpeed;
            public float DetectionRadius;

            [Tooltip("When the Player enters this radius; the mine will be triggered" +
                " and explode after the detonation timer expires")]
            public float DetonationRadius;
            public float DetonationTimer;
            public float ExplosionRadius;

            [Space]
            public bool AllowCollisionDamage;
            public bool RandomStart3DRotation;

            [Space]
            [Tooltip("Played when mine starts a process of detenotation. This is different" +
                " from the explosion clip used inside the explosion game object")]
            public AudioClip DetonationClip;

            protected Rigidbody2D rb;

            /// <summary>
            /// Normalised direction towards player.
            /// </summary>
            protected Vector3 Direction;

            /// <summary>
            /// Distance between mine and player.
            /// </summary>
            protected float Distance; 

            Vector3 Target;

            bool Detonation;

            #endregion

            void OnEnable()
            {
                NumberOfMines++;
            }

            void OnDisable()
            {
                NumberOfMines--;
            }

            protected virtual void Start()
            {
                rb = GetComponent<Rigidbody2D>();
                rb.gravityScale = 0;

                if (RandomStart3DRotation) transform.rotation = Random.rotation;
            }

            protected virtual void FixedUpdate()
            {                        

                if (!GameManager.player)
                {
                    return;
                }

                Target = (GameManager.player.transform.position - transform.position);
                Direction = Target.normalized;
                Distance = Target.magnitude;

                /// <summary>
                /// This triggers the mine to explode after the detonation timer and invokes
                /// the start detonation event so you could apply the flash effect to it once
                /// the Player enters this radius once, there is no need to test it again, 
                /// because the detonation process would have started
                /// </summary>
                if (Detonation == false && Distance < DetonationRadius)
                {
                    GameManager.audioSource.PlayOneShot(DetonationClip);
                    Invoke("Destroy", DetonationTimer);
                    StartDetonation.Invoke();
                    Detonation = true;
                }

            }

            protected virtual void OnCollisionEnter2D(Collision2D col)
            {
                if (((col.gameObject.GetComponent<Player>()) != null) && (AllowCollisionDamage))
                {
                    DamagePlayer.Invoke();
                }              
            }

            protected void Destroy()
            {               
                if (Distance < ExplosionRadius)
                {
                    DamagePlayer.Invoke();
                }

                OnDestroy.Invoke();
                Destroy(gameObject);
            }

            protected void OnDrawGizmosSelected()
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
}
