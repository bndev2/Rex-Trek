using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BattleManager : MonoBehaviour
{

    [SerializeField] PlayerBattler _testPlayer;
    [SerializeField] AIBattler _testEnemy;

    [SerializeField] private BattleMenuManager _menusManager;

    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _playerPrefab;

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

    public void Initialize(CharacterStats playerStats, CharacterStats opponentStats)
    {
        // spawn prefab for player and attach
        GameObject playerGO = Instantiate(_playerPrefab, _playerSpawnLocation.position, _playerSpawnLocation.rotation);
        _player = playerGO.OverwriteComponent<PlayerBattler>();
        _player.Initialize(playerStats, this);

        GameObject enemyGO = Instantiate(_enemyPrefab, _enemySpawnLocation.position, _enemySpawnLocation.rotation);
        _enemy = enemyGO.OverwriteComponent<AIBattler>();
        _enemy.Initialize(opponentStats, this);

        _player.ChangeTarget(_enemy);
        _enemy.ChangeTarget(_player);

        _currentBattler = _player;

        // update the UI
        _menusManager.UpdateStatsUI(_player.stats, true);
        _menusManager.UpdateStatsUI(_player.stats, false);
    }

    private void SwitchToNextBattler()
    {
        Battler oldBattler = _battlers.Dequeue();

        _currentBattler = _battlers.Peek();

        _battlers.Enqueue(oldBattler);

        _currentBattler.onTurnStart.Invoke();
    }

    public void OnMoveExecution(Battler origin, Battler opponent)
    {

        StartCoroutine(OnMoveRoutine());
    }

    private IEnumerator OnMoveRoutine()
    {
        _currentBattler = null;

        yield return new WaitForSeconds(1.5f);

        UpdateUI();

        yield return new WaitForSeconds(1.5f);

        FinishCurrentTurn();
    }

    public void OnBattleEnd()
    {
        // give the player xp

        // coroutine

        // Switch to overworld
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


    public void FinishCurrentTurn()
    {
        SwitchToNextBattler();
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


    private void Awake()
    {
        _player = _testPlayer;
        _enemy = _testEnemy;

        _enemy.Initialize(new CharacterStats("Enemy"), this);
        _player.Initialize(new CharacterStats("Player"), this);

        _enemy.stats.SetMaxHealth(200);
        _enemy.stats.SetHealth(200);

        _player.ChangeTarget(_enemy);
        _enemy.ChangeTarget(_player);

        // Initialize the queue
        _battlers = new Queue<Battler>();

        // Add the player and enemy to the queue
        _battlers.Enqueue(_player);
        _battlers.Enqueue(_enemy);

        _currentBattler = _battlers.Peek();

        // update the UI
        _menusManager.UpdateStatsUI(_player.stats, true);
        _menusManager.UpdateStatsUI(_player.stats, false);
    }


    private void UpdateUI()
    {
        _menusManager.UpdateStatsUI(_player.stats, true);
        _menusManager.UpdateStatsUI(_enemy.stats, false);
    }
}
