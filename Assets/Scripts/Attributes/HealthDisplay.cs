using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        Health _health;

        void Awake()
        {
            _health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        void Update()
        {
            GetComponent<TMP_Text>().text = String.Format("{0:0}/{1:0}",_health.GetHealthPoints(), _health.GetMaxHealthPoints());
        }
    }
}
