using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthElement : MonoBehaviour, IBoardElement
{
    [SerializeField] private float _healthToGive = 1;
    [SerializeField] private bool _isHealthRandom = true;

    [SerializeField] private float _min;
    [SerializeField] private float _max;

    public void Apply(PlayerController playerController)
    {
        playerController.GiveHealth(_healthToGive);
    }

    public void Remove(PlayerController playerController)
    {

    }

    void Awake()
    {
        if (_isHealthRandom)
        {
            _healthToGive = Random.Range(_min, _max);
        }
    }

    public IBoardElement Clone()
    {
        var clone = Instantiate(this);
        // Copy any additional state here...
        return clone;
    }
}
