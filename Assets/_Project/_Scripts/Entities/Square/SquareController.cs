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
    EnterBattle,
}

[System.Serializable]
public class BoardElement
{
    [SerializeField] BoardManager _boardManager;

    public ElementEffect elementEffect;

    [SerializeField] private IBoardElement _boardElement;
    // (The number of turns the effect will last (non applicable for some effects)
    public int effectDuration;
    // The value for the effect (non applicable for some effects)
    public float modifier;

    public void Apply(PlayerController playerController)
    {
        _boardElement.Apply(playerController);
    }

    public void Remove(PlayerController playerController)
    {
        _boardElement.Remove(playerController);
    }

}

public class SquareController : MonoBehaviour
{

     //private BoardElement _element;
    [SerializeField] private BoardItemController _itemController;
    [SerializeField] private GameObject _itemGO;

    // Executes when a pawn lands on the square. returns true if item is present
    public bool OnLand(BoardPawn _pawn)
    {
        Debug.Log("Land" + _pawn.id.ToString());

        if (_itemGO != null)
        {
            if (_pawn is PlayerController playerController)
            {
                _itemGO.SetActive(true);
                _itemController.ApplyEffect(playerController);
            }

            return true;
        }

        return false;
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


    public void AddItem(GameObject boardItemGameObject)
    {
        BoardItemController itemController = boardItemGameObject.GetComponent<BoardItemController>();

        if (itemController == null)
        {
            Debug.LogError("No item controller is attached!");
            return;
        }

        _itemController = itemController;
        _itemGO = boardItemGameObject;

        boardItemGameObject.transform.position = transform.position;

        boardItemGameObject.SetActive(false);
    }

    public void RemoveItem()
    {
        if(_itemGO == null || _itemController == null) {
            return;
        }

        Destroy(_itemGO);
        _itemGO = null;
        _itemController = null;
    }

    private void Update()
    {

    }
}
