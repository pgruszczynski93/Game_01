using UnityEngine;

namespace MinesEvader
{
    /// <summary>
    /// A simple script for repeating a linear motion
    /// </summary>
    public class Repeater : MonoBehaviour
    {

        public float RepeatDistance = 50.0f;
        public float Speed = 5.0f;

        Vector3 startPosition;
        Vector3 direction;


        void Start()
        {
            startPosition = transform.position;      
            
            direction = new Vector3 (
                Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad),
                Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad), 
                0);
        }

        void Update()
        {           
            float newPosition = Mathf.Repeat(Time.time * Speed, RepeatDistance);
            transform.position = startPosition + (direction * newPosition);
        }

    }

}
