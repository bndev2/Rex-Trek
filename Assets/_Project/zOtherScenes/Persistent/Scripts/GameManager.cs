using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum LevelState{
    Overworld,
    Battle,
}

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        // If the singleton hasn't been initialized yet
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private LevelState state = LevelState.Overworld;

    private List<CharacterStats> _playerStats = new List<CharacterStats>();



    private CharacterStats _battleOpponent;
    private CharacterStats _battlePlayer;

    public void ChangeState(LevelState levelState)
    {

        // exiting the state
        switch (state)
        {
            case LevelState.Overworld:
                break;
            case LevelState.Battle:
                SceneManager.UnloadSceneAsync("BattleScene");
                break;
        }

        state = levelState;

        switch (state)
        {
            case LevelState.Overworld:
                SceneManager.LoadScene("DefaultScene");
                // Plop the relevant data into persistent data
                break;
            case LevelState.Battle:
                // Plop the relevant data into persistent data
                SceneManager.LoadScene("BattleScene", LoadSceneMode.Additive);
                break;
            default:
                break;

        }
    }

    // Delete
    public void BattleSwitch()
    {
        ChangeState(LevelState.Battle);
    }

    public void StartBattle(CharacterStats playerStats, CharacterStats opponentStats)
    {
        _battlePlayer = playerStats;

        _battleOpponent = opponentStats;

        ChangeState(LevelState.Battle);
    }

    public void StartBattle(string playerID, CharacterStats opponentStats)
    {
        _battlePlayer = GetPlayerStats(playerID);

        _battleOpponent = opponentStats;

        ChangeState(LevelState.Battle);
    }

    public void EndBattle()
    {
        // Update UI inside main scene
    }

    public void SetPlayerHealth(string id, float health) {

    CharacterStats playerStats = GetCharacterStats(id, _playerStats);

    playerStats.SetHealth(health);

    }

    public void IncreasePlayerHealth(string id, float healthIncrease)
    {
        CharacterStats playerStats = GetCharacterStats(id, _playerStats);

        playerStats.SetHealth(playerStats.health + healthIncrease);
    }

    public void DecreasePlayerHealth(string id, float healthDecrease)
    {
        CharacterStats playerStats = GetCharacterStats(id, _playerStats);

        playerStats.SetHealth(playerStats.health - healthDecrease);
    }

    public void IncreasePlayerExperience(string id, float experienceIncrease)
    {
        CharacterStats playerStats = GetCharacterStats(id, _playerStats);
        playerStats.SetHealth(playerStats.experience + experienceIncrease); // This should be SetExperience
    }


    public bool SetPlayerExperience(string id, float experience)
    {
     CharacterStats playerStats = GetCharacterStats(id, _playerStats);

     bool isLevelUp = playerStats.SetExperience(experience);

      if (isLevelUp)
      {
            // Handle level up (e.g., increase player's max health, improve abilities, etc.)
            return true;
      }

      return false;
    }

    private CharacterStats GetCharacterStats(string id, List<CharacterStats> stats)
    {
        for (int i = 0; i < stats.Count; i++)
        {
            if (stats[i].id == id)
            {
                return stats[i];
            }
        }

        CharacterStats newStats = new CharacterStats(id);

        _playerStats.Add(newStats);

        return newStats;
    }

    public CharacterStats GetPlayerStats(string id)
    {
        return GetCharacterStats(id, _playerStats);
    }

}

public class CharacterStats
{
    public float scaledHealth
    {
        get
        {
            return _health / _maxHealth;
        }
    }

    private float _maxHealth;

    public float maxHealth
    {
        get
        {
            return _maxHealth;
        }
    }
    private string _id;
    public string id
    {
        get
        {
            return _id;
        }
    }
    private float _health;
    public float health
    {
        get
        {
            return _health;
        }
    }
    private int _level;
    public int level
    {
        get
        {
            return _level;
        }
    }
    private float _experience;
    public float experience
    {
        get { 
         return _experience; 
        }
    }

    public void SetHealth(float health)
    {
        this._health = health;

        this._health = Mathf.Clamp(health, 0, _maxHealth);
    }

    public bool SetExperience(float experience)
    {
        this._experience = experience;

        int oldLevel = _level;

        _level = ((int)experience / 100) + 1;

        _maxHealth = _level * 100;

        if (oldLevel < _level)
        {
            return true; // Level up!
        }
        return false; // No level up
    }

    public void SetMaxHealth(float newMaxHealth)
    {
        _maxHealth = newMaxHealth;

        _health = Mathf.Clamp(_health, 0, _maxHealth);

    }

    public CharacterStats(string id)
    {
        _id = id;

        _level = 1;

        _health = 100;

        _experience = 0;

        _maxHealth = 100;
    }
}
