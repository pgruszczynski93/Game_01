using UnityEngine;

namespace SpaceInvaders
{
    public class SIInputSystem : MonoBehaviour
    {
        [SerializeField] private Joystick _joystick;

        private bool _initialised;

        private float _horizontalAxis;
        private float _verticalAxis;
        private float _depthAxis;
        private Vector3 _inputVector;

        void Initialise()
        {
            if (_joystick == null)
            {
                Debug.LogError("No joystick assigned!");
                return;
            }

            _initialised = true;
            _verticalAxis = 0f;
            _depthAxis = 0f;
        }

        void Start()
        {
            Initialise();
        }

        void OnEnable()
        {
            AssignEvents();
        }

        void OnDisable()
        {
            RemoveEvents();
        }

        void AssignEvents()
        {
            SIEventsHandler.OnUpdate += GetInput;
        }

        void RemoveEvents()
        {
            SIEventsHandler.OnUpdate -= GetInput;
        }

        private void GetInput()
        {
#if UNITY_EDITOR

            _horizontalAxis = Input.GetAxis("Horizontal");

#elif UNITY_ANDROID && !UNITY_EDITOR

            _horizontalAxis = _joystick.Horizontal;
            
#endif

            _inputVector = new Vector3(_horizontalAxis, _verticalAxis, _depthAxis);
            Debug.Log(_inputVector);
            SIEventsHandler.BroadcastOnInputCollected(_inputVector);
        }
    }
}