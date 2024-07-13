using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public enum MenuType
{
    PlayerTurn,
    Player2Turn,
    Persistent,
    EnemyTurn,
    Transition,
}

public class Menus_Manager : MonoBehaviour
{
    bool isBob = true;

    [SerializeField] GameObject _menuPlayerTurn;
    [SerializeField] GameObject _menuPlayer2Turn;
    [SerializeField] GameObject _menuPersistent;
    [SerializeField] GameObject _menuEnemyTurn;

    [SerializeField] GameObject _buttonRollDicePlayer1;
    [SerializeField] GameObject _buttonRollDicePlayer2;

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
                ExposeInput(true);
                _turnTransitionNotification.Play("Player 1 Turn");
                _currentTurnMenu = _menuPlayerTurn;
                _menuPlayerTurn.SetActive(true);
                break;
            case MenuType.Player2Turn:
                ExposeInput(false);
                _turnTransitionNotification.Play("Player 2 Turn");
                _currentTurnMenu = _menuPlayer2Turn;
                _menuPlayer2Turn.SetActive(true);
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

    public void HideInput(bool isPlayer1)
    {
        if (isPlayer1)
        {
            _buttonRollDicePlayer1.SetActive(false);
        }
        else
        {
            _buttonRollDicePlayer2.SetActive(false);
        }
    }

    public void ExposeInput(bool isPlayer1)
    {
        if (isPlayer1)
        {
            _buttonRollDicePlayer1.SetActive(true);
        }
        else
        {
            _buttonRollDicePlayer2.SetActive(true);
        }
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
