using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace RPG.SceneManagement
{
    public class  Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A, B, C, D, E
        }
        
        [FormerlySerializedAs("_sceneLoad")] [SerializeField] int _sceneToLoad = -1;
        [SerializeField] Transform _spawnPoint;
        [SerializeField] DestinationIdentifier _destination;
        [FormerlySerializedAs("_fadeTime")] [SerializeField] float _fadeOutTime = 2f;
        [SerializeField] float _fadeInTime = 1f;
        [SerializeField] float _fadeWaitTime = 0.5f;

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")                      
            {
                StartCoroutine(Transition());
            }
        }

        IEnumerator Transition()
        {
            if (_sceneToLoad < 0)
            {
                Debug.LogError("Scene to load not set");
                yield break; 
            }

            DontDestroyOnLoad(gameObject);
            
            Fader fader = FindObjectOfType<Fader>();
            
            yield return fader.FadeOut(_fadeOutTime);

            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            wrapper.Save();
            
            yield return SceneManager.LoadSceneAsync(_sceneToLoad);
            
            wrapper.Load();
            Debug.Log("Game loaded");

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            
            wrapper.Save();

            yield return new WaitForSeconds(_fadeWaitTime);
            yield return fader.FadeIn(_fadeInTime);
            
            Destroy(gameObject);
        }
        void UpdatePlayer(Portal otherPortal)
        {
            GameObject player =  GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.GetComponent<NavMeshAgent>().Warp(otherPortal._spawnPoint.position); 
            player.transform.rotation = otherPortal._spawnPoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;
        }
        Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal._destination != _destination) continue;

                return portal;
            }
            return null;
        }



    }
}
