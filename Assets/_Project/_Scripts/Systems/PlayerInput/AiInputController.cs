using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerInputController;

public class AiInputController : MonoBehaviour
{

    [SerializeField] private bool _isEnemy = true;
    [SerializeField] private float _actionInputDelay = 1;

    private InputState _currentState = InputState.Disabled;

    public void OnTurnStart()
    {
        StartCoroutine(StartAction());
    }

    private IEnumerator StartAction()
    {
        yield return new WaitForSeconds(_actionInputDelay);
        RollDice();
    }

    // Actions
    private void RollDice()
    {
        if (_currentState != InputState.Choosing)
        {
            Debug.LogError("It's not the players turn!");
            return;
        }

        Dice_Manager.Instance.Roll();
    }

    public void ChangeState(InputState newState)
    {
        _currentState = newState;

        switch (_currentState)
        {
            case InputState.Choosing:
                OnTurnStart();
                break;
            case InputState.Disabled:
                break;
        }
    }


    private void InteractWithItemOnSquare()
    {

    }
}
