using System.Collections;
using UnityEngine;

namespace MinesEvaderMinimal
{

    /// <summary>
    /// This mine spawner has way less features than the original one, but it's just to show
    /// that the basics of spawning anything is the instantiate function and how you can 
    /// implement it. We are setting the mine to false then to true so that you don't accidently
    /// detontate the original mine we are referencing from and get a missing reference error.
    /// </summary>
    public class SpawnerMini : MonoBehaviour
    {

        public GameObject Mine;
        public float SpawnRate;

        float spawnRate;

        void Start()
        {
            spawnRate = 1 / SpawnRate;
            Mine.SetActive(false);
            StartCoroutine(SpawnMines());
        }

        IEnumerator SpawnMines()
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnRate);
                Vector3 MinePosition = new Vector3 (
                    Random.Range(GameManagerMini.gameField.xMin, GameManagerMini.gameField.xMax), 
                    Random.Range(GameManagerMini.gameField.yMin, GameManagerMini.gameField.yMax), 
                    0.0f);


                GameObject SpawnedMine = Instantiate(Mine, MinePosition, Quaternion.identity);
                SpawnedMine.SetActive(true);                      
            }
        }

    }

}

