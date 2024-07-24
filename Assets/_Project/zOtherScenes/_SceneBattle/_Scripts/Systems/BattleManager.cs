using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private BattlePlayerInputManager _playerInputManager;

    [SerializeField] private AudioSource _audioSourceMusic;
    [SerializeField] private AudioClip _audioClipVictoryMusic;
    [SerializeField] private AudioClip _audioClipBattleMusic;


    [SerializeField] PlayerBattler _testPlayer;
    [SerializeField] AIBattler _testEnemy;

    [SerializeField] private BattleMenuManager _menusManager;

    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _player2Prefab;

    [SerializeField] Transform _playerSpawnLocation;
    [SerializeField] Transform _enemySpawnLocation;

    private PlayerBattler _player;
    private AIBattler _enemy;
    public AIBattler enemy
    {
        get { return _enemy; }
    }

    private Queue<Battler> _battlers = new Queue<Battler>();

    private Battler _currentBattler;
    public Battler currentBattler
    {
        get
        {
            return _currentBattler;
        }
    }

    private int _moneyToAward = 0;
    private float _experienceToAward = 0;

    private AIBattler CreateAIBattler(CharacterStats stats)
    {
        switch (stats.id)
        {
            case ("Raptor"):
                GameObject enemyGO = Instantiate(_enemyPrefab, _enemySpawnLocation.position, _enemySpawnLocation.rotation);
                _enemy = enemyGO.GetComponent<AIBattler>();
                _enemy.Initialize(stats, this);
                return _enemy;
                break;

        }
        return null;
    }

    private PlayerBattler CreatePlayerBattler(CharacterStats stats)
    {
        switch (stats.id)
        {
            case ("Player 1"):
                { 
                GameObject playerGO = Instantiate(_playerPrefab, _playerSpawnLocation.position, _playerSpawnLocation.rotation);
                _player = playerGO.GetComponent<PlayerBattler>();
                _player.Initialize(stats, this);
                return _player;
        }
            case ("Player 2"):
                {
                    GameObject playerGO = Instantiate(_player2Prefab, _playerSpawnLocation.position, _playerSpawnLocation.rotation);
                    _player = playerGO.GetComponent<PlayerBattler>();
                    _player.Initialize(stats, this);
                    return _player;
                }
        }
        return null;
    }

    public void Initialize(BattleData battleData)
    {
        Debug.Log(battleData.playerStats.id);

        // spawn prefab for player and attach
        _player = CreatePlayerBattler(battleData.playerStats);

        _enemy = CreateAIBattler(battleData.opponentStats);

        _player.ChangeTarget(_enemy);
        _enemy.ChangeTarget(_player);

        _playerInputManager.Initialize(_player);


        // Initialize the queue
        _battlers = new Queue<Battler>();

        // Add the player and enemy to the queue
        _battlers.Enqueue(_player);
        _battlers.Enqueue(_enemy);



        _currentBattler = _battlers.Peek();

        // update the UI
        _menusManager.UpdateStatsUIInstant(_player.stats, true);
        _menusManager.UpdateStatsUIInstant(_enemy.stats, false);

        _currentBattler.onTurnStart.Invoke();
    }

    private void SwitchToNextBattler()
    {
        Battler oldBattler = _battlers.Dequeue();

        _battlers.Enqueue(oldBattler);

        _currentBattler = _battlers.Peek();

        _currentBattler.onTurnStart.Invoke();
    }

    public void OnMoveExecution(Battler origin, Battler opponent)
    {

        StartCoroutine(OnMoveRoutine());
    }

    private IEnumerator OnMoveRoutine()
    {
        _currentBattler = null;

        yield return new WaitForSeconds(.5f);

        UpdateUI();

        yield return new WaitForSeconds(.5f);

        ClearDead();

        yield return new WaitForSeconds(.5f);

        FinishCurrentTurn();
    }

    public void OnBattleEnd()
    {
        foreach(var battler in _battlers)
        {
            battler.OnWin();
        }

        _player.stats.SetExperience(_player.stats.experience + _experienceToAward);
        _player.stats.SetMoney(_player.stats.money + _moneyToAward);

        _audioSourceMusic.clip = _audioClipVictoryMusic;
        _audioSourceMusic.Play();

        _menusManager.ChangeMenu("Win", page1Text: "You got " + _experienceToAward + " experience.", page2Text: "You got " + _moneyToAward + " money.");
    }

    public Battler GetBattlerAdjacent(Battler battler, bool isNext)
    {
        List<Battler> battlerList = new List<Battler>(_battlers);
        int battlerIndex = battlerList.IndexOf(battler);

        if (isNext)
        {
            // Get the next battler in the list, wrapping around to the start if necessary
            battlerIndex = (battlerIndex + 1) % battlerList.Count;
        }
        else
        {
            // Get the previous battler in the list, wrapping around to the end if necessary
            battlerIndex = (battlerIndex - 1 + battlerList.Count) % battlerList.Count;
        }

        return battlerList[battlerIndex];
    }

    //public AIBattler GetAIBattler()
    //{

    //}

    public void FinishCurrentTurn()
    {
        if (CountEnemies() > 0)
        {
            SwitchToNextBattler();
        }
        else
        {
            Debug.Log("All enemies are dead");
            OnBattleEnd();
        }

        
    }

    public void StartTurn() { 

    }

    public void Attack()
    {
        // Get level

        // use it to find damage
    }

    public void GiveExperience()
    {
        // Give experience to game manager
    }

    private List<Battler> GetDeaths()
    {
        var tempList = new List<Battler>();
        float tolerance = 0.05f; // Define your tolerance value here

        foreach (Battler battler in _battlers)
        {
            if (battler.stats.scaledHealth <= tolerance)
            {
                tempList.Add(battler);
            }
        }

        return tempList;
    }


    private void ClearDead()
    {
        List<Battler> deaths = GetDeaths();

        if (deaths != null)
        {
            foreach (var death in deaths)
            {
                _moneyToAward += death.stats.money;

                _experienceToAward += death.stats.level * 10;

                Destroy(death.gameObject);

                RemoveFromQue(death);
            }
        }


    }

    private void RemoveFromQue(Battler battler)
    {
        Queue<Battler> tempBattlers = new Queue<Battler>();

        foreach (Battler _battler in _battlers)
        {
            if (battler != _battler)
            {
                tempBattlers.Enqueue(_battler);
            }
        }

        _battlers = tempBattlers;
    }

    private void Start()
    {

        //CharacterStats playerStats = new CharacterStats("Player 1");
        //CharacterStats enemyStats = new CharacterStats("Raptor");

        //enemyStats.SetMaxHealth(300);
        //enemyStats.SetHealth(300);

        //enemyStats.SetMoney(34);

        //BattleData battleData = new BattleData(playerStats, enemyStats);

        //Initialize(battleData);
    }

    private int CountEnemies()
    {
        int enemyCount = 0;
        foreach (Battler battler in _battlers)
        {
            if (battler is AIBattler)
            {
                enemyCount++;
            }
        }
        return enemyCount;
    }

    private void UpdateUI(bool isInstant = false)
    {
        Debug.Log("enemy scaled" + _enemy.stats.scaledHealth);
        if (!isInstant)
        {
            _menusManager.UpdateStatsUI(_player.stats, true);
            _menusManager.UpdateStatsUI(_enemy.stats, false);
            return;
        }

        else
        {
            _menusManager.UpdateStatsUIInstant(_player.stats, true);
            _menusManager.UpdateStatsUIInstant(_enemy.stats, false);
        }
    }

    
}
