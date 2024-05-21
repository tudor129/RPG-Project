using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjectsSpawner : MonoBehaviour
    {
        [SerializeField] GameObject _persistentObjectPrefab;

        static bool _hasSpawned = false;

        void Awake()
        {
            if (_hasSpawned) return;

            SpawnPersistentObjects();
            
            _hasSpawned = true;
        }
        void SpawnPersistentObjects()
        {
            GameObject _persistentObject = Instantiate(_persistentObjectPrefab);
            DontDestroyOnLoad(_persistentObject); 
        }
    }
}
