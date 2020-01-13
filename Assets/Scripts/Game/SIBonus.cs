using UnityEngine;

namespace SpaceInvaders
{
    [RequireComponent(typeof(SIBonusColliderBehaviour))]
    public class SIBonus : MonoBehaviour, ICanMove
    {
        [SerializeField] protected BonusSetup _bonusSetup;
        [SerializeField] protected Rigidbody _rigidbody;
        protected BonusSettings _bonusSettings;

        bool _initialised;
        Transform _thisTransform;
        Vector3 _startPosition;

        protected virtual void Initialise()
        {
            if (_initialised)
                return;
            _initialised = true;
            _thisTransform = transform;
            _startPosition = _thisTransform.position;
            _bonusSettings = _bonusSetup.bonusSettings;
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

        protected virtual void AssignEvents()
        {
            SIEventsHandler.OnUpdate += TryToResetObject;
        }

        protected virtual void RemoveEvents()
        {
            SIEventsHandler.OnUpdate -= TryToResetObject;
        }


        protected void TryToResetObject()
        {
            Vector3 bonusViewPortPosition =
                SIGameMasterBehaviour.Instance.MainCamera.WorldToViewportPoint(_thisTransform.position);

            if (bonusViewPortPosition.IsInVerticalViewportSpace()) StopObject();
        }

        public void MoveObject()
        {
            _rigidbody.AddForce(SIHelpers.VectorDown * _bonusSettings.releaseForceMultiplier, ForceMode.Impulse);
        }

        public void StopObject()
        {
            _rigidbody.velocity = SIHelpers.VectorZero;
            _thisTransform.position = _startPosition;
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