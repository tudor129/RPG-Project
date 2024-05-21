using RPG.Attributes;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float _maxSpeed = 6f;
        
        NavMeshAgent _navMeshAgent;
        Vector3 _destination;
        Animator _animator;
        ActionScheduler _startAction;
        Animator _stopAttack;
        Health _health;

        void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _startAction = GetComponent<ActionScheduler>();
            _stopAttack = GetComponent<Animator>();
            _health = GetComponent<Health>();
        }
        
        void Update()
        {
            _navMeshAgent.enabled = !_health.IsDead();
            UpdateAnimator(); 
        }
        void UpdateAnimator() 
        {
            Vector3 velocity = _navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            //GetComponent<Animator>().SetFloat("forwardSpeed", speed);
            _animator.SetFloat("forwardSpeed", speed);
        }

        
        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            if (_health.IsDead())
                return;
           
            _startAction.StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void Cancel()
        {
            _navMeshAgent.isStopped = true;
        }
        public void MoveTo(Vector3 destination, float speedFraction)
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.speed = _maxSpeed * Mathf.Clamp01(speedFraction);
            _navMeshAgent.destination = destination;
            //_stopAttack.SetTrigger("stopAttack");
        }


        public object CaptureState()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["position"] = new SerializableVector3(transform.position);
            data["rotation"] = new SerializableVector3(transform.eulerAngles);
            return data;
        }
        public void RestoreState(object state)
        {
            Dictionary<string, object> data = (Dictionary<string, object>)state;
            _navMeshAgent.enabled = false;
            transform.position = ((SerializableVector3)data["position"]).ToVector();
            transform.eulerAngles = ((SerializableVector3)data["rotation"]).ToVector();
            _navMeshAgent.enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}
