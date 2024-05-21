using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class CameraFacing : MonoBehaviour
    {
        Transform _cameraTransform;

        void Awake()
        {
            _cameraTransform = Camera.main.transform;
        }

        void LateUpdate()
        {
            transform.LookAt(_cameraTransform);
            transform.rotation = Quaternion.LookRotation(transform.position - _cameraTransform.position);
        }
    }
}
