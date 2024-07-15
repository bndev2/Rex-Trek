using System.Collections;
using System.Collections.Generic;
using System.Net;
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

    private int _lastPawnPosition = -1;

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

    /// <summary>
    /// Determines if a given pawn's path intersects with any other pawn on the board.
    /// </summary>
    /// <param name="pawn">The pawn whose path is being checked for intersections.</param>
    /// <param name="path">The list of Vector3 positions representing the path of the pawn.</param>
    /// <returns>
    /// Returns the index at which the intersection occurs in the path. If no intersection is found, returns -1.
    /// </returns>
    public int PathIntersects(BoardPawn pawn, List<Vector3> path)
    {
        // Iterate over each position in the path
        for (int i = 0; i < path.Count; i++)
        {
            Vector3 position = path[i];

            // Get the square at the current position
            SquareController square = GetSquare(position);

            // If the square is null, continue to the next position
            if (square == null) continue;

            // Get the pawns at the current square
            List<BoardPawn> pawnsAtSquare = GetPawns(square);

            // If there are any pawns at the square (excluding the current pawn), return the index
            if (pawnsAtSquare.Exists(p => p != pawn))
            {
                return i;
            }
        }

        // If no intersections were found, return -1
        return -1;
    }

    /// <summary>
    /// Computes the path between two points on the board, represented by their indices.
    /// </summary>
    /// <param name="start">The index of the starting point on the board.</param>
    /// <param name="end">The index of the ending point on the board.</param>
    /// <returns>
    /// Returns a list of Vector3 positions representing the path from the start to the end point. If the start and end points are the same, or if they are out of bounds, returns null.
    /// </returns>
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

    public PlayerController GetClosestPlayer(BoardPawn pawn)
    {
        int pawnPosition = GetPositionOnBoardActual(pawn);
        if (pawnPosition == -1)
        {
            Debug.LogError("Pawn not found on the board!");
            return null;
        }

        PlayerController closestPlayer = null;
        int closestDistance = int.MaxValue;

        for (int i = 0; i < _pawns.Count; i++)
        {
            for (int j = 0; j < _pawns[i].Count; j++)
            {
                PlayerController otherPlayer = _pawns[i][j] as PlayerController;
                if (otherPlayer == null || otherPlayer == pawn) continue; // Skip non-players and the pawn itself

                int distance = Mathf.Abs(i - pawnPosition);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPlayer = otherPlayer;
                }
            }
        }

        return closestPlayer;
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

        bool isFinished = pawn.SetMove(path, _squareControllers[newPosition]);

        if (isFinished)
        {
            UpdatePawnPosition(pawn, newPosition);

            _currentState = BoardState.Moving;
        }
    }

    private void UpdatePawnPosition(BoardPawn pawn, int oldPosition, int newPosition)
    {
        _pawns[oldPosition].Remove(pawn);
        _pawns[newPosition].Add(pawn);
    }

    private void UpdatePawnPosition(BoardPawn pawn, int newPosition)
    {
        for(int i = 0; i < _pawns.Count; i++)
        {
            for(int j = 0; j < _pawns[i].Count; j++)
            {
                if (_pawns[i][j] == pawn)
                {
                    _pawns[i].RemoveAt(j);
                }
            }
        }

        _pawns[newPosition].Add(pawn);
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
    public List<BoardPawn> GetPawns(int squareIndex)
    {
        int index = squareIndex;

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

public class BoardPath
{
    private BoardManager _manager;
    private int _originIndex;
    private int _destinationIndex;
    private List<Vector3> _points;

    public int Distance
    {
        get { return _points.Count - 1; }
    }

    public int Length
    {
        get { return _points.Count; }
    }

    public bool IsIntersecting
    {
        get
        {
            return GetIntersectPosition() != -1;
        }
    }


    public BoardPath(int origin, int end, BoardManager manager)
    {
        _manager = manager;
        _originIndex = origin;
        _destinationIndex = end;
        _points = _manager.GetPath(origin, end);
    }

    public void Shorten(int newEndPosition)
    {
        _destinationIndex = newEndPosition;
        _points = _manager.GetPath(_originIndex, newEndPosition);
    }

    public void Negate()
    {
        int temp = _originIndex;
        _originIndex = _destinationIndex;
        _destinationIndex = temp;
        _points.Reverse();
    }

    public void Scale(int factor)
    {
        // Scaling a path might not be as straightforward as multiplying the index,
        // as it depends on how your BoardManager handles positions.
        // You might need to implement a method in BoardManager to handle this.
    }

    /// <summary>
    /// Gets the position of the first intersection along the path.
    /// </summary>
    /// <returns>The index of the first intersection, or -1 if there is no intersection or if the origin and destination are the same.</returns>
    public int GetIntersectPosition()
    {
        if (_destinationIndex == _originIndex)
        {
            return -1;
        }

        int step = _destinationIndex > _originIndex ? 1 : -1;

        for (int i = _originIndex; i != _destinationIndex; i += step)
        {
            List<BoardPawn> _pawns = _manager.GetPawns(i);

            if (_pawns != null && _pawns.Count > 0)
            {
                return i;
            }
        }

        return -1;
    }

}

