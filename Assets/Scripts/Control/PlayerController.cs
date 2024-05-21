using RPG.Attributes;
using RPG.Combat;
using RPG.Movement;
using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Mover _mover; 
        CombatTarget _target;
        Fighter _attacker;
        Health _health;
        CursorMapping _cachedCursorMapping;

        

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] _cursorMappings;
        [SerializeField] float _maxNavMeshProjectionDistance = 1f;
        [SerializeField] float _maxNavPathLength = 40f;
        void Awake()
        {
            _mover = GetComponent<Mover>();
            _target = GetComponent<CombatTarget>();
            _attacker = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            
        }
        void Update()
        {
            if (InteractWithUI())
            {
                return;
            }
            if (_health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            }
            if (InteractWithComponent())
            {
                return;
            }
            if (InteractWithMovement())
            {
                return;
            }
            
            SetCursor(CursorType.None);
        }
        bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }
        RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            float[] distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            
            Array.Sort(distances, hits);
            return hits;
        }
        bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }
        bool InteractWithMovement()
        {
            Vector3 target;
            
            bool hasHit = RaycastNavMesh(out target);

            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    _mover.StartMoveAction(target, 1f);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }
        bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (!hasHit)
            {
                return false;
            }
            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(hit.point, out navMeshHit, _maxNavMeshProjectionDistance, NavMesh.AllAreas);
            if (!hasCastToNavMesh)
            {
                return false;
            }
            target = navMeshHit.position;
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
            if (!hasPath)
            {
                return false;
            }
            if (path.status != NavMeshPathStatus.PathComplete)
            {
                return false;
            }
            if (GetPathLength(path) > _maxNavPathLength)
            {
                return false;
            }
            
            return true;
        }
        float GetPathLength(NavMeshPath path)
        {
            float total = 0;
            if (path.corners.Length < 2)
            {
                return total;
            }

            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }
            
            return total;
        }

        void SetCursor(CursorType type)
        {
            if (_cachedCursorMapping.type == type)
            {
                return;
            }
            _cachedCursorMapping = GetCursorMapping(type);
            Cursor.SetCursor(_cachedCursorMapping.texture, _cachedCursorMapping.hotspot, CursorMode.Auto);
        }
        CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in _cursorMappings)
            {
                if (mapping.type == type) 
                    return mapping; 
            }
            return _cursorMappings[0];
        }
        static Ray GetMouseRay() 
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
        
        public Weapon GetPlayerCurrentWeapon()
        {
            return _attacker.GetCurrentWeapon();
        }

        
    }
}
