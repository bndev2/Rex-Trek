using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;

public enum LevelState{
    Overworld,
    Battle,
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private Menus_Manager _overworldMenusManager;

    private bool _hasInitialized = false;
    private void Start()
    {

    }


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

    [SerializeField] private List<CharacterStats> _playerStats = new List<CharacterStats>();

    private BattleData _battleData;

    public void ChangeState(LevelState levelState)
    {

        // exiting the state
        switch (state)
        {
            case LevelState.Overworld:
                DeactivateScene("DefaultScene");
                break;
            case LevelState.Battle:
                SceneManager.UnloadSceneAsync("BattleScene");
                //EndBattle();
                break;
        }

        state = levelState;

        switch (state)
        {
            case LevelState.Overworld:
                ActivateScene("DefaultScene");
                UpdateOverworldUI();
                break;
            case LevelState.Battle:
                // Plop the relevant data into persistent data
                SceneManager.LoadScene("BattleScene", LoadSceneMode.Additive);
                break;
            default:
                break;

        }
    }

    private void UpdateOverworldUI()
    {
        _overworldMenusManager.UpdateHealthUI(GetPlayerStats("Player 1").scaledHealth, true);
        _overworldMenusManager.UpdateHealthUI(GetPlayerStats("Player 2").scaledHealth, false);
    }

    public void DeactivateScene(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (scene.isLoaded)
        {
            foreach (GameObject rootObject in scene.GetRootGameObjects())
            {
                rootObject.SetActive(false);
            }
        }
    }

    public void ActivateScene(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (scene.isLoaded)
        {
            foreach (GameObject rootObject in scene.GetRootGameObjects())
            {
                rootObject.SetActive(true);
            }
        }
    }

    // Delete
    public void BattleSwitch()
    {
        CharacterStats enemyStats = new CharacterStats("Raptor");

        enemyStats.SetMaxHealth(300);
        enemyStats.SetHealth(300);

        enemyStats.SetMoney(34);

        enemyStats.SetExperience(Random.Range(0, 500));

        _battleData = new BattleData(_playerStats[0], enemyStats);
        StartBattle(_battleData);
    }

    public void EnterBattle(CharacterStats player, CharacterStats opponent)
    {
        StartCoroutine(EnterBattleCoroutine(player, opponent));
    }

    public IEnumerator EnterBattleCoroutine(CharacterStats player, CharacterStats opponent)
    {
        BattleData battleData = new BattleData(player, opponent);

        ChangeState(LevelState.Battle);

        yield return null; // Wait for one frame

        BattleManager battleManager;
        while ((battleManager = FindFirstObjectByType<BattleManager>()) == null)
        {
            yield return null; // Wait for one frame
        }

        // Pass the CharacterStats from the GameManager to the BattleManager
        battleManager.Initialize(battleData);
    }

    public void StartBattle(BattleData battleData)
    {
        _battleData = battleData;
        ChangeState(LevelState.Battle);
        StartCoroutine(InitializeBattle());
    }

    private IEnumerator InitializeBattle()
    {
        yield return null; // Wait for one frame

        BattleManager battleManager;
        while ((battleManager = FindFirstObjectByType<BattleManager>()) == null)
        {
            yield return null; // Wait for one frame
        }

        // Pass the CharacterStats from the GameManager to the BattleManager
        battleManager.Initialize(_battleData);
    }


    public CharacterStats CreatePlayerStats(string id)
    {
        CharacterStats newStats = new CharacterStats(id);
        _playerStats.Add(newStats);

        return _playerStats.Find(stats => stats.id == id);
    }



    public void EndBattle()
    {
        _battleData = null;
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

[System.Serializable]
public class CharacterStats
{
    public string traceID;
    [SerializeField] private int _money;
    public int money
    {
        get
        {
            return _money;
        }
    }
    public float scaledHealth
    {
        get
        {
            return _health / _maxHealth;
        }
    }

    [SerializeField] private float _maxHealth;

    public float maxHealth
    {
        get
        {
            return _maxHealth;
        }
    }
    [SerializeField] private string _id;
    public string id
    {
        get
        {
            return _id;
        }
    }
    [SerializeField] private float _health;
    public float health
    {
        get
        {
            return _health;
        }
    }
    [SerializeField] private int _level;
    public int level
    {
        get
        {
            return _level;
        }
    }
    [SerializeField] private float _experience;
    public float experience
    {
        get { 
         return _experience; 
        }
    }

    public void SetHealth(float health)
    {
        _health = Mathf.Clamp(health, 0, _maxHealth);
    }


    public bool SetExperience(float experience)
    {
        this._experience = experience;

        int oldLevel = _level;

        _level = ((int)experience / 100) + 1;

        if (oldLevel < _level)
        {
            _maxHealth = _level * 100;
            _health = _maxHealth;
            return true; // Level up!
        }
        return false; // No level up
    }

    public void SetLevel(int newLevel)
    {
        _experience = (newLevel * 100) + 1;

        _level = newLevel;

        _maxHealth = _level * 100;
        _health = _maxHealth;
    }

    public void SetMaxHealth(float newMaxHealth)
    {
        _maxHealth = newMaxHealth;

        _health = Mathf.Clamp(_health, 0, _maxHealth);

    }

    public void SetMoney(int newMoney)
    {
        _money = newMoney;
    }

    public CharacterStats(string id)
    {
        _id = id;

        _level = 1;

        _health = 100;

        _experience = 0;

        _maxHealth = 100;
    }

    public CharacterStats(CharacterStats original)
    {
        traceID = original.traceID;
        _money = original._money;
        _maxHealth = original._maxHealth;
        _id = original._id;
        _health = original._health;
        _level = original._level;
        _experience = original._experience;
    }
}

public class BattleData
{
    private CharacterStats _playerStats;
    private CharacterStats _opponentStats;

    public CharacterStats playerStats
    {
        get { return _playerStats; }
    }
    public CharacterStats opponentStats
    {
        get { return _opponentStats; }
    }

    public BattleData(CharacterStats playerStats, CharacterStats opponentStats)
    {
        _playerStats = playerStats;
        _opponentStats = opponentStats;
    }
}