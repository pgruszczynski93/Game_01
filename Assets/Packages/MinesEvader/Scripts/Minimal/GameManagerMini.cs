using UnityEngine;

namespace MinesEvaderMinimal
{

    /// <summary>
    /// The GameManager in the minimal version is really simpler than the original
    /// script in the complete version No UI considerations have been made and no audio
    /// as well which have simplified it a lot Now this script only passes through the
    /// damage player function ad displays the game over while caching the game field
    /// and player.
    /// </summary>
    public class GameManagerMini : MonoBehaviour
    {
        public GameObject GameOver;
        public static GameObject gameOver;

        public static Rect gameField;
        public Rect GameField;

        public static PlayerMini PlayerScript;
        public static GameObject PlayerGO;


        void Start()
        {
            gameField = GameField;
            gameOver = GameOver;

            PlayerScript = FindObjectOfType<PlayerMini>();
            PlayerGO = PlayerScript.gameObject;
        }

        public static void DamagePlayer()
        {
            PlayerScript.TakeDamage();
        }

        public static void RoundEnd()
        {
            gameOver.SetActive(true);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(GameField.center, 
                new Vector3(GameField.width, GameField.height, 0.0f));
        }
    }
}
