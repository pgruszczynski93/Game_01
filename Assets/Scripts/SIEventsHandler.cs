﻿using System;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIEventsHandler
    {
        public static Action OnPlayerMove = delegate { };
        public static Action OnPlayerShoot = delegate { };
    }
}