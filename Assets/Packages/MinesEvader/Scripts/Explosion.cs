using UnityEngine;

namespace MinesEvader
{

    /// <summary>
    /// Makes sure that the animation clip is played once a destroy event is activated
    /// and that the animation does not loop infinitely. Disables the clip on start,
    /// then starts it again when the explode function is called then destroys the clip.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    public class Explosion : MonoBehaviour
    {

        public AudioClip ExplodeClip;

        SpriteRenderer spriteRenderer;
        Animator animator;
        AnimatorClipInfo[] clips;

        // Duration of clip will be stored in it.
        float timeToDestroy;


        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();

            if (spriteRenderer == null)
            {
                return;
            }

            if (animator == null)
            {
                return;
            }
            
            spriteRenderer.enabled = false;
            animator.enabled = false;
        }

        public void Explode()
        {
            if (animator == null)
            {
                return;
            }

            if (spriteRenderer == null)
            {
                return;
            }

            GameManager.audioSource.PlayOneShot(ExplodeClip);

            transform.parent = null;         
            transform.rotation = Quaternion.identity;

            spriteRenderer.enabled = true;
            animator.enabled = true;

            //Gets the current animation clip info from the Animator to find its length afterwards
            clips = animator.GetCurrentAnimatorClipInfo(0); 

            if (clips.Length > 0)
            {
                timeToDestroy = clips[0].clip.length;
            }
                 
            Destroy(gameObject, timeToDestroy);
        }

    }
}
