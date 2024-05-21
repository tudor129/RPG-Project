using RPG.Attributes;
using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {

        [SerializeField] float _projectileSpeed = 10f;
        [SerializeField] bool _isHoming = true;
        [SerializeField] GameObject _hitEffect = null;
        [SerializeField] float _maxLifeTime = 10f;
        [SerializeField] GameObject[] _destroyOnHit = null;
        [SerializeField] float _lifeAfterImpact = 2f;

        Health _target = null;
        GameObject _instigator = null;
        float _damage;


        void Start()
        {
            transform.LookAt(GetAimLocation());
        }

        void Update()
        {
            if (_target == null) return;

            if (_isHoming && !_target.IsDead())
                transform.LookAt(GetAimLocation());

            transform.Translate(Vector3.forward * _projectileSpeed * Time.deltaTime);

        }

        public void SetTarget(Health target, GameObject instigator ,float damage)
        {
            this._target = target;
            this._damage = damage;
            this._instigator = instigator;

            Destroy(gameObject, _maxLifeTime);
        }

        Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = _target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return _target.transform.position;
            }
            return _target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != _target) return;
            if (_target.IsDead()) return;

            _target.TakeDamage(_instigator, _damage);

            if (_hitEffect != null)
                Instantiate(_hitEffect, GetAimLocation(), transform.rotation);

            foreach (GameObject toDestroy in _destroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, _lifeAfterImpact);
        }
    }
}
