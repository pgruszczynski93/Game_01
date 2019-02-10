using UnityEngine;
using UnityEngine.Events ;

namespace MinesEvader
{

    /// <summary>
    /// Creates an array of radial bullets that spawns from the mine.
    /// </summary>
    [RequireComponent(typeof(ParticleSystem))]
	public class BulletBurst : MonoBehaviour
    {

        #region Declarations

		public UnityEvent BulletHitsPlayer;
        
        [Space]
        public Material BulletsMaterial;

		[Space]
		[Tooltip("Distance that bullets travel in world units.")]
		public float Distance;
		[Tooltip("Speed of bullets (Units/Sec).")]
		public float Speed;
		[Tooltip("Number of bullets in the radial array spawned by the bullet burst.")]
		public int BulletsNumber;
		[Tooltip("Use this value if you want to shift the firing point from the center," +
            " bullets will then fire from a circle instead of a point.")]
		public float EmitDistance;
        [Tooltip("Use this value to change the collision radius scale.")]
        public float ColliderSize;

        [Space]
		[Tooltip("Audio Clip to be played with each shot.")]
		public AudioClip FireClip;        

        [Space]
		[Tooltip("To enable drawing the path that the bullets will take.")]
		public bool ShowBulletsPath;

        ParticleSystem ps; //To store the particle system reference
        
        // Angle in radians between each radial bullet burst.
        float arrayAngleStep;
        float bulletsLifetime;

        #endregion

        void Start()
        {
            //To find the angles between bullet bursts  
            FindAngleStep(BulletsNumber);
            bulletsLifetime = Distance / Speed;

            ps = gameObject.GetComponent<ParticleSystem> ();
            ParticleSystem.MainModule psm = ps.main;
            psm.startLifetime = bulletsLifetime;

            ParticleSystem.EmissionModule pse = ps.emission;
            pse.enabled = false;

            // This will be used to turn collision on and changing the collider size so
            // that you can easily add a new script & not worry about doing it manually.
            ParticleSystem.CollisionModule psc = ps.collision;

            psc.enabled = true;
            psc.radiusScale = ColliderSize;
            psc.sendCollisionMessages = true;
            psc.type = ParticleSystemCollisionType.World;
            psc.mode = ParticleSystemCollisionMode.Collision2D;

            //This ensure that the bullets are killed on collision
            psc.maxKillSpeed = 0;


            ParticleSystemRenderer psr = GetComponent<ParticleSystemRenderer>();
            psr.material = BulletsMaterial;
        }

        /// <summary>
        /// To find the angles between bullet bursts (i.e. 6 bullets need a 60 degree
        /// angle between each to make a 360). 
        /// </summary>
        void FindAngleStep(int bulletsNumber)
        {
            if (bulletsNumber == 0)
            {
                bulletsNumber = 1;
            }
          
            arrayAngleStep = (360 / bulletsNumber) * Mathf.Deg2Rad;
        }

        /// <summary>
        /// Emits all the bullets array particles.
        /// </summary>
        public void Fire ()
        {
            transform.rotation = Quaternion.identity;

            ParticleSystem.EmitParams[] BulletsArray =
                new ParticleSystem.EmitParams[BulletsNumber];

            // Gets the radial bullets position around a unit circle.
            for (int i = 0; i < BulletsNumber; i++)
            {			
				Vector3 BulletsPosition = new Vector3 (
                    Mathf.Cos (i * arrayAngleStep), 
                    Mathf.Sin (i * arrayAngleStep), 
                    0);

                // Will change the unit circle into a circle with a radius of the
                //  Emit Distance and fill the array with each new value.
                BulletsArray[i].position = BulletsPosition * EmitDistance;            
                BulletsArray[i].velocity = BulletsPosition * Speed;
            }        

            for (int i = 0; i < BulletsArray.Length; i++)
            {
                ps.Emit(BulletsArray[i], 1);
            }

            GameManager.audioSource.PlayOneShot(FireClip);
        }

        public void FireAndDestroy ()
        {
            Fire();            
            transform.parent = null;         
            Destroy(gameObject, bulletsLifetime);
        }

        void OnParticleCollision(GameObject other)
        {
            if (other.GetComponent<Player>() == null)
            {
                return;
            }

            BulletHitsPlayer.Invoke();
        }

        void OnDrawGizmosSelected ()
        {
            if (!ShowBulletsPath || BulletsNumber <= 0)
            {
                return;
            }

            Gizmos.color = Color.red;
            // Shows the emit distance radius so you know where the bullets will emit from.
            Gizmos.DrawWireSphere(transform.position, EmitDistance);
            // Shows the final distance the bullets will cover.
            Gizmos.DrawWireSphere(transform.position, (Distance + EmitDistance));

            FindAngleStep(BulletsNumber);

            // loop through the number of bullets to draw line for each one to show the
            // projectile the bullets are headed.
            for (int i = 0; i < BulletsNumber; i++)
            {
                // Gets the radial bullets position around a unit circle.   
                Vector3 bulletsPosition = new Vector3(
                    Mathf.Cos(i * arrayAngleStep), 
                    Mathf.Sin(i * arrayAngleStep), 
                    0);

                // Draws a Gizmo line starting from bullet start location and 
                // ending where the bullet will end its path.
                Gizmos.DrawLine(bulletsPosition * EmitDistance + transform.position, 
                    bulletsPosition * (Distance + EmitDistance) + transform.position);
            }
        }

    }

}