using System.Collections;
using UnityEngine;

namespace MinesEvader
{

    /// <summary>
    /// Responsible for spawning enemy mine waves infinitely, it basically spawns a level 
    /// which in turn spawns waves which in turn spawn mines and does this repeatedly.
    /// </summary>
    public class Spawner : MonoBehaviour
    {

        #region Declarations

        // We use serializable to be able to delve into the Wave member class in the inspector
        // and reference the mines we want to use. Without it; Waves won't show in the inspector.
        [System.Serializable] 
        public class Wave
        {
            public GameObject[] Mines;
            public float SpawnRate;
            public float WaveTime;
            public float TimeForNextWave;
        }

        // This will use the serialized class above and list its items.
        public Wave[] Waves;
        public float SpawnRateIncrement;
        public float SpawnDistanceFromPlayer;
        public int MaximumNumberOFMines = 100;

        float currentSpawnRateIncrement;
        bool missingMines;

        #endregion

        void Start()
        {
            if ((Waves.Length == 0) || (Waves == null))
            {
                return;
            }

            // This for loop is to deactivate any mines you have in the scene before playing
            // the game, so that the only ones active are the ones spawned by the spawner.
            // It also notifies you of any missing mines reference and uses that test 
            // later on to prevent spawning missing mines and any exceptions.
            for (int i = 0; i < Waves.Length; i++)
            {
                for (int j = 0; j < Waves[i].Mines.Length; j++)
                {
                    if ((Waves[i].Mines[j]) == null)
                    {
                        missingMines = true;
                        Debug.LogError("You are missing mines reference inside spawner," +
                            " please check the spawner before hitting play again.");
                    }
                    else Waves[i].Mines[j].SetActive(false);
                }
            }

            StartCoroutine(CreateLevels());        
        }

        IEnumerator CreateLevels()
        {
            while (true)
            {
                for (int i = 0; i < Waves.Length; i++)
                {
                    StartCoroutine(CreateWaves(Waves[i], currentSpawnRateIncrement));
                    yield return new WaitForSeconds(Waves[i].WaveTime + Waves[i].TimeForNextWave);
                }

                currentSpawnRateIncrement += SpawnRateIncrement;
            }
        }

        IEnumerator CreateWaves(Wave wave, float rateIncrement)
        {
            float waveEndTime = Time.time + wave.WaveTime;
            float timeBetweenSpawns = 1 / (wave.SpawnRate + rateIncrement);

            while (Time.time < waveEndTime)
            {
                SpawnMines(wave.Mines);
                yield return new WaitForSeconds(timeBetweenSpawns);
            }
            yield return null;
        }

        void SpawnMines(GameObject[] mines)
        {
            // We need to check for the player first because the spawnPosDistance is 
            // dependent on it. We also need to make sure we don't spawn any missing mines.
            if (GameManager.player == null || missingMines)
            {
                return;
            }
            
            if (NotComponent.MineBase.NumberOfMines > MaximumNumberOFMines)
            {
                return;
            }

            Vector3 mineSpawnPosition;
            float distanceFromPlayer;

            do
            {
                mineSpawnPosition = new Vector3(
                Random.Range(GameManager.gameField.xMin, GameManager.gameField.xMax),
                Random.Range(GameManager.gameField.yMin, GameManager.gameField.yMax),
                0.0f);

                distanceFromPlayer = (mineSpawnPosition - GameManager.player.transform.position).magnitude;
            }
            while (distanceFromPlayer < SpawnDistanceFromPlayer);

            GameObject mine = Instantiate(mines[Random.Range(0, mines.Length)], mineSpawnPosition, Quaternion.identity);
            mine.SetActive(true);
        }

    }

}