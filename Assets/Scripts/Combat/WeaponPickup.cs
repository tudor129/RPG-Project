using RPG.Combat;
using RPG.Control;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] Weapon _weapon;
        [SerializeField] float _respawnTime = 5f;

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                Pickup(other.GetComponent<Fighter>());
            }
        }
        void Pickup(Fighter fighter)
        {
            fighter.EquipWeapon(_weapon);
            StartCoroutine(HideForSeconds(_respawnTime));
        }

        IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }
        void ShowPickup(bool shouldShow)
        {
            GetComponent<Collider>().enabled = false;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(shouldShow);
            }
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButton(0))
            {
                Pickup(callingController.GetComponent<Fighter>());
            }
            return true;
        }
    }
}
