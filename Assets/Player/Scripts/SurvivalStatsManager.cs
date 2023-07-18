using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalStatsManager : MonoBehaviour
{
    [SerializeField]
    public SurvivalStatsVariables healthStruct;
    [SerializeField]
    public SurvivalStatsVariables hungerStruct;
    [SerializeField]
    private float hungerDepletionMultiplier;
    [SerializeField]
    private float hungerHealthDepletionAdd;
    [SerializeField]
    public SurvivalStatsVariables thirstStruct;
    [SerializeField]
    private float thirstDepletionMultiplier;
    [SerializeField]
    private float thirstHealthDepletionAdd;
    [SerializeField]
    public SurvivalStatsVariables sleepStruct;
    [SerializeField]
    public SurvivalStatsVariables sanityStruct;
    [SerializeField]
    private float sanityHealthDepletionAdd;
    [SerializeField]
    public SurvivalStatsVariables staminaStruct;

    public SurvivalStats health;
    public SurvivalStats hunger;
    public SurvivalStats thirst;
    public SurvivalStats sleep;
    public SurvivalStats sanity;
    public SurvivalStats stamina;

    private bool _hungerDepletionAddActive = false;
    private bool _thirstDepletionAddActive = false;
    private bool _sanityDepletionAddActive = false;

    private void Start()
    {
        health = new SurvivalStats(healthStruct);
        hunger = new SurvivalStats(hungerStruct);
        thirst = new SurvivalStats(thirstStruct);
        sleep = new SurvivalStats(sleepStruct);
        sanity = new SurvivalStats(sanityStruct);
        stamina = new SurvivalStats(staminaStruct);

        health.StartDepletion();
        hunger.StartDepletion();
        thirst.StartDepletion();
        sleep.StartDepletion();
        
    }


    private void Update()
    {   
        health.Update();
        hunger.Update();
        thirst.Update();
        sleep.Update();
        sanity.Update();
        stamina.Update();


        if (hunger.IsEmpty())
        {
            if (!_hungerDepletionAddActive)
            {
                health.AddDepletionRate(hungerHealthDepletionAdd);
            }
            _hungerDepletionAddActive = true;
        } else
        {
            if (_hungerDepletionAddActive)
            {
                health.SubtractDepletionRate(hungerHealthDepletionAdd);
            }
            _hungerDepletionAddActive = false;
        }
        if (thirst.IsEmpty())
        {
            if (!_thirstDepletionAddActive)
            {
                health.AddDepletionRate(thirstHealthDepletionAdd);
            }
            _thirstDepletionAddActive = true;
        } else
        {
            if (_thirstDepletionAddActive)
            {
                health.SubtractDepletionRate(thirstHealthDepletionAdd);
            }
            _thirstDepletionAddActive = false;
        }
        if (sanity.IsEmpty())
        {
            if (!_sanityDepletionAddActive)
            {
                health.AddDepletionRate(sanityHealthDepletionAdd);
            }
            _sanityDepletionAddActive = true;
        } else
        {
            if (_sanityDepletionAddActive)
            {
                health.SubtractDepletionRate(sanityHealthDepletionAdd);
            }
            _sanityDepletionAddActive = false;
        }

        if (sleep.IsEmpty())
        {
            sanity.SetDepletionRate(0.5f);
            sanity.StartDepletion();
        } else
        {
            sanity.SetDepletionRate(sanityStruct._depletionRate);
            sanity.StartDepletion();
        }

        if (stamina.IsDepleting() && !stamina.IsEmpty())
        {
            hunger.SetDepletionRate(hungerStruct._depletionRate * hungerDepletionMultiplier);
            thirst.SetDepletionRate(thirstStruct._depletionRate * thirstDepletionMultiplier);
        }
        else
        {
            hunger.SetDepletionRate(hungerStruct._depletionRate);
            thirst.SetDepletionRate(thirstStruct._depletionRate);
        }
    }

    public void UseItem(Item item)
    {
        this.health.Add(item.health);
        this.hunger.Add(item.hunger);
        this.thirst.Add(item.thirst);
        this.sleep.Add(item.sleep);
        this.sanity.Add(item.sanity);
    }

    [System.Serializable]
    public struct SurvivalStatsVariables
    {
        [SerializeField]
        public float _max;
        [SerializeField]
        public float _depletionRate;
        [SerializeField]
        public float _depletionDelay;
        [SerializeField]
        public float _recoveryRate;
        [SerializeField]
        public float _recoveryDelay;
    }
    
    public class SurvivalStats
    {
        private SurvivalStatsVariables _variables;

        private float _current;
        private float _depletionDelayTimer;
        private float _recoveryDelayTimer;

        private float percentage { get { return _current / _variables._max; } }

        private bool _onDepletion;
        private bool _onRecovery;

        //Constructor
        public SurvivalStats(SurvivalStatsVariables variables)
        {
            _variables = variables;
            _current = _variables._max;
            _depletionDelayTimer = _variables._depletionDelay;
            _recoveryDelayTimer = _variables._recoveryDelay;
            _onDepletion = false;
            _onRecovery = false;
        }

        public void Update()
        {
            if(_onDepletion)
            {
                Deplete();
            }
            if(_onRecovery)
            {
                Recover();
            }
        }

        private void Deplete()
        {
            if(_depletionDelayTimer <= 0)
            {
                _current = Mathf.Clamp(float.IsNaN(_current - _variables._depletionRate * Time.deltaTime) ? 0 : _current - _variables._depletionRate * Time.deltaTime, 0, _variables._max);
            }
            else
            {
                _depletionDelayTimer -= Time.deltaTime;
            }
        }

        private void Recover()
        {
            if(_recoveryDelayTimer <= 0)
            {
                _current = Mathf.Clamp(float.IsNaN(_current + _variables._recoveryRate * Time.deltaTime) ? 0 : _current + _variables._recoveryRate * Time.deltaTime, 0, _variables._max);
            }
            else
            {
                _recoveryDelayTimer -= Time.deltaTime;
            }
        }

        public void StartDepletion()
        {
            _onDepletion = true;
            
        }

        public void StopDepletion()
        {
            _onDepletion = false;
            _depletionDelayTimer = _variables._depletionDelay;
        }

        public void StartRecovery()
        {
            _onRecovery = true;
            
        }

        public void StopRecovery()
        {
            _onRecovery = false;
            _recoveryDelayTimer = _variables._recoveryDelay;
        }

        public bool IsEmpty()
        {
            return _current <= 0;
        }

        public bool IsFull()
        {
            return _current >= _variables._max;
        }

        public float Add(float value)
        {
            _current = Mathf.Clamp(_current + value, 0, _variables._max);
            return _current;
        }

        public float Subtract(float value)
        {
            _current = Mathf.Clamp(_current - value, 0, _variables._max);
            return _current;
        }

        public void SetDepletionRate(float value)
        {
            _variables._depletionRate = value;
        }

        public void SetRecoveryRate(float value)
        {
            _variables._recoveryRate = value;
        }

        public float GetPercentage()
        {
            return percentage;
        }

        public float GetCurrentValue()
        {
            return _current;
        }

        public bool IsDepleting()
        {
            return _onDepletion;
        }

        public void AddDepletionRate(float value)
        {
            _variables._depletionRate += value;
        }

        public void SubtractDepletionRate(float value)
        {
            _variables._depletionRate -= value;
        }

        public bool IsRecovering()
        {
            return _onRecovery;
        }
    }
}
