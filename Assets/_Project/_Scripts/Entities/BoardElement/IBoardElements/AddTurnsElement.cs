using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AddTurnsElement : MonoBehaviour, IBoardElement
{
    [SerializeField] private int _min = 1;
    [SerializeField] private int _max = 2;

    private int _turnsToAdd;

    public void Apply(PlayerController playerController)
    {
        playerController.GiveTurns(_turnsToAdd);
    }

    public void Remove(PlayerController playerController)
    {
        // Implement remove behavior here...
    }

    public void SetRange(int min, int max)
    {
        _min = min;
        _max = max;

        _turnsToAdd = Mathf.RoundToInt(Random.Range(_min, _max));
    }
    private void Awake()
    {
        _turnsToAdd = Mathf.RoundToInt(Random.Range(_min, _max));
    }

    public IBoardElement Clone()
    {
      var clone = Instantiate(this);
      // Copy any additional state here...
      return clone;
    }

}