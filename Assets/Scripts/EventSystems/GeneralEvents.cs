using System;
using PG.Game.Helpers;
using UnityEngine;

namespace PG.Game.EventSystem {
    public class GeneralEvents {
        public static event Action<GameStates> OnGameStateChanged;
        public static event Action OnIndependentUpdate;
        public static event Action OnUpdate;
        public static event Action<Vector3> OnAxesInputReceived;

        public static void BroadcastOnGameStateChanged(GameStates gameState) {
            OnGameStateChanged?.Invoke(gameState);
        }

        public static void BroadcastOnAxesInputReceived(Vector3 inputVector) {
            OnAxesInputReceived?.Invoke(inputVector);
        }

        public static void BroadcastOnIndependentUpdate() {
            OnIndependentUpdate?.Invoke();
        }

        public static void BroadcastOnUpdate() {
            OnUpdate?.Invoke();
        }
    }
}