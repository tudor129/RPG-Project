using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {

        //[SerializeField] TextMeshProUGUI _damageText;

        TextMeshProUGUI _text;
        Vector3 _moveVector;

        void Awake()
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
        }

       
        
        public IEnumerator Start()
        {
            yield return new WaitForSeconds(2);
            Destroy(gameObject);
        }



        public void SetValue(float amount)
        {
            _text.text = String.Format("{0:0}", amount);
        }
        
    }
}
