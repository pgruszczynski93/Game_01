using UnityEngine;

namespace MinesEvaderMinimal
{

    /// <summary>
    /// Quite similar to the original player script in terms of the basics, 
    /// no UI health acces needed and no mobile controls added, and taking
    /// health damage only reduces by 1 and not a vlaue.
    /// </summary>
    public class PlayerMini : MonoBehaviour
    {
        public float Health;
        public float Speed;
        public float RotationSpeed;

        Rigidbody2D rb;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            float verticalInput = Input.GetAxis("Vertical") * Speed;
            float horizontalInput = Input.GetAxis("Horizontal") * -RotationSpeed;

            rb.AddForce(verticalInput * transform.up, ForceMode2D.Force);
            rb.AddTorque(horizontalInput, ForceMode2D.Force);

            if (rb.velocity.magnitude > Speed)
            {
                rb.velocity = rb.velocity.normalized * Speed;
            }

            if (rb.angularVelocity > RotationSpeed)
            {
                rb.angularVelocity = RotationSpeed;
            }
            if (rb.angularVelocity < -RotationSpeed)
            {
                rb.angularVelocity = -RotationSpeed;
            } 

            float clampedX = Mathf.Clamp(transform.position.x, 
                GameManagerMini.gameField.xMin, 
                GameManagerMini.gameField.xMax);

            float clampedY = Mathf.Clamp(transform.position.y, 
                GameManagerMini.gameField.yMin, 
                GameManagerMini.gameField.yMax);
                        
            transform.position = new Vector3 (clampedX, clampedY, transform.position.z);
        }

        public void TakeDamage()
        {
            Health--;
            if (Health <= 0)
            {
                Destroy(gameObject);
                GameManagerMini.RoundEnd();
            }
        }
    }
}

