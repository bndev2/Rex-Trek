using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardItemFactory : MonoBehaviour
{
    [SerializeField] BoardManager manager;

    [SerializeField] private GameObject _giveHealthTemplate;
    [SerializeField] private GameObject _addTurnsTemplate;
    [SerializeField] private GameObject _removeTurnsTemplate;
    [SerializeField] private GameObject _moveBackTemplate;



    public GameObject BuildBoardItem(BoardElement element, string customTemplate = "")
    {
        return null;
        // Instantiate a prefab

        // If GO contains a script delete it and make a new one.

        
    }

    public BoardElement BuildBoardElement()
    {
        return null;
    }

}
