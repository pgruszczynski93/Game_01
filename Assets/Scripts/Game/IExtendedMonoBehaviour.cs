using UnityEngine;

namespace SpaceInvaders
{
    public interface IExtendedMonoBehaviour
    {
        void Initialise();
        void AssignEvents();
        void RemoveEvents();
    }
}