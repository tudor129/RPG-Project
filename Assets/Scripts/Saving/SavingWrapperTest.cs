using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Saving
{
    public class SavingWrapperTest : MonoBehaviour
    {
        SavingSystemTest _onKeyDown;
        const string _defaultSaveFile = "save";
        
        void Awake()
        {
            _onKeyDown = GetComponent<SavingSystemTest>();
        }

        void Start()
        {
            Load();
        }
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.S))
                Save();
            
            if(Input.GetKeyDown(KeyCode.L))
                Load();
        }
        public void Load()
        {
            _onKeyDown.Load(_defaultSaveFile);
        }
        public void Save()
        {
            _onKeyDown.Save(_defaultSaveFile);
        }
    }
}
