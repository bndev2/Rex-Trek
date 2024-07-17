using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private BattleMenuManager _menusManager;

    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _playerPrefab;

    [SerializeField] Transform _playerSpawnLocation;
    [SerializeField] Transform _enemySpawnLocation;

    private PlayerBattler _player;
    private AIBattler _enemy;

    private Queue<Battler> _battlers;
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
    }

    public void OnMoveExecution(Battler origin, Battler opponent)
    {

        // Move the camera to the target

        // Coroutine delay

        // Move it back

        // Finish the turn
        FinishCurrentTurn();
    }

    public void OnBattleEnd()
    {
        // give the player xp

        // coroutine

        // Switch to overworld
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

}
