using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class BattleMenuManager : MonoBehaviour
{
    [SerializeField] private Image _sliderPlayerHealth;
    [SerializeField] private Image _sliderEnemyHealth;

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
        if(isPlayer)
        {
            _sliderPlayerHealth.fillAmount = characterStats.scaledHealth;
        }
        else if (isPlayer)
        {
            _sliderEnemyHealth.fillAmount = characterStats.scaledHealth;
        }
    }
}
