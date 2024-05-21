using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class DestroyAfterEffects : MonoBehaviour
    {
        [SerializeField] GameObject _targetToDestroy = null;
        
        void Update()
        {
            if (!GetComponent<ParticleSystem>().IsAlive())
            {
                if (_targetToDestroy != null)
                {
                    Destroy(_targetToDestroy);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
