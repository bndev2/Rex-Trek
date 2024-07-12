using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] BoardManager _boardManager;
    [SerializeField] Menus_Manager _menuManager;
    [SerializeField] BoardPawn _playerPawn;

    public enum InputState
    {
        Choosing,
        Disabled,
    }

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
                _menuManager.ExposeInput();
                break;
            case InputState.Disabled:
                _menuManager.HideInput();
                break;
        }
    }


    private void Start()
    {

    }
}
