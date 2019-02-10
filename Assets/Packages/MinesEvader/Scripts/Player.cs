using UnityEngine;
using UnityEngine.Events;

namespace MinesEvader
{

    ///<summary>
    /// The main script for moving the player, setting health and cool down time and for 
    /// raising the OnDestroy event.
    ///</summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {

        #region Declarations

        public UnityEvent OnDestroy;

        public float Speed = 10.0f;
        public float RotationSpeed = 10.0f;

        [Space]
        public int Health = 5;       
        public int CurrentHealth
        {
            get
            {
                return currentHealth;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }

                currentHealth = value;
            }
        }
        int currentHealth;

        [Space]
        public float DamageCoolDownTime;
        public float DamageCoolDownCounter { get; private set; }

        [Space]
        [Tooltip("Audio Clip to be played when the player is damaged.")]
        public AudioClip GetHitClip;

        PlayerAnimation playerAnimation;      
        Rigidbody2D rb;   

        #endregion

        void Awake()
        {
            currentHealth = Health;
        }

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0;

            //The Player Animation component is an extra component added to the player 
            // if you want to make some animation for the player.
            playerAnimation = GetComponent<PlayerAnimation>();
        }

        void Update()
        {
            DamageCoolDownCounter -= Time.deltaTime;
        }

        void FixedUpdate()
        {
            // This does not take any input if you are using a mobile device, input from
            // mobile devices is taken using the moving methods near the end of this script.
            float verticalInput = Input.GetAxis("Vertical");
            float horizontalInput = Input.GetAxis("Horizontal");


            // The verticalInput is multiplied by transform.up to convert it into
            // a vector3 directed upwards.
            rb.AddForce (verticalInput * transform.up * Speed, ForceMode2D.Force); 

            rb.AddTorque (-horizontalInput * RotationSpeed, ForceMode2D.Force);

            // Only active if the PlayerAnimation component is added.
            if (playerAnimation != null)
            {
                if (horizontalInput > 0)
                {
                    playerAnimation.TurnRightAnimation();
                }
                else 
                if (horizontalInput < 0)
                {
                    playerAnimation.TurnLeftAnimation();
                } 
                else playerAnimation.StopTurningAnimation();
            }

            #region Clamping Velocity, Rotation Speed and Position.

            if (rb.velocity.magnitude >= Speed)
            {
                rb.velocity = rb.velocity.normalized * Speed;
            }

            if (rb.angularVelocity >= RotationSpeed)
            {
                rb.angularVelocity = RotationSpeed;
            }

            if (rb.angularVelocity <= -RotationSpeed)
            {
                rb.angularVelocity = -RotationSpeed;
            }


            float clampX = Mathf.Clamp (rb.position.x, 
                GameManager.gameField.xMin, GameManager.gameField.xMax);

            float clampY = Mathf.Clamp (rb.position.y, 
                GameManager.gameField.yMin, GameManager.gameField.yMax);

            rb.position = new Vector3 (clampX, clampY);

            #endregion
        }

        public void TakeDamage(int damage)
        {
            // Player is only damaged if cool down time is zero or less.
            if (DamageCoolDownCounter <= 0)
            {
                CurrentHealth -= damage;
                GameManager.audioSource.PlayOneShot(GetHitClip);

                if (CurrentHealth <= 0)
                {
                    OnDestroy.Invoke();
                    Destroy(gameObject);
                }

                // Resetting the cool down counter.
                DamageCoolDownCounter = DamageCoolDownTime;
            }
        }

        #region Moving Methods Used With Mobile Devices

        public void MoveForward()
        {
            if (playerAnimation == null)
            {
                return;
            }

            rb.AddForce(transform.up * Speed, ForceMode2D.Force);
            playerAnimation.StopTurningAnimation();
        }

        public void MoveBackward()
        {           
            if (playerAnimation == null)
            {
                return;
            }

            rb.AddForce(-transform.up * Speed, ForceMode2D.Force);
            playerAnimation.StopTurningAnimation();
        }

        public void TurnRight()
        {

            if (playerAnimation == null)
            {
                return;
            }

            rb.AddTorque(-RotationSpeed, ForceMode2D.Force);
            playerAnimation.TurnRightAnimation();
        }

        public void TurnLeft()
        {           
            if (playerAnimation == null)
            {
                return;
            }

            rb.AddTorque(RotationSpeed, ForceMode2D.Force);
            playerAnimation.TurnLeftAnimation();
        }

        #endregion

    }

}