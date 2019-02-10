using UnityEngine;

namespace MinesEvader
{

    /// <summary>
    /// This script controls a mine that heads towards the player and changes speed &
    /// rotation as it gets closer or further away from the Player. Most of this script 
    /// code is actually inherited from Mine Base, very little changes have been made
    /// to the base.
    /// </summary>
    public class TrackMine : NotComponent.MineBase
    {

        public float RotationSpeed;

        [Tooltip("The mine directional speed when its distance to the mine is zero, it also" +
            " helps determine the rotational speed as the mine gets closer to the player.")]
        public float EndSpeed;    

        float speed;
        
        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            speed = Mathf.Lerp(EndSpeed, StartSpeed, 
                (Distance / DetectionRadius));

            rb.velocity = Direction * speed;

            // Angular velocity multiplied by 20 so you can use smaller numbers in the inspector.
            rb.angularVelocity = RotationSpeed * speed * 20.0f; 
        }

    }

}