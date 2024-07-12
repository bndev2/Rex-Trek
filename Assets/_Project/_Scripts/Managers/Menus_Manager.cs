using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public enum MenuType
{
    PlayerTurn,
    Persistent,
    EnemyTurn,
    Transition,
}

public class Menus_Manager : MonoBehaviour
{
    bool isBob = true;

    [SerializeField] GameObject _menuPlayerTurn;
    [SerializeField] GameObject _menuPersistent;
    [SerializeField] GameObject _menuEnemyTurn;

    [SerializeField] GameObject _buttonRollDice;

    [SerializeField] UINotification _turnTransitionNotification;

    private GameObject _currentTurnMenu;

    [SerializeField] GameObject _menuPause;

    public void ChangeTurnMenu(MenuType menuType, bool transition = true, bool clearCurrentScreen = true)
    {
        if (_currentTurnMenu != null && clearCurrentScreen == true)
        {
            _currentTurnMenu.SetActive(false);
        }

        switch (menuType)
        {
            case MenuType.PlayerTurn:
                ExposeInput();
                _turnTransitionNotification.Play("Player Turn");
                _currentTurnMenu = _menuPlayerTurn;
                _menuPlayerTurn.SetActive(true);
                break;
            case MenuType.Persistent:
                break;
            case MenuType.EnemyTurn:
                _turnTransitionNotification.Play("Enemy Turn");
                _currentTurnMenu = _menuEnemyTurn;
                _menuEnemyTurn.SetActive(true);
                break;
            case MenuType.Transition:
                break;
        }
    }

    public void HideInput()
    {
        _buttonRollDice.SetActive(false);
    }

    public void ExposeInput()
    {
        _buttonRollDice.SetActive(true);
    }

    public void PlayNotification(string message)
    {
        _turnTransitionNotification.Play(message);
    }

    private void Awake()
    {
        ChangeTurnMenu(MenuType.PlayerTurn);
    }

    private void Update()
    {


    }
}
