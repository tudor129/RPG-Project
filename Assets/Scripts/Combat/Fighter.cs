using GameDevTV.Utils;
using RPG.Attributes;
using RPG.Control;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using RPG.Stats;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.WSA;
using Cursor = UnityEngine.Cursor;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField] float _timeBetweenAttacks = 1f;
        
        [SerializeField] Transform _rightHandTransform = null;
        [SerializeField] Transform _leftHandTransform = null;
        [SerializeField] Weapon _defaultWeapon = null;
        
        Health _target;
        Animator _attack;
        Animator _stopAttack;
        ActionScheduler _startAction;
        Health _isDead;
        LazyValue<Weapon> _currentWeapon;
        WeaponBehaviour _currentWeaponBehaviour;
        
        float _distance;
        float _timeSinceLastAttack = Mathf.Infinity;
        GameObject _instigator;
        WeaponPickup _weaponPickup;


        void Awake()
        {
            _attack = GetComponent<Animator>();
            _startAction = GetComponent<ActionScheduler>();
            _stopAttack = GetComponent<Animator>();
            _isDead = GetComponent<Health>();
            _weaponPickup = FindObjectOfType<WeaponPickup>();
            _currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }
        Weapon SetupDefaultWeapon() 
        {
            AttachWeapon(_defaultWeapon);
            return _defaultWeapon;
        }

        void Start()
        {
            _currentWeapon.ForceInit();
        }
        

        void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;

            if (_target == null)
            {
                return;
            }
            if (_target.IsDead())
            {
                Cancel();
                return;
            }
            
            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(_target.transform.position, 1f);
                //GetComponent<NavMeshAgent>().speed = 4f;
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }
        
        public void EquipWeapon(Weapon weapon)
        {
            _currentWeapon.value = weapon;
            AttachWeapon(weapon);
            
        }
        void AttachWeapon(Weapon weapon)
        {
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(_rightHandTransform, _leftHandTransform, animator);
        }

        public Health GetTarget()
        {
            return _target;
        }
        
        void AttackBehaviour()
        {
            transform.LookAt(_target.transform);
            
            if (_timeSinceLastAttack > _timeBetweenAttacks)
            {
                //This will trigger the hit event
                TriggerAttack();
                _timeSinceLastAttack = 0f;
            }
        }
        void TriggerAttack()
        {
            _attack.ResetTrigger("stopAttack");
            _attack.SetTrigger("attack");
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null)
                return false;
            
            Health targetToTest = _isDead;
            return targetToTest != null && !targetToTest.IsDead();
        }
        
        public void Attack(GameObject combatTarget)
        {
            //GetComponent<ActionScheduler>().StartAction(this);
            _startAction.StartAction(this);
            _target = combatTarget.GetComponent<Health>();
           
        }
        
        // Animation event  -- it is called when the specific moment in the animation happens
        void Hit()
        {
            if (_target == null)
                return;
            
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            
            if (_currentWeapon.value.HasProjectile())
            {
                _currentWeapon.value.LaunchProjectile(_leftHandTransform, _rightHandTransform, _target, gameObject, damage);
                
                //_target.TakeDamage(_currentWeapon.GetDamage());

            }
            else
            {
                
                _target.TakeDamage(gameObject, damage);
                //_target.TakeDamage(_instigator, damage); 
            }
            
        }
        void Shoot()
        {
            Hit();
        }
   
         
        bool GetIsInRange()  
        {
            return Vector3.Distance(transform.position, _target.transform.position) < _currentWeapon.value.GetRange();
        }
        bool GetIsInRangeOfPickup()
        {
            
            return Vector3.Distance(transform.position, _weaponPickup.transform.position) < 1f;
        }

        public void Cancel()
        {
            StopAttack();
            _target = null;
            GetComponent<Mover>().Cancel();
        }
        void StopAttack()
        {
            _stopAttack.ResetTrigger("attack");
            _stopAttack.SetTrigger("stopAttack");
        }

        public Weapon GetCurrentWeapon()
        {
            
            return _currentWeapon.value;
        }
        
        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return _currentWeapon.value.GetDamage();
            }
        }
        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return _currentWeapon.value.GetPercentageBonus();
            }
        }
        public object CaptureState()
        {
             return _currentWeapon.value.name;
        }
        public void RestoreState(object state)
        {
            string _weaponName = (string)state;
            Weapon _weapon = Resources.Load<Weapon>(_weaponName);
            EquipWeapon(_weapon);
        }
        
    }
}
