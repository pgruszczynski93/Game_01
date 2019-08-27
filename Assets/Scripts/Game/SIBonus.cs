using UnityEngine;

namespace SpaceInvaders
{
    [RequireComponent(typeof(SIBonusColliderBehaviour))]
    public class SIBonus : MonoBehaviour, IMoveable
    {
        [SerializeField] private float _forceMultiplier;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private SIBonusInfo _bonusInfo;

        private Transform _cachedTransform;
        private Vector3 _startPosition;

        public SIBonusInfo BonusInfo
        {
            get => _bonusInfo;
            set => _bonusInfo = value;
        }

        private void Awake()
        {
            SetInitialReferences();
        }

        private void OnEnable()
        {
            SIEventsHandler.OnUpdate += TryToResetObject;
        }

        private void OnDisable()
        {
            SIEventsHandler.OnUpdate -= TryToResetObject;

            _bonusInfo.OnBonusFinishEvent.RemoveListener(() =>
            {
                SIGameMasterBehaviour.Instance.Player.ShieldVfxBehaviour.OnEnableVFXCallback(false);
            });
        }

        private void SetInitialReferences()
        {
            if (SIGameMasterBehaviour.Instance.Player == null || _rigidbody == null)
            {
                SIHelpers.SISimpleLogger(this, "SetInitialReferences - no player assigned.", SimpleLoggerTypes.Error);
                return;
            }

            _cachedTransform = transform;
            _startPosition = _cachedTransform.position;

            TryToAddShieldEventListener();
        }

        private void TryToAddShieldEventListener()
        {
            if (_bonusInfo.bonusType != BonusType.Shield)
            {
                return;
            }

            _bonusInfo.OnBonusFinishEvent.AddListener(() =>
            {
                SIGameMasterBehaviour.Instance.Player.ShieldVfxBehaviour.OnEnableVFXCallback(false);
            });
        }

        private void TryToResetObject()
        {
            Vector3 bonusViewPortPosition =
                SIGameMasterBehaviour.Instance.MainCamera.WorldToViewportPoint(_cachedTransform.position);

            if (bonusViewPortPosition.IsObjectOutOfVerticalViewportBounds3D())
            {
                StopObject();
            }
        }

        public void MoveObject()
        {
            _rigidbody.AddForce(SIHelpers.VectorDown * _forceMultiplier, ForceMode.Impulse);
        }

        public void StopObject()
        {
            _rigidbody.velocity = SIHelpers.VectorZero;
            _cachedTransform.position = _startPosition;
            gameObject.SetActive(false);
        }
    }

    /* WYCZYSCIC KOD
     *
     * 1. dopisać mechanike dropienia bonusow:
     * a) bonusy przypisane do kazdego enemy - ok
     * b) bonusy dropią z odpowiednimi szansami ( do sprawdzenia) - ok: do testow
     * c) bonusy po przekroczneiu ekranu/ colizji wracaja na swoje miejsce: ok - testy
     * d) napisac w postaci menager
     *
     * 2. poprawic dropienie bonusow
     * 3. poprawic reset bonusow
     * 4. dodac eventy
     */
}
