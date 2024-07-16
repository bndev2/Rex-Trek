using System.Collections.Generic;
using UnityEngine;

public class SquaresManager : MonoBehaviour
{
    [SerializeField] private Transform _startingTransform;
    [SerializeField] private int _startingSquares = 10;
    [SerializeField] private float _spacing;
    [SerializeField] private GameObject _squarePrefab;
    [SerializeField] private GameObject _squareParent; // Add this line

    private List<SquareController> _squareControllers = new List<SquareController>();
    [SerializeField] private BoardItemFactory _boardItemFactory;

    private void Awake()
    {
        //Initialize();
    }

    public void SetItem(SquareController squareController, BoardElement boardElement)
    {
        GameObject boardItemGO = _boardItemFactory.BuildBoardItem(boardElement);
        squareController.AddItem(boardItemGO);
    }

    public void RemoveItem(int squareIndex)
    {
        _squareControllers[squareIndex].RemoveItem();
    }

    public void AddSquareToEnd(int number = 1, bool isXAxis = true)
    {
        for (int i = 0; i < number; i++)
        {
            Vector3 lastSquarePosition = _squareControllers.Count > 0 ? _squareControllers[_squareControllers.Count - 1].transform.position : _startingTransform.position;
            Vector3 newPosition = lastSquarePosition + (isXAxis ? new Vector3(_spacing, 0, 0) : new Vector3(0, 0, _spacing));
            AddSquare(newPosition);
        }
    }

    private void AddSquare(Vector3 position)
    {
        GameObject squarePrefab = Instantiate(_squarePrefab, position, Quaternion.identity, _squareParent.transform); // Change this line
        _squareControllers.Add(squarePrefab.GetComponent<SquareController>());
    }

    private void Initialize()
    {
        // Populate the squares
        for (int i = 0; i < _startingSquares; i++)
        {
            bool isXAxis = Random.value >= 0.5;
            AddSquareToEnd(1, isXAxis);
        }

        // Add the items
        for (int i = 0; i < _squareControllers.Count; i++)
        {
            GameObject boardItemInstance = _boardItemFactory.GetRandomItem();

            _squareControllers[i].AddItem(boardItemInstance);
        }
    }
}
