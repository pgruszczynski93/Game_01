using System;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIEventsHandler
    {
        public static Action OnObjectMovement = delegate { };
        public static Action OnPlayerShoot = delegate { };
    }
}