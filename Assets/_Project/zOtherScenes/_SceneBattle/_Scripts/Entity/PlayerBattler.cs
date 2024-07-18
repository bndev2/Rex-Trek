using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerBattler : Battler, IHasGun
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _playerTurnStart;
    [SerializeField] private AudioClip _rifleTest;

    public override void OnPlayerTurnStart()
    {
        _audioSource.PlayOneShot(_playerTurnStart);
    }

    public override void Attack(Battler battler)
    {
        if(_manager.currentBattler != this)
        {
            return;
        }
    }

    public override void AttackCurrent()
    {

        if (_manager.currentBattler != this)
        {
            return;
        }
        if(_currentTarget == null)
        {
            return;
        }

        _audioSource.PlayOneShot(_rifleTest);

        // damage the target
        _currentTarget.Damage(5);

        _manager.OnMoveExecution(this, _currentTarget);
    }

    public override void Run()
    {
        // Player specific behaviour eg animation

        // Coroutibe

        // Switch to the regular overworld
        GameManager.Instance.ChangeState(LevelState.Overworld);
    }

    protected override void UpdateUI()
    {

    }

    public void Shoot(Battler battler)
    {
        if (_manager.currentBattler != this)
        {
            return;
        }


        // gun specific behaviour eg firing

        //coroutine delay

        // Tell the battle manager
        _manager.OnMoveExecution(this, battler);

        // damage the target
        battler.Damage(5);
    }

    public void Shoot()
    {
        if (_manager.currentBattler != this)
        {
            return;
        }

        // gun specific behaviour eg firing

        //coroutine delay

        // Tell the battle manager
        _manager.OnMoveExecution(this, _currentTarget);

        // damage the target
        _currentTarget.Damage(5);
    }

    public override void Damage(float damage)
    {
        float actualDamage = damage - stats.level * 2;

        actualDamage = actualDamage + Random.Range(.1f, 1);

        _stats.SetHealth(_stats.health - actualDamage);
    }

    public void SwitchToNextTarget()
    {
        if (_manager.currentBattler != this)
        {
            return;
        }

        _currentTarget = _manager.GetBattlerAdjacent(_currentTarget, true);

        Debug.Log(_currentTarget.stats.id);
    }

    public void SwitchToPreviousTarget()
    {
        if (_manager.currentBattler != this)
        {
            return;
        }

        _currentTarget = _manager.GetBattlerAdjacent(_currentTarget, false);

        Debug.Log(_currentTarget.stats.id);
    }

    void Start() {
        _currentTarget = _manager.enemy;
    }
}
