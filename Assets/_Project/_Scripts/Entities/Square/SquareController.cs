using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementEffect
{
    AddTurns,
    RemoveTurns,
    MoveBack,
    MoveForward,
}

public class Element
{
    public ElementEffect elementEffect;
    // (The number of turns the effect will last (non applicable for some effects)
    public int effectDuration;
    // The value for the effect (non applicable for some effects)
    public float modifier;
}

public class SquareController : MonoBehaviour
{
    float _onSquareTime;

    Element element;

    // Executes when a pawn lands on the square
    public void OnLand(BoardPawn _pawn)
    {
        Debug.Log("Land" + _pawn.id.ToString());
    }

    // Executes when a pawn remains on the square
    public void OnRemain(BoardPawn _pawn)
    {
        Debug.Log("Remain");
    }

    // Executes when a pawn leaves the square
    public void OnLeave(BoardPawn _pawn)
    {
        Debug.Log("Leave");
    }

    public void OnPass(BoardPawn _pawn)
    {
        Debug.Log("Pass" + _pawn.id.ToString());
    }
}
