using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class BattleMenuManager : MonoBehaviour
{
    [SerializeField] private Slider _sliderPlayerHealth;
    [SerializeField] private Slider _sliderEnemyHealth;

    [SerializeField] private GameObject _menuPlayerTurn;
    [SerializeField] private GameObject _menuEnemyTurn;

    private GameObject _menuActive;

    public void ChangeMenu(string turnName)
    {
        if(_menuActive != null)
        {
            _menuActive.SetActive(false);
        }

        if (turnName == "Player")
        {
            _menuPlayerTurn.SetActive(true);
            _menuActive = _menuPlayerTurn;
        }
        else if (turnName == "Enemy")
        {
            _menuEnemyTurn.SetActive(true);
            _menuActive = _menuEnemyTurn;
        }
    }

    public void UpdateStatsUI(CharacterStats characterStats, bool isPlayer)
    {
        if (isPlayer)
        {
            LeanTween.value(gameObject, UpdatePlayerHealthBar, _sliderPlayerHealth.value, characterStats.scaledHealth, 1f);
        }
        else if (!isPlayer)
        {
            LeanTween.value(gameObject, UpdateEnemyHealthBar, _sliderEnemyHealth.value, characterStats.scaledHealth, 1f);
        }
    }

    private void UpdatePlayerHealthBar(float value)
    {
        _sliderPlayerHealth.value = value;
    }

    private void UpdateEnemyHealthBar(float value)
    {
        _sliderEnemyHealth.value = value;
    }

}
