using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class BattleMenuManager : MonoBehaviour
{
    [SerializeField] private Slider _sliderPlayerHealth;
    [SerializeField] private Slider _sliderEnemyHealth;

    [SerializeField] private MultiPageText _multiPageText;

    [SerializeField] private GameObject _menuPlayerTurn;
    [SerializeField] private GameObject _menuWin;
    [SerializeField] private GameObject _menuEnemyTurn;

    private GameObject _menuActive;

    public void ChangeMenu(string turnName, string page1Text = "", string page2Text = "")
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
        else if (turnName == "Win")
        {
            if (page1Text == "" || page2Text == "")
            {
                Debug.LogError("Win screen requires optional parameters page1 and page2 to be entered!");
                return;
            }

            _menuWin.SetActive(true);

            _multiPageText.SetText(0, page1Text);
            _multiPageText.SetText(1, page2Text);

            _menuActive = _menuWin;
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
