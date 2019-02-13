using UnityEngine;


namespace MinesEvader
{

    ///<summary>
    /// A component which adds simple animation functionality to an existing Player script.
    ///</summary>
    [RequireComponent(typeof(Player))]
    public class PlayerAnimation : MonoBehaviour
    {
        
        public Animator AnimatorReference;

        // To be able to manipulate the animation parameters more efficiently,
        // we are storing strings as hash ids.
        int turningLeftHash = Animator.StringToHash("TurningLeft"); 
        int turningRightHash = Animator.StringToHash("TurningRight");
        

        void Awake ()
        {

            if (AnimatorReference == null)
            {
                AnimatorReference = GetComponent<Animator>();
            }

            if (AnimatorReference == null)
            {
                Debug.Log("You are using a player animation reference yet you have nothing" +
                    " referenced or no animator controllers on your object.");
            }                      
        }
        
        #region Animation Methods

        // If the hash ids do not exist, i.e. you do not have TurningLeft or TurningRight 
        // bools or the wrong animator is being used; you will keep getting Hash ######### 
        // does not exist warnings.
        public void StopTurningAnimation()
        {
            if (AnimatorReference != null)
            {
                AnimatorReference.SetBool(turningRightHash, false);
                AnimatorReference.SetBool(turningLeftHash, false);
            }         
        }

        public void TurnLeftAnimation()
        {
            if (AnimatorReference != null)
            {
                AnimatorReference.SetBool(turningRightHash, false);
                AnimatorReference.SetBool(turningLeftHash, true);
            }
        }

        public void TurnRightAnimation()
        {
            if (AnimatorReference != null)
            {
                AnimatorReference.SetBool(turningRightHash, true);
                AnimatorReference.SetBool(turningLeftHash, false);
            }
        }

        #endregion

    }

}