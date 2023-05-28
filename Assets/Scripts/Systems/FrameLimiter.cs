using UnityEngine;

namespace PG.Game.Systems {
    public class FrameLimiter : MonoBehaviour {
        [SerializeField] int _targetFrames;

        void Awake() {
            SetTargetFramerate();
        }

        void SetTargetFramerate() => Application.targetFrameRate = _targetFrames;
    }
}