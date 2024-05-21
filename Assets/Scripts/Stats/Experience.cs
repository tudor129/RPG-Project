using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats 
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float _experiencePoints = 0;

        public delegate void ExperienceGainedDelegate();
        public event ExperienceGainedDelegate onExperienceGained;
        public void GainExperience(float experience)
        {
            _experiencePoints += experience;
            onExperienceGained();
        }
        public object CaptureState()
        {
            return _experiencePoints;
        }
        public void RestoreState(object state)
        {
            _experiencePoints = (float)state;
        }

        public float GetPoints()
        {
            return _experiencePoints;
        }
    }
}
