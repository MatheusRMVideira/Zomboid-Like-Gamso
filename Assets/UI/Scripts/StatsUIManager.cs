using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsUIManager : MonoBehaviour
{
    [SerializeField]
    private SurvivalStatsManager _survivalStatsManager;

    [SerializeField]
    private Image _healthBar,
                  _hungerBar,
                  _thirstBar,
                  _sleepBar,
                  _sanityBar,
                  _staminaBar;

    private void FixedUpdate()
    {
        _healthBar.fillAmount = _survivalStatsManager.health.GetPercentage();
        _hungerBar.fillAmount = _survivalStatsManager.hunger.GetPercentage();
        _thirstBar.fillAmount = _survivalStatsManager.thirst.GetPercentage();
        _sleepBar.fillAmount = _survivalStatsManager.sleep.GetPercentage();
        _sanityBar.fillAmount = _survivalStatsManager.sanity.GetPercentage();
        _staminaBar.fillAmount = _survivalStatsManager.stamina.GetPercentage();
    }
}
