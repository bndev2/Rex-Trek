using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;

public enum BoardState
{
    Idle,
    Moving,
}

public class BoardManager : MonoBehaviour
{
    [SerializeField] List<Transform> _squareTransforms;

    // Positions of the squares
    private List<Vector3> _squarePositions;
    private List<SquareController> _squareControllers;
    // Positions of the pawns on the board
    [SerializeField] private List<BoardPawn> _pawns = new List<BoardPawn>();

    // Player current and start index
    [SerializeField] private int _playerIndex = 0;

    // References to the player and enemy pawns
    [SerializeField] private BoardPawn _player;
    [SerializeField] private BoardPawn _player2;

    private BoardState _currentState = BoardState.Idle;

    private void Awake()
    {
        Initialize();
        if (_squarePositions == null || _squarePositions.Count == 0)
        {
            Debug.LogError("Square positions are not set!");
            return;
        }
    }

    private void Initialize()
    {
        _squarePositions = new List<Vector3>(); // Initialize the list
        _squareControllers = new List<SquareController>();

        foreach (Transform _squareTransform in _squareTransforms)
        {
            _squarePositions.Add(_squareTransform.position);
            SquareController squareController = _squareTransform.GetComponent<SquareController>();
            if (squareController == null)
            {
                Debug.LogError("SquareController component not found on " + _squareTransform.name);
                continue;
            }
            _squareControllers.Add(squareController);
        }

        _pawns.Capacity = _squarePositions.Count;

        // Fill the _pawns list with null elements
        for (int i = 0; i < _pawns.Capacity; i++)
        {
            _pawns.Add(null);
        }

        if (_player == null)
        {
            Debug.LogError("Player pawn is not set!");
            return;
        }

        AddToBoard(_player, _playerIndex);
    }


    private List<Vector3> GetPath(int start, int end)
    {
        if (start < 0 || end >= _squarePositions.Count)
        {
            Debug.LogError("Invalid start or end position for path!");
            return null;
        }

        List<Vector3> tempList = new List<Vector3>();


        if (start < end)
        {
            for (int i = start; i < end + 1; i++)
            {
                tempList.Add(_squarePositions[i]);
            }
        }
        else if (start > end)
        {
            for (int i = start; i > end - 1; i--)
            {
                tempList.Add(_squarePositions[i]);
            }
        }
        else if(start == end)
        {
            return null;
        }

        return tempList;
    }




    public void SetPosition(int pawnIndex, int newPosition)
    {
        if (pawnIndex < 0 || pawnIndex >= _pawns.Count || newPosition < 0 || newPosition >= _squarePositions.Count)
        {
            Debug.LogError("Invalid pawn index or new position!");
            return;
        }

        if (pawnIndex == _playerIndex)
        {
            _playerIndex = newPosition;
        }

        if (pawnIndex == newPosition)
        {
            return;
        }


        BoardPawn pawnRef = _pawns[pawnIndex];

        List<Vector3> path = GetPath(pawnIndex, newPosition);

        

        if (path == null)
        {
            return;
        }


        pawnRef.SetMove(path, _squareControllers[newPosition]);

        _pawns[newPosition] = pawnRef;

        _pawns[pawnIndex] = null;

        _currentState = BoardState.Moving;
    }

    public void SetPosition(BoardPawn pawn, int newPosition)
    {
        if (newPosition < 0 || newPosition >= _squarePositions.Count)
        {
            Debug.LogError("Invalid new position!");
            return;
        }

        List<Vector3> path = GetPath(_pawns.IndexOf(pawn), newPosition);

        if (path == null)
        {
            return;
        }

        if (pawn == null)
        {
            Debug.LogError("Pawn is null!");
            return;
        }

        pawn.SetMove(path, _squareControllers[newPosition]);

        BoardPawn pawnRef = pawn;

        _pawns[_pawns.IndexOf(pawn)] = null;

        _pawns[newPosition] = pawnRef;

        _currentState = BoardState.Moving;
    }


    public void AddToBoard(BoardPawn pawn, int positionIndex) // Changed GameObject to BoardPawn
    {
        if (positionIndex < 0 || positionIndex >= _pawns.Count)
        {
            Debug.LogError("Invalid position index!");
            return;
        }

        if (_pawns[positionIndex] != null)
        {
            Debug.LogError("Position is already occupied!");
            return;
        }

        _pawns[positionIndex] = pawn;
        pawn.transform.position = _squarePositions[positionIndex];

        //pawn.Initialize(this);
    }

    public void RemoveFromBoard(BoardPawn pawn)
    {
        if (!_pawns.Contains(pawn))
        {
            Debug.LogError("Pawn not found on the board!");
            return;
        }

        Destroy(pawn.gameObject); // Changed gameobject to gameObject
        _pawns.Remove(pawn);
    }

    public void RemoveFromBoard(int index)
    {
        if (index < 0 || index >= _pawns.Count)
        {
            Debug.LogError("Invalid index!");
            return;
        }

        Destroy(_pawns[index].gameObject); // Changed pawn.transform.gameobject to _pawns[index].gameObject
        _pawns.RemoveAt(index);
    }

    public int GetPositionOnBoardReadable(BoardPawn pawn)
    {
        int index = _pawns.IndexOf(pawn);
        if (index == -1)
        {
            Debug.LogError("Pawn not found on the board!");
            return -1;
        }

        return index + 1;
    }
    public int GetPositionOnBoardActual(BoardPawn pawn)
    {
        int index = _pawns.IndexOf(pawn);
        if (index == -1)
        {
            Debug.LogError("Pawn not found on the board!");
            return -1;
        }

        return index;
    }

    public int GetDistance(BoardPawn _startPawn, BoardPawn _targetPawn)
    {
        // Get the positions of the start and target pawns on the board
        int startPos = GetPositionOnBoardActual(_startPawn);
        int targetPos = GetPositionOnBoardActual(_targetPawn);

        // Check if the pawns are on the board
        if (startPos == -1 || targetPos == -1)
        {
            Debug.LogError("One or both pawns are not found on the board!");
            return -1;
        }

        // Calculate and return the distance
        return Mathf.Abs(startPos - targetPos);
    }


    private void Start()
    {

    }
}
