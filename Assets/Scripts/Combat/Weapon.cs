using RPG.Attributes;
using RPG.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [FormerlySerializedAs("_weaponOverride")] [SerializeField] AnimatorOverrideController _animatorOverride = null;
        [SerializeField] GameObject _equippedPrefab = null;
        [SerializeField] float _weaponRange = 2f;
        [SerializeField] int _weaponDamage = 3;
        [SerializeField] int _percentageBonus = 0;
        [SerializeField] bool _isRightHanded = true;
        [SerializeField] Projectile _projectile = null;

        const string _weaponName = "Weapon";
        
        public float GetRange()
        {
            return _weaponRange;
        }

        public int GetDamage()
        {
            return _weaponDamage;
        }

        public float GetPercentageBonus()
        {
            return _percentageBonus;
        }
        
        public bool HasProjectile()
        {
            return _projectile != null; 
        }

        public void LaunchProjectile(Transform _rightHand, Transform _leftHand, Health target, GameObject instigator, float calculatedDamage)
        {
            Projectile projectileInstance = Instantiate(_projectile, GetTransform(_leftHand, _rightHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, calculatedDamage);
        }

        public void Spawn(Transform _rightHand, Transform _leftHand, Animator _animator)
        {
            DestroyOldWeapon(_rightHand, _leftHand);
            
            if (_equippedPrefab != null)
            {
                Transform handTransform = GetTransform(_rightHand, _leftHand);
                GameObject weapon = Instantiate(_equippedPrefab, handTransform);
                weapon.name = _weaponName;
            }
            
            var overrideController = _animator.runtimeAnimatorController as AnimatorOverrideController;    
            if (_animatorOverride != null)
                _animator.runtimeAnimatorController = _animatorOverride;
            else if (overrideController != null)
            {
                    _animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
        }
        void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(_weaponName);
            if (oldWeapon == null)
            {
                oldWeapon = leftHand.Find(_weaponName);
            }
            if (oldWeapon == null) return;

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
            
        }
        Transform GetTransform(Transform _rightHand, Transform _leftHand)
        {
            Transform handTransform;
            if (_isRightHanded) handTransform = _rightHand;
            else handTransform = _leftHand;
            return handTransform;
        }


    }
}
