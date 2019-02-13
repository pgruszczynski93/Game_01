using UnityEngine;

namespace MinesEvader
{
    
    /// <summary>
    /// The camera simply follows the player and does not go out of bounds.
    /// </summary>
    [RequireComponent(typeof(Camera))]   
    public class MinesEvaderCamera : MonoBehaviour
    {

        Vector2 cameraSize;
        Rect cameraField;
        Camera cam;

        void Start()
        {

            #if UNITY_ANDROID || UNITY_IOS
			Screen.autorotateToPortrait = true;
			Screen.autorotateToLandscapeLeft = false;
			Screen.autorotateToLandscapeRight = false;
            #endif

            cam = GetComponent<Camera>();
            cam.orthographic = true;
           
            cameraSize = new Vector2(cam.orthographicSize * cam.aspect, cam.orthographicSize);


            // Please note that when you are defining a Rect, you are inputting the position
            // in X & Y then the width and height, that's why cameraSize.X/Y is multiplied
            // here by 2.
            cameraField = new Rect( GameManager.gameField.position.x + cameraSize.x,
                                    GameManager.gameField.position.y + cameraSize.y,
                                    GameManager.gameField.size.x - cameraSize.x * 2,
                                    GameManager.gameField.size.y - cameraSize.y * 2 );
        }

        void Update()
        {
            if (!GameManager.player)
            {
                return;
            }

            transform.position = new Vector3(
                GameManager.player.transform.position.x, 
                GameManager.player.transform.position.y, 
                transform.position.z);

            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, cameraField.xMin, cameraField.xMax),                                            
                Mathf.Clamp(transform.position.y, cameraField.yMin, cameraField.yMax),                                             
                transform.position.z);
        }

    }

}