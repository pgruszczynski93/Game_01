using UnityEngine;

namespace SpaceInvaders
{
    [RequireComponent(typeof(SIBonusColliderBehaviour))]
    public class SIBonus : MonoBehaviour, ICanMove
    {
        [SerializeField] float _forceMultiplier;
        [SerializeField] Rigidbody _rigidbody;

        Transform _cachedTransform;
        Vector3 _startPosition;

        [field: SerializeField] public SIBonusInfo BonusInfo { get; set; }

        void Awake()
        {
            SetInitialReferences();
        }

        void OnEnable()
        {
            SIEventsHandler.OnUpdate += TryToResetObject;
        }

        void OnDisable()
        {
            SIEventsHandler.OnUpdate -= TryToResetObject;

            BonusInfo.OnBonusFinishEvent.RemoveListener(() =>
            {
                //SIGameMasterBehaviour.Instance.Player.ShieldVfxBehaviour.TryToEnableVFX(false);
            });
        }

        void SetInitialReferences()
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

        void TryToAddShieldEventListener()
        {
            if (BonusInfo.bonusType != BonusType.Shield) return;

            BonusInfo.OnBonusFinishEvent.AddListener(() =>
            {
//                SIGameMasterBehaviour.Instance.Player.ShieldVfxBehaviour.TryToEnableAndDetachVFX(false);
            });
        }

        void TryToResetObject()
        {
            Vector3 bonusViewPortPosition =
                SIGameMasterBehaviour.Instance.MainCamera.WorldToViewportPoint(_cachedTransform.position);

            if (bonusViewPortPosition.IsInVerticalViewportSpace()) StopObject();
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