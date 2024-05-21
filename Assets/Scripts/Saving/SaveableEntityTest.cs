using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntityTest : MonoBehaviour
    {
        [SerializeField] string _uniqueIdentifier = "";
        public string GetUniqueIdentifier()
        {
            return _uniqueIdentifier;
        }

        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                state[saveable.GetType().ToString()] = saveable.CaptureState();
            }
            return state;

            //return new SerializableVector3Test(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3Test position = (SerializableVector3Test)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = position.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
#if UNITY_EDITOR
        void Update()
        {
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;
            
            
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");    
                print("Editing");

                if (string.IsNullOrEmpty(property.stringValue))
                {
                    property.stringValue = System.Guid.NewGuid().ToString();
                    serializedObject.ApplyModifiedProperties();
                }   
        }
#endif        
    }
}
 