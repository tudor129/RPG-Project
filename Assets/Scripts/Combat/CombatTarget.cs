using RPG.Attributes;
using RPG.Control;
using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        PlayerController _player;
        Fighter _fighter;

        void Awake()
        {
            _player = FindObjectOfType<PlayerController>();
            _fighter = GetComponent<Fighter>();

        }

        public CursorType GetCursorType()
        {
             if (_player.GetPlayerCurrentWeapon().HasProjectile())
             {
                 return CursorType.CombatRanged;
             }
             return CursorType.CombatMelee;
        }

        
        public bool HandleRaycast(PlayerController callingController)
        {
            if (!callingController.GetComponent<Fighter>().CanAttack(gameObject))
            {
                return false;
            }

            if (Input.GetMouseButton(0))
            {
                callingController.GetComponent<Fighter>().Attack(gameObject);
            }
            return true;
        }
    }
}
