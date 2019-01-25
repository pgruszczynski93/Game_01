using UnityEngine;

namespace SpaceInvaders
{
    public class SIProjectileBehaviour : MonoBehaviour, IMoveable
    {

        [SerializeField] public bool canMove;

        public void MoveObj()
        {
            Debug.Log("Xxxx");
            transform.Translate(0,2*Time.deltaTime,0);
        }

        private void Update()
        {
            if (canMove == false)
            {
                return;
            }

            MoveObj();;
        }
    }

}

