using RPG.Combat;
using RPG.Control;
using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicsControlRemover : MonoBehaviour
    {
        GameObject _player;
        
        void Awake()
        {
            _player = GameObject.FindWithTag("Player");
        }

        void Start()
        {
            
        }

        void OnEnable()
        {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }
        void OnDisable()
        {
            GetComponent<PlayableDirector>().played -= DisableControl;
            GetComponent<PlayableDirector>().stopped -= EnableControl;
        }

        void DisableControl(PlayableDirector someValue)
        {
            _player.GetComponent<ActionScheduler>().CancelCurrentAction();
            _player.GetComponent<Fighter>().Cancel();
            _player.GetComponent<PlayerController>().enabled = false;
        }
        void EnableControl(PlayableDirector someValue)
        {
            _player.GetComponent<PlayerController>().enabled = true;
            print("EnableControl");
        }
    }
}
