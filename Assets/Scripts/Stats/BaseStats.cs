using RPG.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Utils;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int _startingLevel = 1;
        [SerializeField] CharacterClass _characterClass;
        [SerializeField] Progression _progression = null;
        [SerializeField] GameObject _levelUpParticleEffect = null;
        [SerializeField] bool _shouldUseModifiers = false;
        Experience _experience;

        LazyValue<int> _currentLevel;

        public event Action onLevelUp;


        void Awake()
        {
            _experience = GetComponent<Experience>();
            _currentLevel = new LazyValue<int>(InitializeLevel);
        }

        int InitializeLevel() 
        {
            return CalculateLevel();
        }

        void Start()
        {
            _currentLevel.ForceInit();
        }

        void OnEnable()
        {
            if (_experience != null)
            {
                _experience.onExperienceGained += UpdateLevel;
            }
        }
        void OnDisable()
        {
            if (_experience != null)
            {
                _experience.onExperienceGained -= UpdateLevel;
            }
        }

        void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > _currentLevel.value)
            {
                _currentLevel.value = newLevel;
                LevelUpEffect();
                onLevelUp?.Invoke();
            }
        }
        void LevelUpEffect()
        {
            Instantiate(_levelUpParticleEffect, transform);
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat)/100);
        }
        
        float GetBaseStat(Stat stat)
        {
            return _progression.GetStat(stat, _characterClass, GetLevel());
        }

        public int GetLevel()
        {
            return _currentLevel.value;
        }
        
        float GetAdditiveModifier(Stat stat)
        {
            if (!_shouldUseModifiers) return 0;
            
            float total = 0;
            foreach (IModifierProvider provider  in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetAdditiveModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }
        
        float GetPercentageModifier(Stat stat)
        {
            if (!_shouldUseModifiers) return 0;
            //this is the sum of all of the percentage modifiers 
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();

            if (experience == null) return _startingLevel;
            
            float currentXP = experience.GetPoints();
            int penultimateLevel = _progression.GetLevels(Stat.ExperienceToLevelUp, _characterClass);
            for (int level = 1; level <= penultimateLevel; level++)
            {
                float XPtoLevelUp = _progression.GetStat(Stat.ExperienceToLevelUp, _characterClass, level);
                if (XPtoLevelUp > currentXP)
                {
                    return level;
                }
            }
            return penultimateLevel + 1;
        }
    }
}
    