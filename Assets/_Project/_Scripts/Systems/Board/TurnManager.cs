using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum TurnState
{
    Moving,
    Idle,
}

public class TurnManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioPlayerTurnStart;
    [SerializeField] private AudioClip _audioEnemyTurnStart;

    [SerializeField] protected UnityEvent _onTurnEnd;

    [SerializeField] BoardManager _boardManager;
    [SerializeField] Menus_Manager _menusManager;

    [SerializeField] CameraController _cameraController;

    [SerializeField] PlayerInputController _playerInputController;
    [SerializeField] PlayerInputController _player2InputController;
    [SerializeField] AiInputController _aiInputController;

    // The starting pawns
    [SerializeField] List<BoardPawn> _pawns;

    [SerializeField] private int _totalTurns = 0;
    public int totalTurns
    {
        get
        {
            return _totalTurns;
        }
    }

    // Dictionary to hold the number of turns each pawn gets during their next turn.
    private Dictionary<BoardPawn, int> _pawnTurns = new Dictionary<BoardPawn, int>();
    // The List to alternate between the pawns
    private Queue<BoardPawn> _pawnTurnOrder = new Queue<BoardPawn>();

    // The current pawn who's turn it is.
    private BoardPawn _currentPawn;

    private TurnState TurnState = TurnState.Idle;
    private bool _isPlayerTurn = true;


    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        // Pack the pawns into the queue
        foreach (BoardPawn pawn in _pawns)
        {
            _pawnTurnOrder.Enqueue(pawn);

            pawn.Initialize(this, _boardManager);
        }

        // Add the pawns to the _pawnTurns dict with initial turns set to 1
        foreach (BoardPawn pawn in _pawns)
        {
            _pawnTurns[pawn] = 1;
        }

        // Set the current pawn to the first pawn in the queue
        if (_pawnTurnOrder.Count > 0)
        {
            _currentPawn = _pawnTurnOrder.Peek();
        }

    }

    public void PauseCurrentTurn()
    {
        // Stop the pawns movement
        _currentPawn.PauseMove();

        // disable ui input
    }

    public void ResumeCurrentTurn()
    {
        // Stop the pawns movement
        _currentPawn.ResumeMove();

        // Enable ui input
    }

    public void MoveAdditiveCurrent(int amountToMove)
    {
        if(_currentPawn == null)
        {
            Debug.LogError("Pawn null");
        }

        TurnState = TurnState.Moving;

        _boardManager.SetPosition(_currentPawn, _boardManager.GetPositionOnBoardActual(_currentPawn) + amountToMove);
    }

    public void MoveAdditiveOverride(BoardPawn _pawn, int amountToMove)
    {
        MoveToFront(_pawn);

        MoveAdditiveCurrent(amountToMove);
    }

    public void MoveToFront(BoardPawn pawn)
    {
        // Check if the pawn is in the queue
        if (!_pawnTurnOrder.Contains(pawn))
        {
            Debug.LogError("Pawn not found in the turn order queue!");
            return;
        }

        // Create a new queue and add the specific pawn to the front
        Queue<BoardPawn> newQueue = new Queue<BoardPawn>();
        newQueue.Enqueue(pawn);

        // Dequeue all items from the old queue and enqueue them to the new queue
        while (_pawnTurnOrder.Count > 0)
        {
            BoardPawn currentPawn = _pawnTurnOrder.Dequeue();
            // Only enqueue the pawn if it is not the pawn we want to move to the front
            if (!currentPawn.Equals(pawn))
            {
                newQueue.Enqueue(currentPawn);
            }
        }

        // Replace the old queue with the new queue
        _pawnTurnOrder = newQueue;
    }


    public void IncreaseTurns(BoardPawn pawn, int turnsToAdd)
    {
         _pawnTurns[pawn] += turnsToAdd;

        _pawnTurns[pawn] = Mathf.Clamp(_pawnTurns[pawn], 1, int.MaxValue);

        UpdateTurnsUI(pawn);
    }

    private void UpdateTurnsUI(BoardPawn pawn)
    {
        if (pawn.id == "Player 1")
        {
            if (pawn == _currentPawn)
            {
                _playerInputController.ChangeState(InputState.Choosing);
            }
            _menusManager.UpdatePlayerTurnsUI(_pawnTurns[pawn], true);
        }
        else if (pawn.id == "Player 2")
        {
            if (pawn == _currentPawn)
            {
                _player2InputController.ChangeState(InputState.Choosing);
            }
            _menusManager.UpdatePlayerTurnsUI(_pawnTurns[pawn], false);
        }
    }
    public void OnFinishMove(BoardPawn pawn)
    {

        _pawnTurns[_currentPawn] -= 1;

        _totalTurns += 1;

        UpdateTurnsUI(pawn);

        // If their turns are out switch to the next pawn
        if (_pawnTurns[_currentPawn] <= 0)
        {

            _onTurnEnd.Invoke();

            ResetTurns(pawn);

            SwitchToNextTurn();
        }
        else if (_currentPawn.id == "Player 1")
        {
            _playerInputController.ChangeState(InputState.Choosing);
        }
        else if(_currentPawn.id == "Player 2")
        {
            _player2InputController.ChangeState(InputState.Choosing);
        }
        else if (_currentPawn.id == "Enemy")
        {
            _aiInputController.ChangeState(InputState.Choosing);
        }
    }

    // Reset turns back to 1
    public void ResetTurns(BoardPawn pawn)
    {
        _pawnTurns[pawn] = 1;

        if (_currentPawn.id == "Player 1")
        {
            _menusManager.UpdatePlayerTurnsUI(_pawnTurns[pawn], true);
        }
        else if (_currentPawn.id == "Player 2")
        {
            _menusManager.UpdatePlayerTurnsUI(_pawnTurns[pawn], false);
        }
    }

    private void SwitchToNextTurn()
    {
        BoardPawn currentPawn = _pawnTurnOrder.Dequeue();

        _pawnTurnOrder.Enqueue(currentPawn);

        _currentPawn = _pawnTurnOrder.Peek();

        _cameraController.SwitchTarget(_currentPawn.transform);

        if (_currentPawn.id == "Player 1")
        {
            _audioSource.PlayOneShot(_audioPlayerTurnStart);
            _menusManager.ChangeTurnMenu(MenuType.PlayerTurn);
            _playerInputController.ChangeState(InputState.Choosing);
        }
        else if(_currentPawn.id == "Player 2")
        {
            _audioSource.PlayOneShot(_audioPlayerTurnStart);
            _menusManager.ChangeTurnMenu(MenuType.Player2Turn);
            _player2InputController.ChangeState(InputState.Choosing);
        }
        else if(_currentPawn.id == "Enemy")
        {
            _audioSource.PlayOneShot(_audioEnemyTurnStart);
            _menusManager.ChangeTurnMenu(MenuType.EnemyTurn);
            _aiInputController.ChangeState(InputState.Choosing);
        }
    }

    public void AddPawn(BoardPawn pawn)
    {
        // Add the pawn into the turns que and dictionary.
        _pawnTurnOrder.Enqueue(pawn);
        _pawnTurns[pawn] = 1;
        _pawns.Add(pawn);

        pawn.Initialize(this, _boardManager);

        // Add them to the physical board
        _boardManager.AddToBoard(pawn, 0);
    }

}
