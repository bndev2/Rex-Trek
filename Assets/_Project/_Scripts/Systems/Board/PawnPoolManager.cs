using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnPoolManager : MonoBehaviour
{


    [SerializeField] TurnManager _turnManager;

    [SerializeField] List<BoardPawn> _startingPawns;

    // Key: Number of turns before adding, Value: the pawns to add.
    private Dictionary<int, List<BoardPawn>> _pawnPool = new Dictionary<int, List<BoardPawn>>();

    private void Start()
    {
        AddToPool(_startingPawns[0], 2);
    }

    public void UpdateAdding()
    {
        int currentTurnNumber = _turnManager.totalTurns;

        // Check if the key exists in the dictionary
        if (!_pawnPool.ContainsKey(currentTurnNumber))
        {
            return;
        }

        for (int i = 0; i < _pawnPool[currentTurnNumber].Count; i++)
        {
            _turnManager.AddPawn(_pawnPool[currentTurnNumber][i]);
        }

        // Remove the pawns from the dictionary
        _pawnPool.Remove(currentTurnNumber);
    }

    public void AddToPool(BoardPawn pawn, int numberOfTurns)
    {
        if (numberOfTurns < _turnManager.totalTurns)
        {
            Debug.Log(numberOfTurns.ToString() + " is less than the number of turns that have already been played which is " + _turnManager.totalTurns.ToString());
            return;
        }

        // Check if the key exists in the dictionary, if not add it
        if (!_pawnPool.ContainsKey(numberOfTurns))
        {
            _pawnPool[numberOfTurns] = new List<BoardPawn>();
        }

        _pawnPool[numberOfTurns].Add(pawn);
    }

}
