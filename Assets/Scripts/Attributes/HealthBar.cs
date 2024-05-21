using RPG.UI.DamageText;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] RectTransform _foreground;
        [SerializeField] Health _health;

        bool _healthBarEnabled = false;

        void Awake()
        {
            _health.OnDeath += DeactivateHealthBar;
            transform.GetChild(0).gameObject.SetActive(false);
        }
        public void UpdateHealthBar(GameObject character)
        {
            if (!_healthBarEnabled)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                _healthBarEnabled = true;
            }
            _foreground.localScale = new Vector3(character.GetComponent<Health>().GetFraction(), 1, 1);
        }
        void DeactivateHealthBar(object sender, EventArgs e)
        {
            transform.GetChild(0).gameObject.SetActive(false); 
            _healthBarEnabled = false;
        }
    }
}
