using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        BaseStats _baseStats;

        void Awake()
        {
            _baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        void Update()
        {
            GetComponent<TMP_Text>().text = String.Format("{0:0}", _baseStats.GetLevel());
        }
    }
}
