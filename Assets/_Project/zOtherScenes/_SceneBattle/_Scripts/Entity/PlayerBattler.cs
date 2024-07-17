using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattler : Battler, IHasGun
{
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

        _stats.SetHealth(_stats.health - actualDamage);
    }
}
