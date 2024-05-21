using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        Experience _playerExperience;


        void Awake()
        {
            _playerExperience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        void Update()
        {
            GetComponent<TMP_Text>().text = String.Format("{0:0}", _playerExperience.GetPoints());
        } 
    }
}
