using UnityEngine;

namespace MinesEvaderMinimal
{

    /// <summary>
    /// This script is not very different from the complete MinesEvaderCamera
    /// script except that no considerations for mobile have been made It gets
    /// the player transform and game field and uses the orthogaphic camera
    /// size to substract or add to the game field so that the camera does
    /// not see anything outside of it.
    /// </summary>
    public class MinesEvaderCameraMini : MonoBehaviour
    {

        Rect cameraField;
        Vector2 cameraSize;

        void Start()
        {
            cameraSize = new Vector2(
                Camera.main.orthographicSize * Camera.main.aspect, 
                Camera.main.orthographicSize);

            cameraField = new Rect(
                GameManagerMini.gameField.position.x + cameraSize.x,
                GameManagerMini.gameField.position.y + cameraSize.y,
                GameManagerMini.gameField.size.x - cameraSize.x * 2,
                GameManagerMini.gameField.size.y - cameraSize.y * 2);
        }

        void Update()
        {
            if (GameManagerMini.PlayerGO == null) return;

            transform.position = new Vector3(
                GameManagerMini.PlayerGO.transform.position.x, 
                GameManagerMini.PlayerGO.transform.position.y, 
                transform.position.z);

            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, cameraField.xMin, cameraField.xMax),
                Mathf.Clamp(transform.position.y, cameraField.yMin, cameraField.yMax),
                transform.position.z);
        }

    }

}
