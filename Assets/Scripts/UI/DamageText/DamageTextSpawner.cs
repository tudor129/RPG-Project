using RPG.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText _damageTextPrefab;
        [SerializeField] float _damageTextHeight = 2.5f;

        Quaternion _cameraRot;

        void Awake()
        {
            _cameraRot = Camera.main.transform.rotation;
        }


        public void Spawn(float damageAmount)
        {
            
            DamageText instance = Instantiate(_damageTextPrefab, (transform.position + Vector3.up * _damageTextHeight), _cameraRot);
            //instance.GetComponentInChildren<TextMeshProUGUI>().text = damageAmount.ToString();
            instance.SetValue(damageAmount);
        }
    }
}
