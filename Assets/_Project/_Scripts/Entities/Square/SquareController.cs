using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ElementEffect
{
    AddTurns,
    RemoveTurns,
    MoveBack,
    MoveForward,
    GiveHealth,
}

[System.Serializable]
public class BoardElement
{
    [SerializeField] BoardManager _boardManager;

    public ElementEffect elementEffect;
    // (The number of turns the effect will last (non applicable for some effects)
    public int effectDuration;
    // The value for the effect (non applicable for some effects)
    public float modifier;

    public void Apply(PlayerController playerController)
    {
        switch (elementEffect)
        {
            case ElementEffect.AddTurns:
                break;
            case ElementEffect.RemoveTurns:
                break;
            case ElementEffect.MoveBack:
                break;
            case ElementEffect.MoveForward:
                break;
            case ElementEffect.GiveHealth:
                playerController.GiveHealth(20);
                break;
            default:
                break;
        }
    }

    public void Remove(PlayerController playerController)
    {
        switch (elementEffect)
        {
            case ElementEffect.AddTurns:
                break;
            case ElementEffect.RemoveTurns:
                break;
            case ElementEffect.MoveBack:
                break;
            case ElementEffect.MoveForward:
                break;
            case ElementEffect.GiveHealth:
                break;
            default:
                break;
        }
    }

}

public class SquareController : MonoBehaviour
{
    float _onSquareTime;

    private BoardElement _element;

    // Executes when a pawn lands on the square
    public void OnLand(BoardPawn _pawn)
    {
        Debug.Log("Land" + _pawn.id.ToString());

        if (_pawn is PlayerController playerController)
        {

        }
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


    public void AddItem()
    {

    }

    public void RemoveItem()
    {

    }
}
