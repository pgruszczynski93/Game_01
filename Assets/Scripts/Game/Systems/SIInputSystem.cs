using UnityEngine;

namespace SpaceInvaders
{
    public class SIInputSystem : MonoBehaviour
    {
        [SerializeField] Joystick _joystick;

        bool _initialised;

        float _horizontalAxis;
        float _verticalAxis;
        float _depthAxis;
        Vector3 _inputVector;

        void Initialise()
        {
            if (_joystick == null)
            {
                Debug.LogError("No joystick assigned!");
                return;
            }

            _initialised = true;
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
            SIEventsHandler.OnUpdate += TryToGetInput;
        }

        void RemoveEvents()
        {
            SIEventsHandler.OnUpdate -= TryToGetInput;
        }

        void TryToGetInput()
        {
            GetAxes();
            TryToSendShootAction();
        }

        void TryToSendShootAction()
        {
//            if (!SIEnemiesGridManager.Instance.IsEnemyGridMovementAllowed)
//                return;
            
            if(IsShootingKeyPressed())
                SIEventsHandler.BroadcastOnShootInputReceived();
                
        }

        bool IsShootingKeyPressed()
        {
            return Input.GetKeyDown(KeyCode.Space);
        }

        void GetAxes()
        {
#if UNITY_EDITOR

            _horizontalAxis = Input.GetAxis("Horizontal");
            _verticalAxis = Input.GetAxis("Vertical");

#elif UNITY_ANDROID && !UNITY_EDITOR
            _horizontalAxis = _joystick.Horizontal;
            _verticalAxis = _joystick.Vertical;

#endif
            _inputVector = new Vector3(_horizontalAxis, _verticalAxis, _depthAxis);
            SIEventsHandler.BroadcastOnAxesInputReceived(_inputVector);
        }
    }
}