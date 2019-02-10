using System.Collections;
using UnityEngine;

namespace MinesEvader
{

    /// <summary>
    /// Overlays a color over an object's material intermittently when the StartFlash
    /// function is called to indicate the start of a mine detonation process.
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public class FlashEffect : MonoBehaviour
    {
        public float TotalFlashTime = 2.0f;
        public float TimeBetweenFlashes = 0.5f;
        public Color FlashColor = Color.red;

        Renderer myMaterial;

        public void StartFlash()
        {
            StartCoroutine(Flash());
        }

        IEnumerator Flash()
        {
            // We could cache th material in start, but because this is a coroutine it
            // might happen that this function is called before start.
            myMaterial = gameObject.GetComponent<Renderer>();

            float FlashEndTime = Time.time + TotalFlashTime;
            while (Time.time <= FlashEndTime)
            {
                myMaterial.material.SetColor("_Color", FlashColor);
                yield return new WaitForSeconds(TimeBetweenFlashes);
                myMaterial.material.SetColor("_Color", Color.white);
                yield return new WaitForSeconds(TimeBetweenFlashes);
            }
        }

    }
}