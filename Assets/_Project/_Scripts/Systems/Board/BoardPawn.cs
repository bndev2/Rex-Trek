using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] protected float _moveSpeed = 5;
    [SerializeField] protected UnityEvent _onMoveStart;
    [SerializeField] protected UnityEvent _onMoveEnd;

    [SerializeField] protected PawnState _currentState = PawnState.Idle;
    protected TurnManager _manager;
    protected SquareController _squareController;

    protected List<Vector3> _path;
    protected int _currentMoveIndex = 0;

    public void ApplyElement(Element element)
    {
        switch (element.elementEffect)
        {
            case ElementEffect.AddTurns:
                _manager.IncreaseTurns(this, (int)element.modifier);
                break;
            case ElementEffect.RemoveTurns:
                _manager.IncreaseTurns(this, (int)-element.modifier);
                break;
            case ElementEffect.MoveForward:
                _manager.MoveAdditiveOverride(this, (int)element.modifier);
                break;
            case ElementEffect.MoveBack:
                break;
        }
    }

    public virtual void SetMove(List<Vector3> path, SquareController squareController)
    {

        if (_squareController != null)
        {
            _squareController.OnLeave(this);
        }

        _squareController = squareController;
    }

    public virtual void FinishTurn()
    {
        _manager.OnFinishMove(this);

        _squareController.OnLand(this);
    }

    public void Initialize(TurnManager manager)
    {
        _manager = manager;
    }

    public void ClearPath()
    {
        
    }
}
