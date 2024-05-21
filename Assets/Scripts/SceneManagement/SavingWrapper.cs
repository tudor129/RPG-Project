using System;
using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string _defaultSaveFile = "save";
        [SerializeField] private float _fadeInTime = 0.2f;
        void Awake()
        {
            StartCoroutine(LoadLastScene());
        }

        IEnumerator LoadLastScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return GetComponent<SavingSystem>().LoadLastScene(_defaultSaveFile);
            yield return fader.FadeIn(_fadeInTime);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }

            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
            }
        }
        public void Save()
        {
            GetComponent<SavingSystem>().Save(_defaultSaveFile);
        }
        public void Load()
        {
            //call saving system
            GetComponent<SavingSystem>().Load(_defaultSaveFile);
        } 
        void Delete()
        {
            GetComponent<SavingSystem>().Delete(_defaultSaveFile);
        }
    }
}
