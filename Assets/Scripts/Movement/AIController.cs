using GameDevTV.Utils;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float _chaseDistance = 5f;
        [SerializeField] float _suspicionTime = 5f;
        [SerializeField] float _dwellingTime = 3f;
        [SerializeField] PatrolPath _patrolPath;
        [SerializeField] float _waypointTolerance = 1f;
        [Range(0,1)]
        [SerializeField] float _patrolSpeedFraction = 0.2f;
        
        GameObject _player;
        Fighter _fighter;
        Mover _mover;

        LazyValue<Vector3> _guardPosition;
        Vector3 _currentPosition;

        float _timeSinceLastSawPlayer = Mathf.Infinity;
        float _timeSinceLastWaypoint = Mathf.Infinity;
        int _currentWaypointIndex = 0;
        

        void Awake()
        {
            _fighter = GetComponent<Fighter>();
            _player = GameObject.FindWithTag("Player");
            _mover = GetComponent<Mover>();
            _guardPosition = new LazyValue<Vector3>(InitializePosition);
        }
        Vector3 InitializePosition()
        {
            return transform.position;
        }
        void Start()
        {
            _guardPosition.ForceInit();
        }
        void Update()
        {
            _timeSinceLastSawPlayer += Time.deltaTime;
            _timeSinceLastWaypoint += Time.deltaTime;
            
            if (ShouldChase() && _fighter.CanAttack(_player))
            {
                AttackBehaviour();
                _timeSinceLastSawPlayer = 0;
            }
            else if(_timeSinceLastSawPlayer < _suspicionTime)
            {
               SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }
        }
        void PatrolBehaviour()
        {
            //GetComponent<NavMeshAgent>().speed = 2f;
            
            Vector3 nextPosition = _guardPosition.value;

            if (_patrolPath != null)
            {
                if (AtWayPoint())
                {
                    _timeSinceLastWaypoint = 0f;
                    CycleWayPoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            if (_timeSinceLastWaypoint > _dwellingTime)
            {
                _mover.StartMoveAction(nextPosition, _patrolSpeedFraction); 
            }
        }
        Vector3 GetCurrentWaypoint()
        {
            return _patrolPath.GetWaypoint(_currentWaypointIndex);
        }
        void CycleWayPoint()
        {
            _currentWaypointIndex = _patrolPath.GetNextIndex(_currentWaypointIndex);
        }
        bool AtWayPoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < _waypointTolerance;
        }
        void AttackBehaviour()
        {
            _fighter.Attack(_player); 
        }
        void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
        bool ShouldChase()
        {

            float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);
            return distanceToPlayer < _chaseDistance;
            
        }
        // Called by Unity
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, _chaseDistance);
        }
    }
}
