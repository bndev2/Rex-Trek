using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum RexMentalState
{
    PursuingPlayer,
    Preoccupied,
}

public class RexController : BoardPawn
{
    [SerializeField] private float _damageAmount = 10;

    private bool _hasMovedThisTurn = false;
    private RexMentalState _currentMentalState = RexMentalState.PursuingPlayer;

    public override bool SetMove(List<Vector3> path, SquareController squareController)
    {
        _currentMoveIndex = 0;

        if (_currentMentalState == RexMentalState.PursuingPlayer && !_hasMovedThisTurn)
        {
            MoveTowardsClosestPlayer(path);
            return false;
        }

        StartMoving(path, squareController);
        return true;
    }

    private void MoveTowardsClosestPlayer(List<Vector3> path)
    {
        PlayerController closestPlayer = _boardManager.GetClosestPlayer(this);
        int closestPlayerDirection = (int)Mathf.Sign(_boardManager.GetPositionOnBoardActual(closestPlayer) - _boardManager.GetPositionOnBoardActual(this));
        int closestPlayerDistance = _boardManager.GetDistance(this, closestPlayer);
        int spacesToMove = closestPlayerDirection * Mathf.Min(path.Count - 1, closestPlayerDistance);

        _hasMovedThisTurn = true;
        _boardManager.SetPosition(this, _boardManager.GetPositionOnBoardActual(this) + spacesToMove);
    }

    private void StartMoving(List<Vector3> path, SquareController squareController)
    {
        _currentMoveIndex = 0;
        base.SetMove(path, squareController);
        _path = path;
        _currentState = PawnState.Moving;
    }

    public override void FinishTurn()
    {
        _hasMovedThisTurn = false;
        base.FinishTurn();
    }

    private void HandleDamagingPlayers(SquareController square)
    {
        List<PlayerController> players = _boardManager.GetPlayers(square);

        if (players != null)
        {
            foreach (PlayerController player in players)
            {
                player.DoDamage(_damageAmount);
            }
        }
    }

    private void Update()
    {
        HandleMove();
    }

    private void HandleMove()
    {
        if (_currentState == PawnState.Moving && _currentMoveIndex < _path.Count)
        {
            MoveToNextPositionInPath();

            if (_currentMoveIndex >= _path.Count)
            {
                FinishMove();
            }
        }
    }

    private void MoveToNextPositionInPath()
    {
        Vector3 position = _path[_currentMoveIndex];
        transform.position = Vector3.MoveTowards(transform.position, position, _moveSpeed * Time.deltaTime);

        Vector3 direction = position - transform.position;
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, _moveSpeed * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, position) < 0.01f)
        {
            transform.position = position;
            SquareController passingSquare = _boardManager.GetSquare(position);
            passingSquare.OnPass(this);
            HandleDamagingPlayers(passingSquare);
            _currentMoveIndex += 1;
        }
    }

    private void FinishMove()
    {
        _onMoveEnd.Invoke();
        transform.position = _path[_path.Count - 1];
        ChangeState(PawnState.Idle);
        _path.Clear();
        FinishTurn();
    }

    private void ChangeState(PawnState pawnState)
    {
        _currentState = pawnState;
    }
}
