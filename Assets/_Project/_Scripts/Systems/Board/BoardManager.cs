using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum BoardState
{
    Idle,
    Moving,
}

public class BoardManager : MonoBehaviour
{

    [SerializeField] private List<Transform> _squareTransforms;
    private List<Vector3> _squarePositions;
    private List<SquareController> _squareControllers;

    // Positions of the pawns on the board
    [SerializeField] private List<List<BoardPawn>> _pawns;

    [SerializeField] private int _playerIndex = 0;
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
        _squarePositions = new List<Vector3>();
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

        _pawns = new List<List<BoardPawn>>(_squarePositions.Count);

        // Fill the _pawns list with empty sublists
        for (int i = 0; i < _squarePositions.Count; i++)
        {
            _pawns.Add(new List<BoardPawn>());
        }

        if (_player == null)
        {
            Debug.LogError("Player pawn is not set!");
            return;
        }

        AddToBoard(_player, _playerIndex);
        AddToBoard(_player2, _playerIndex);
    }

    public List<Vector3> GetPath(int start, int end)
    {
        if (start < 0 || end >= _squarePositions.Count)
        {
            Debug.LogError("Invalid start or end position for path!");
            return null;
        }

        List<Vector3> tempList = new List<Vector3>();

        if (start < end)
        {
            for (int i = start; i <= end; i++)
            {
                tempList.Add(_squarePositions[i]);
            }
        }
        else if (start > end)
        {
            for (int i = start; i >= end; i--)
            {
                tempList.Add(_squarePositions[i]);
            }
        }
        else
        {
            return null;
        }

        return tempList;
    }

    public List<Vector3> GetPath(BoardPawn origin, BoardPawn end)
    {
        int startIndex = GetPositionOnBoardActual(origin);
        int endIndex = GetPositionOnBoardActual(end);

        return GetPath(startIndex, endIndex);
    }

    public BoardPawn GetClosest(BoardPawn pawn)
    {
        int pawnPosition = GetPositionOnBoardActual(pawn);
        if (pawnPosition == -1)
        {
            Debug.LogError("Pawn not found on the board!");
            return null;
        }

        BoardPawn closestPawn = null;
        int closestDistance = int.MaxValue;

        for (int i = 0; i < _pawns.Count; i++)
        {
            for (int j = 0; j < _pawns[i].Count; j++)
            {
                BoardPawn otherPawn = _pawns[i][j];
                if (otherPawn == pawn) continue; // Skip the pawn itself

                int distance = Mathf.Abs(i - pawnPosition);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPawn = otherPawn;
                }
            }
        }

        return closestPawn;
    }


    public void SetPosition(BoardPawn pawn, int newPosition)
    {
        if (newPosition < 0 || newPosition >= _squarePositions.Count)
        {
            Debug.LogError("Invalid new position!");
            return;
        }

        int oldPosition = GetPositionOnBoardActual(pawn);
        if (oldPosition == -1)
        {
            Debug.LogError("Pawn not found on the board!");
            return;
        }

        List<Vector3> path = GetPath(oldPosition, newPosition);
        if (path == null)
        {
            return;
        }

        pawn.SetMove(path, _squareControllers[newPosition]);

        _pawns[oldPosition].Remove(pawn);
        _pawns[newPosition].Add(pawn);

        _currentState = BoardState.Moving;
    }

    public void AddToBoard(BoardPawn pawn, int positionIndex)
    {
        if (positionIndex < 0 || positionIndex >= _pawns.Count)
        {
            Debug.LogError("Invalid position index!");
            return;
        }

        _pawns[positionIndex].Add(pawn);
        pawn.transform.position = _squarePositions[positionIndex];
    }

    public void RemoveFromBoard(BoardPawn pawn)
    {
        int index = GetPositionOnBoardActual(pawn);
        if (index == -1)
        {
            Debug.LogError("Pawn not found on the board!");
            return;
        }

        Destroy(pawn.gameObject);
        _pawns[index].Remove(pawn);
    }

    public int GetPositionOnBoardActual(BoardPawn pawn)
    {
        for (int i = 0; i < _pawns.Count; i++)
        {
            if (_pawns[i].Contains(pawn))
            {
                return i;
            }
        }
        return -1;
    }

    public int GetDistance(BoardPawn startPawn, BoardPawn targetPawn)
    {
        int startPos = GetPositionOnBoardActual(startPawn);
        int targetPos = GetPositionOnBoardActual(targetPawn);

        if (startPos == -1 || targetPos == -1)
        {
            Debug.LogError("One or both pawns are not found on the board!");
            return -1;
        }

        return Mathf.Abs(startPos - targetPos);
    }

    public SquareController GetSquare(int index)
    {
        return _squareControllers[index];
    }


    public SquareController GetSquare(Vector3 position)
    {
        for(int i = 0; i < _squareTransforms.Count; i++)
        {
            if (_squareTransforms[i].position.Equals(position))
            {
                return _squareControllers[i];
            }
        }

        return null;
    }

    public List<BoardPawn> GetPawns(SquareController square)
    {
        int index = _squareControllers.IndexOf(square);

        List<BoardPawn> tempList = new List<BoardPawn>();

        for (int i = 0; i < _pawns[index].Count; ++i)
        {
            tempList.Add(_pawns[index][i]);
        }

        return tempList;
    }

    public List<PlayerController> GetPlayers(SquareController square)
    {
        List<BoardPawn> pawns = GetPawns(square);

        List<PlayerController> playersTemp = new List<PlayerController>();

        foreach (BoardPawn p in pawns)
        {
            PlayerController player = p as PlayerController;
            if (player != null)
            {
                playersTemp.Add(player);
            }
        }

        return playersTemp;
    }

}
