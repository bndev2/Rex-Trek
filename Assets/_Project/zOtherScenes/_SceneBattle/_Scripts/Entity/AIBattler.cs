using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBattler : Battler
{
    [SerializeField] private Animator _animator;

    private IEnumerator HandleAttackCurrent()
    {
        _animator.CrossFade("Attack", .05f);

        yield return new WaitForSeconds(1);

        _animator.CrossFade("Idle", .05f);

        _currentTarget.Damage(4);

        _manager.OnMoveExecution(this, _currentTarget);
    }

    public override void OnPlayerTurnStart()
    {
        AttackCurrent();
    }
    public override void Attack(Battler battler)
    {
        // Implement player attack logic here
    }

    public override void AttackCurrent()
    {
        StartCoroutine(HandleAttackCurrent());
    }

    public override void Run()
    {
        // Implement player run logic here
    }

    protected override void UpdateUI()
    {
        // Implement player UI update logic here
    }

    public override void Damage(float damage)
    {
        float actualDamage = damage - stats.level * 2;

        _stats.SetHealth(_stats.health - actualDamage);

        Debug.Log("Youch that hurt! " + stats.health.ToString());
    }
}
