using UnityEngine;

namespace SpaceInvaders
{
    public class SIGameMasterBehaviour : SIGenericSingleton<SIGameMasterBehaviour> {
        [SerializeField] Camera _mainCamera;
        [SerializeField] SIPlayerBehaviour _player;
        [SerializeField] SIScreenAreaCalculator _screeenAreaCalculator;

        public Camera MainCamera
        {
            get
            {
                if (_mainCamera != null)
                    return _mainCamera;

                Debug.LogError("No camera assigned!");
                return null;

            }
        }

        public SIPlayerBehaviour Player
        {
            get
            {
                if (_player != null)
                    return _player;

                Debug.LogError("No player assigned to SIGameMasterBehaviour");
                return null;
            }
        }

        public SIScreenAreaCalculator ScreenAreaCalculator => _screeenAreaCalculator;
    }
}