using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Battler : MonoBehaviour
{
    protected BattleManager _manager;
    protected CharacterStats _stats;
    public CharacterStats stats
    {
        get { return _stats; }
    }
    protected bool _canAttack = false;
    protected Battler _currentTarget;

    public void Initialize(CharacterStats stats, BattleManager manager)
    {
        if(stats != null)
        {
            Debug.LogError("Battler has already been initialized!");
            return;
        }

        _stats = stats;
        _manager = manager;
        UpdateUI();
    }

    public abstract void Attack(Battler battler);

    public abstract void AttackCurrent();

    public abstract void Run();

    public abstract void Damage(float damageAmount);

    public void ChangeTarget(Battler battler)
    {
        _currentTarget = battler;
    }

    protected abstract void UpdateUI();
}

public interface IHasGun
{
    void Shoot(Battler battler);
    void Shoot();
}
