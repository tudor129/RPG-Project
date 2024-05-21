using GameDevTV.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using System;
using UnityEngine;
using GameDevTV.Utils;
using RPG.UI.DamageText;
using System.Collections;
using UnityEngine.Events;

namespace RPG.Attributes

{
    public class Health : MonoBehaviour, ISaveable
    {
        public event EventHandler OnDeath;
        
        [SerializeField] float _regenerationPercentage = 70;
        [SerializeField] UnityEvent<float> _takeDamage;

        LazyValue<float> _healthPoints;

        bool _isDead;
        bool _healthUpdated;

        void Awake()
        {
            _healthPoints = new LazyValue<float>(GetInitialHealth);
        }
        float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }
        void Start()
        {
           _healthPoints.ForceInit();
        }
        void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }
        void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }
        void RegenerateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (_regenerationPercentage / 100);
            _healthPoints.value = Mathf.Max(_healthPoints.value, regenHealthPoints);
        }
        public bool IsDead()
        {
            return _isDead;
        }
        public void TakeDamage(GameObject instigator, float damage)
        {
            _healthPoints.value = Mathf.Max(_healthPoints.value - damage, 0);
           
            if (_healthPoints.value <= 0)
            {
                _takeDamage.Invoke(damage);
                 OnDeath?.Invoke(this, EventArgs.Empty);
                 HasDied();
                 AwardExperience(instigator);
            }
            else 
            {
                _takeDamage.Invoke(damage);
            }
        }
        public float GetHealthPoints()
        {
            return _healthPoints.value;
        }
        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }
        public float GetPercentage()
        {
            return 100 * (_healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health));
        }
        public float GetFraction()
        {
            float getFraction = _healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health);

            if (getFraction < 0)
            {
                return 0;
            }
            
            return getFraction;
        }
        void HasDied() 
        {
            if (_isDead)
            {
                return;
            }

            _isDead = true;
            
            
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
        void AwardExperience(GameObject instigator)
        {
            Experience experience =  instigator.GetComponent<Experience>();
            if (experience == null) return;
            
            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }
        public object CaptureState()
        {
            return _healthPoints;
        }
        public void RestoreState(object state)
        {
            _healthPoints.value = (float)state;
            
            if (_healthPoints.value <= 0)
            {
                HasDied(); 
            }
        }
    }
}
