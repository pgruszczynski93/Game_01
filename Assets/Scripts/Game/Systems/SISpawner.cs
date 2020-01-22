using System;
using UnityEngine;

namespace SpaceInvaders
{
    public class SISpawner : MonoBehaviour
    {
        bool _initialised;

        protected virtual void Initialise()
        {
            if (_initialised)
                return;

            _initialised = true;
        }

        protected virtual void LoadSetup() { }

        protected void OnEnable()
        {
            AssignEvents();
        }

        protected void OnDisable()
        {
            RemoveEvents();
        }

        protected virtual void AssignEvents() { }

        protected virtual void RemoveEvents() { }

        protected virtual void TryToSpawn()
        {
            LoadSetup();
            Initialise();
        }

        protected virtual void TryToRespawn() { }
    }
}