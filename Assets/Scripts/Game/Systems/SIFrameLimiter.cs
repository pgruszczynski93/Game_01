using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIFrameLimiter : MonoBehaviour
    {
        [SerializeField] int _targetFrames;

        void Awake()
        {
            SetTargetFramerate();
        }

        void SetTargetFramerate() => Application.targetFrameRate = _targetFrames;
    }
}