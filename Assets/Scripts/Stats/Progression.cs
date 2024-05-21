using RPG.Stats;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New  Progression", order = 0)]

    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] _characterClasses = null;

        [Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass _characterClass;
            public ProgressionStat[] _stats;
        }

        [Serializable]
        class ProgressionStat
        {
            public Stat _stat;
            public float[] _levels;
        }

        Dictionary<CharacterClass, Dictionary<Stat, float[]>> _lookupTable;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookup();

            float[] levels =  _lookupTable[characterClass][stat];

            if (levels.Length < level)
            {
                return 0;
            }
            return levels[level - 1];

           //----------------------------------------------------------------------------------//

            /*foreach (ProgressionCharacterClass progressionClass in _characterClasses)
            {
                if (progressionClass._characterClass != characterClass) continue;

                foreach (ProgressionStat progressionStat in progressionClass._stats )
                {
                    if (progressionStat._stat != stat) continue;
                    
                    if (progressionStat._levels.Length < level) continue;
                    
                    return progressionStat._levels[level - 1];
                }
            }
            return 0;*/
            //---------------------------------------------------------------------------------//
        }

        public int GetLevels(Stat stat, CharacterClass characterClass)
        {
            BuildLookup();
            float[] levels = _lookupTable[characterClass][stat];
            return levels.Length;
        }

        // public int GetDamage(Stat stat, CharacterClass characterClass)
        // {
        //     BuildLookup();
        //     float[] damageLevels = lookupTable[characterClass][stat];
        //     return damageLevels.Length;
        // }
        void BuildLookup()
        {
            if (_lookupTable != null) return;

            _lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (ProgressionCharacterClass progressionClass in _characterClasses)
            {
                var statLookupTable = new Dictionary<Stat, float[]>();

                foreach (ProgressionStat progressionStat in progressionClass._stats)
                {
                    statLookupTable[progressionStat._stat] = progressionStat._levels; 
                }
                _lookupTable[progressionClass._characterClass] = statLookupTable;
            }
        }
    }
}


 