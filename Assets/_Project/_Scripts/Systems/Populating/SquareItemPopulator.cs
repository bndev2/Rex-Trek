using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SquareItemPopulator : MonoBehaviour
{
    [SerializeField] float _percentContainingItem = 40;


    [SerializeField] private List<GameObject> _lowLevelItems;
    [SerializeField] private List<GameObject> _midLevelItems;
    [SerializeField] private List<GameObject> _highLevelItems;

    public void Populate(List<SquareController> squares)
    {
        // Check that all gameobjs contain a Board item controller


        for (int i = 0; i < squares.Count; i++)
        {
            GameObject item;

            if (i < 30)
            {
                item = _lowLevelItems[Random.Range(0, _lowLevelItems.Count)];
            }
            else if (i >= 30 && i < 60)
            {
                item = _midLevelItems[Random.Range(0, _midLevelItems.Count)];
            }
            else
            {
                item = _highLevelItems[Random.Range(0, _highLevelItems.Count)];
            }

            // 60% chance for no item to be added
            float ranNum = Random.Range(0, 100);
            if (ranNum > 100 - _percentContainingItem)
            {
                item = Instantiate(item);
                squares[i].AddItem(item);
            }
            else
            {

            }
        }
    }
}
