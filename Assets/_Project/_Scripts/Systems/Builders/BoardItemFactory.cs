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
        GameObject template = null;

        switch (element.elementEffect)
        {
            case ElementEffect.GiveHealth:
                template = _giveHealthTemplate;
                template.GetComponent<BoardItemController>().SetElement(element);
                break;
            case ElementEffect.MoveBack:
                template = _moveBackTemplate;
                template.GetComponent<BoardItemController>().SetElement(element);
                break;
            case ElementEffect.AddTurns:
                template = _addTurnsTemplate;
                template.GetComponent<BoardItemController>().SetElement(element);
                break;
        }

        if (customTemplate != "")
        {
            switch (customTemplate)
            {
                case "giveHealth":
                    template = _giveHealthTemplate;
                    template.GetComponent<BoardItemController>().SetElement(element);
                    break;
                case "moveBack":
                    template = _moveBackTemplate;
                    template.GetComponent<BoardItemController>().SetElement(element);
                    break;
                case "addTurns":
                    template = _addTurnsTemplate;
                    template.GetComponent<BoardItemController>().SetElement(element);
                    break;
            }
        }

        if (template != null)
        {
            GameObject tempGO = Instantiate(template);
            tempGO.GetComponent<BoardItemController>().SetElement(element);
            return tempGO;
        }

        return null;
    }

    public GameObject GetRandomItem()
    {
        BoardElement element = new BoardElement();
        element.modifier = Random.Range(0, 2);

        float ranNum = Random.Range(0, 60);

        if(ranNum <= 20)
        {
            element.elementEffect = ElementEffect.GiveHealth;
        }
        else if (ranNum <= 40)
        {
            element.elementEffect = ElementEffect.AddTurns;
        }
        else if(ranNum <= 60)
        {
            element.elementEffect= ElementEffect.MoveBack;
        }

        return Instantiate(BuildBoardItem(element));
    }

    public BoardElement BuildBoardElement()
    {
        // Implement this method if needed
        return null;
    }
}
