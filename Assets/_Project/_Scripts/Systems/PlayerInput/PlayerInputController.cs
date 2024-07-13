using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private bool _isPlayer1 = false;

    [SerializeField] BoardManager _boardManager;
    [SerializeField] Menus_Manager _menuManager;
    [SerializeField] BoardPawn _playerPawn;

    private InputState _currentState = InputState.Choosing;

    public void RollDice()
    {
        if(_currentState != InputState.Choosing)
        {
            Debug.LogError("It's not the players turn!");
            return;
        }

        ChangeState(InputState.Disabled);

        Dice_Manager.Instance.Roll();
    }

    public void EnterMiniGame()
    {

    }

    public void ChangeState(InputState newState)
    {
        _currentState = newState;

        switch (_currentState)
        {
            case InputState.Choosing:
                _menuManager.ExposeInput(_isPlayer1);
                break;
            case InputState.Disabled:
                _menuManager.HideInput(_isPlayer1);
                break;
        }
    }


    private void Start()
    {

    }
}
