using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterBattleElement : MonoBehaviour, IBoardElement
{
    [SerializeField] private bool _isEnemyTypeRandom = true;

    [SerializeField] private int _minLevel;
    [SerializeField] private int _maxLevel = 5;

    private List<CharacterStats> _enemys = new List<CharacterStats> { new CharacterStats("Raptor") };

    [SerializeField] CharacterStats _enemyCharacterStats;

    public void Apply(PlayerController playerController)
    {
        playerController.ForceTurnEnd();

        playerController.Engage(_enemyCharacterStats);
    }

    public void Remove(PlayerController playerController)
    {
        
    }

    private void CreateEnemy()
    {
        int index = Random.Range(0, _enemys.Count);
        CharacterStats newEnemy = new CharacterStats(_enemys[index]); // Use the copy constructor
        newEnemy.SetLevel(Random.Range(_minLevel, _maxLevel));
        _enemyCharacterStats = newEnemy;
    }

    public void SetRange(int min, int max)
    {
        // Ensure _minLevel is 1 or greater
        _minLevel = Mathf.Max(min, 1);
        _maxLevel = max;

        if (_isEnemyTypeRandom)
        {
            CreateEnemy();
        }
        else
        {
            _enemyCharacterStats = new CharacterStats(_enemyCharacterStats); // Use the copy constructor
            _enemyCharacterStats.SetLevel(Random.Range(_minLevel, _maxLevel));
        }
    }



    private void Awake()
    {
        SetRange(_minLevel, _maxLevel);
    }
    public IBoardElement Clone()
    {
        var clone = Instantiate(this);
        // Copy any additional state here...
        return clone;
    }
}
