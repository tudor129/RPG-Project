using RPG.Attributes;
using System;
using TMPro;
using UnityEngine;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter _playerFighter;

        void Awake()
        {
            _playerFighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        void Update()
        {
            
            if (_playerFighter.GetTarget() == null)
            {
                GetComponent<TMP_Text>().text = "N/A";
                return;
            }
            Health health = _playerFighter.GetTarget();
            
            if (health != null)
                GetComponent<TMP_Text>().text = String.Format("{0:0}/{1:0}",health.GetHealthPoints(), health.GetMaxHealthPoints());

        }
    }
}
