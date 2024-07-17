using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBattler : Battler
{
    public override void Attack(Battler battler)
    {
        // Implement player attack logic here
    }

    public override void AttackCurrent()
    {
        // Implement player attack current logic here
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
    }
}
