using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveElement : MonoBehaviour, IBoardElement
{
    [SerializeField] private int _spacesToMove = 1;

    [SerializeField] private int _min;
    [SerializeField] private int _max;

    public void Apply(PlayerController playerController)
    {
        playerController.MoveSpaces(_spacesToMove);
    }

    public void Remove(PlayerController playerController)
    {
        // Implement remove behavior here...
    }

    private void Awake()
    {
        _spacesToMove = Random.Range(_min, _max);
    }

    public IBoardElement Clone()
    {
        var clone = Instantiate(this);
        // Copy any additional state here...
        return clone;
    }
}
