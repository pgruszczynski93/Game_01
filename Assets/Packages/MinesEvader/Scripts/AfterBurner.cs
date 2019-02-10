using UnityEngine;

namespace MinesEvader
{

    /// <summary>
    /// Any script attached to a state within a state machine behaviour (animation controller)
    /// needs to inherit from StateMachineBehaviour This script is for controlling a special
    /// thruster particle system, it basically turns the particles emission off if the player
    /// entered that state (changed direction) and then back on
    /// </summary>
    public class AfterBurner : StateMachineBehaviour
    {

        [Tooltip("You need to type here the exact name as the thruster you are using. Because " +
            "this isn't a mono behaviour script; we can't reference game objects directly.")]
        public string ThrusterName;

        ParticleSystem ps;
        ParticleSystem.EmissionModule pse;


        void Awake()
        {
            GameObject thruster = GameObject.Find(ThrusterName);

            if (thruster == null)
            {
                Debug.Log("Your AfterBurner animation script requires that you reference the thruster you are using.");
                return;
            }

            ps = thruster.GetComponent<ParticleSystem>();

            if (ps == null)
            {
                return;
            }

            pse = ps.emission;
        }

        // The following OnStateEnder/Exit turn the thurster off/on if it entered an animation state.
        // If you are going left for exmample, only the right engine needs to be on or this is
        // how imagined our spaceship.

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (ps == null)
            {
                return;
            }

            pse.enabled = false;
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (ps == null)
            {
                return;
            }

            pse.enabled = true;
        }

    }

}