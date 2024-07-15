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

    //public void ApplyElement(Element element)
    //{
        //switch (element.elementEffect)
        //{
            //case ElementEffect.AddTurns:
                //_turnManager.IncreaseTurns(this, (int)element.modifier);
                //break;
            //case ElementEffect.RemoveTurns:
                //_turnManager.IncreaseTurns(this, (int)-element.modifier);
                //break;
            //case ElementEffect.MoveForward:
                //_turnManager.MoveAdditiveOverride(this, (int)element.modifier);
                //break;
            //case ElementEffect.MoveBack:
                //break;
        //}
    //}
    // Behavior depends on board manager
    public virtual bool SetMove(List<Vector3> path, SquareController squareController)
    {

        if (_squareController != null)
        {
            _squareController.OnLeave(this);
        }

        _squareController = squareController;

        return true;
    }

    public virtual void FinishTurn()
    {

        _turnManager.OnFinishMove(this);

        _squareController.OnLand(this);
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
