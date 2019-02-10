using UnityEngine;

namespace MinesEvader
{

    ///<summary>
    /// This script uses the particle system to generate stars by emitting one star at a time,
    /// located randomly inside the game field, the different particle modules you see, is
    /// just the way Unity's shuriken particle system is accessed.
    /// </summary>
    [RequireComponent(typeof(ParticleSystem))]
    public class StarsGenerator : MonoBehaviour
    {
        
        public float StarsSize = 2.0f;
        public float StarsLifetime = 7.0f;
        public float StarsSpawnRate = 50f;
        public int MaximumStarsNumber = 150;

        [Space]
        public bool RandomStarsLifetime = true;
        public bool RandomStarsSize = true;

        [Space]
        public Material StarsMaterial;

        float spawnTime;
        float nextRespawn;

        ParticleSystem ps;
        

        void Start()
        {
            if (StarsSpawnRate == 0)
            {
                StarsSpawnRate = 1;
            }

            spawnTime = (1 / StarsSpawnRate);

            ps = GetComponent<ParticleSystem>();

            ParticleSystemRenderer psr = GetComponent<ParticleSystemRenderer>();
            psr.material = StarsMaterial;

            ParticleSystem.MainModule psm = ps.main;
            psm.startLifetimeMultiplier = StarsLifetime;
            psm.startSizeMultiplier = StarsSize;
            psm.maxParticles = MaximumStarsNumber;
            psm.startSpeedMultiplier = 0;

            ParticleSystem.EmissionModule pse = ps.emission;
            pse.enabled = false;

            ParticleSystem.SizeOverLifetimeModule pss = ps.sizeOverLifetime;
            pss.enabled = true;
            pss.size = new ParticleSystem.MinMaxCurve(1f, new AnimationCurve(
                new Keyframe(0, 0),
                new Keyframe(0.5f, 1f),
                new Keyframe(1f, 0)));
        }

        void Update()
        {
            if (Time.time > nextRespawn)
            {
                ParticleSystem.EmitParams star = new ParticleSystem.EmitParams();

                star.position = new Vector3(
                    Random.Range(GameManager.gameField.xMin, GameManager.gameField.xMax),
                    Random.Range(GameManager.gameField.yMin, GameManager.gameField.yMax),
                    0);

                if (RandomStarsSize) star.startSize = StarsSize * Random.value;              
                if (RandomStarsLifetime) star.startLifetime = StarsLifetime * Random.value;

                ps.Emit(star, 1);
                nextRespawn += spawnTime;              
            }
        }

    }

}