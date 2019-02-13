using UnityEngine;

namespace MinesEvader
{

    /// <summary>
    /// This mine is very similar to the standard mine, with the difference that it uses 
    /// impulse force instead of rigidbody velocity calculations
    /// </summary>
    public class ChainMine : NotComponent.MineBase
    {
        
        [Tooltip("Prevents mine from using impulse force again until speed is less than threshold.")]
        public float SpeedThreshold;

        [Tooltip("Will move the mine towards the player.")]      
        public float ImpulseForce;

        [Tooltip("Applied if mine collides with anything other than the player.")]
        public float ChainForce;
        public bool EnableMineRotation;


        protected override void Start()
        {
            base.Start();

            if (!EnableMineRotation)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            } 
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (Distance > DetectionRadius)
            {
                rb.velocity = Direction * StartSpeed;
            }
            else if (rb.velocity.magnitude < SpeedThreshold)
            {
                ImpulseToTarget(ImpulseForce);
            }


            // Unlike the track mine which follows the player like a magnet, the chain mine
            // uses impulse force which might push it out of the game bounds, this is why
            // we are clamping its position inside the game field.

            float clampX = Mathf.Clamp(rb.position.x,
                GameManager.gameField.xMin, GameManager.gameField.xMax);

            float clampY = Mathf.Clamp(rb.position.y,
                GameManager.gameField.yMin, GameManager.gameField.yMax);

            rb.position = new Vector3(clampX, clampY);
        }

        public void ImpulseToTarget(float force)
        {
            rb.AddForce(Direction * force, ForceMode2D.Impulse);       
            if (EnableMineRotation) rb.AddTorque(force);
        }

        protected override void OnCollisionEnter2D(Collision2D col)
        {
            if (((col.gameObject.GetComponent<Player>()) != null) && (AllowCollisionDamage))
            {
                DamagePlayer.Invoke();
            }

            // If the mine has hit an object which is not the player, then it will head towards
            // the player with the chain force, just to add some variation to the game.
            else ImpulseToTarget(ChainForce); 
        }

    }

}