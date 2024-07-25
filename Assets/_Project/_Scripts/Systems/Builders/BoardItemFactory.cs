using System.Collections.Generic;
using UnityEngine;

public class BoardItemFactory : MonoBehaviour
{
    [SerializeField] private BoardManager manager;
    [SerializeField] private GameObject _giveHealthTemplate;
    [SerializeField] private GameObject _addTurnsTemplate;
    [SerializeField] private GameObject _removeTurnsTemplate;
    [SerializeField] private GameObject _moveBackTemplate;
    [SerializeField] private GameObject _vfxPrefab;

    public GameObject BuildBoardItem(BoardElement element, string customTemplate = "")
    {
        return null;
    }

    public GameObject GetRandomItem()
    {
        return null;
    }

    public BoardElement BuildBoardElement()
    {
        return null;
    }
}
