using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Events;

public enum PawnState
{
    Idle,
    Moving,
    Disabled,
}

public class BoardPawn : MonoBehaviour
{
    [SerializeField] private string _id;
    public string id
    {
        get
        {
            return _id;
        }
    }

    public int positionIndex
    {
        get
        {
            return _boardManager.GetPositionOnBoardActual(this);
        }
    }

    [SerializeField] protected float _moveSpeed = 5;
    [SerializeField] protected UnityEvent _onMoveStart;
    [SerializeField] protected UnityEvent _onMoveEnd;

    [SerializeField] protected PawnState _currentState = PawnState.Idle;
    protected TurnManager _turnManager;
    protected BoardManager _boardManager;

    protected SquareController _squareController;

    protected List<Vector3> _path;

    protected int _currentMoveIndex = 0;

    protected Coroutine _waitToFinishRoutine;
    public virtual bool SetMove(List<Vector3> path, SquareController squareController)
    {

        if (_squareController != null)
        {
            _squareController.OnLeave(this);
        }

        _squareController = squareController;

        return true;
    }

    public virtual void PauseMove()
    {

    }

    public virtual void ResumeMove()
    {

    }

    public virtual void FinishTurn()
    {
        bool isItemPresent = _squareController.OnLand(this);

        // If a coroutine is already running, stop it
        if (_waitToFinishRoutine != null)
        {
            StopCoroutine(_waitToFinishRoutine);
        }

        if (isItemPresent)
        {
            _waitToFinishRoutine = StartCoroutine(WaitToFinish(3));
        }
        else
        {
            _waitToFinishRoutine = StartCoroutine(WaitToFinish(0));
        }
    }

    // Coroutine to delay the finishing of a turn
    private IEnumerator WaitToFinish(float delay)
    {
        yield return new WaitForSeconds(delay);

        _turnManager.OnFinishMove(this);
        _waitToFinishRoutine = null;
    }



    public void Initialize(TurnManager manager, BoardManager boardManager)
    {
        _turnManager = manager;
        _boardManager = boardManager;
    }

    public void ClearPath()
    {
        
    }
}
