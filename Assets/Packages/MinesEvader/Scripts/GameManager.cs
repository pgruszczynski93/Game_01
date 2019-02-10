using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace MinesEvader
{

    /// <summary>
    /// Finds reference for the player and UI elements, sets the game field, 
    /// damages the player and performs UI functions.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class GameManager : MonoBehaviour
    {       
        #region UI Data Serialization

        [System.Serializable]
        public class IGUI
        {
            public GameObject[] HealthBar;
            public GameObject HitEffect;
            public Text Score;          
        }

        [System.Serializable]
        public class GOUI
        {
            public GameObject GameOverCanvas;
            public Text FinalScore;
            public Text HighScore;
        }

        [System.Serializable]
        public class UI
        {
            public IGUI InGame;
            [Space]
            public GOUI GameOver;
            [Space]
            public GameObject MobileUI;
        }

        #endregion

        #region Declarations

        public Rect GameField;
        public static Rect gameField;
        [Space]

        public UI UIReference;
        [Space]

        public float HitEffectDuration = 1.0f;

        Player[] playerRefs;       
        public static Player playerScript;
        public static GameObject player;

        public static AudioSource audioSource;
        
        public static int score;                                

        #endregion

        void Awake()
        {
            // We need the game field Rect to be static to easily access that rect from
            // outside of the game manager and improve performance.
            gameField = GameField;

            // Finding all Player class objects, and caching them to the game manager's 
            // static player game object. You should have a single player in the scene,
            // if you have more than one; then the last item checked in the foreach 
            // loop will be cached.
            int playersCount = 0;
            playerRefs = (Player[])Object.FindObjectsOfType(typeof(Player));
            foreach (Player p in playerRefs)
            {
                playersCount++;
                player = p.gameObject;
                playerScript = p;
            }

            if (playersCount > 1)
            {
                Debug.LogWarning("You have more than one active player in the scene, one" +
                    " of them will be picked randomly.");
            }

            if (playersCount < 1)
            {
                Debug.LogError("No player objects found in the scene, please make sure to" +
                    " have one active player in the scene.");
            }

            audioSource = GetComponent<AudioSource>();

        }

        void Start()
        {
            
            #if UNITY_ANDROID || UNITY_IOS

			if ( UIReference.MobileUI == null )
            {
                return;
            }
            
            UIReference.MobileUI.SetActive (true);

            #endif

            if (player == null)
            {
                return;
            }

            if (playerScript.Health != UIReference.InGame.HealthBar.Length)
            {
                Debug.LogWarning("Please use the same number of health bars in the Game " +
                    "Manager and InGame UI as your player health size");
            }
        }

        public void DamagePlayer(int damage)
        {
            if (playerScript == null)
            {
                return;
            }

            StartCoroutine(UIHitEffect());
            playerScript.TakeDamage(damage);
            UpdateHealthBar();                                            
        }

        public void AddScore(int scoreIncrease)
        {
            if (player == null)
            {
                return;
            }

            if (UIReference.InGame.Score == null)
            {
                return;
            }

            score += scoreIncrease;
            UIReference.InGame.Score.text = "Score : " + score.ToString();
        }

        public void UpdateHealthBar()
        {
            if (playerScript.Health != UIReference.InGame.HealthBar.Length)
            {
                return;
            }

            // Updates UI health by deactivating full bars accoring to player's health value.
            for (int i = playerScript.Health; i > playerScript.CurrentHealth; i--)
            {
                UIReference.InGame.HealthBar[i - 1].SetActive(false);
            }            
        }

        IEnumerator UIHitEffect()
        {
            if (playerScript.DamageCoolDownCounter > 0)
            {
                yield break;
            }

            if (UIReference.InGame.HitEffect == null)
            {
                yield break;
            }

            UIReference.InGame.HitEffect.SetActive(true);      
            yield return new WaitForSeconds(HitEffectDuration);
            UIReference.InGame.HitEffect.SetActive(false);
        }

        public void EndRound()
        {
            Invoke("endRound", 1.0f);
        }
        
        void endRound()
        {
            if (UIReference.GameOver.GameOverCanvas == null)
            {
                return;
            }

            UIReference.GameOver.GameOverCanvas.SetActive(true);

            // PlayerPrefs is a special Unity class.
            int highScore = PlayerPrefs.GetInt("HighScore");

            if (highScore < score)
            {
                highScore = score;
                PlayerPrefs.SetInt("HighScore", highScore);
                PlayerPrefs.Save();
            }

            UIReference.GameOver.FinalScore.text = "Final Score : " + score.ToString();            
            UIReference.GameOver.HighScore.text = "High Score : " + highScore.ToString(); 


            #if UNITY_ANDROID || UNITY_IOS

	        if ( UIReference.MobileUI == null )
            {
                return;
            } 	
                
            UIReference.MobileUI.SetActive (false);
            
            #endif
        }

        public void Retry()
        {           
            score = 0;

            // SceneManager is a special Unity class which requires Scene Management directive.
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        public void Quit()
        {
            Application.Quit();
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(GameField.center, 
                new Vector3(GameField.width, GameField.height, 0.0f));
        }

    }

}