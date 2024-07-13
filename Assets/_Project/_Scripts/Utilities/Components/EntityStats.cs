using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    public float maxHealth
    {
        get { return _maxHealth; }
    }
    [SerializeField] private float _startingHealth;
    private float _currentHealth;
    public float currentHealth
    {
        get { return _currentHealth; }
    }
    public float currentScaledHealth
    {
        get { return _currentHealth / _maxHealth; }


    }

    private void Awake()
    {
        if (_startingHealth <= _maxHealth && _startingHealth >= 0)
        {
            _currentHealth = _startingHealth;
        }
    }

    public void SetCurrentHealth(float newHealth)
    {
        if (newHealth < 0)
        {
            Debug.LogError("less than zero.");
            return;
        }

        if (newHealth > _maxHealth)
        {
            Debug.LogError("less than max");
            return;
        }

        _currentHealth = newHealth;
    }

    public void SetMaxHealth(float newMaxHealth)
    {
        if (newMaxHealth < 0)
        {
            Debug.LogError("less than zero.");
            return;
        }

        _maxHealth = newMaxHealth;
    }

    private void Update()
    {

    }
}
