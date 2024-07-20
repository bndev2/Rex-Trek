using MyAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBattler : Battler
{
    [SerializeField] private Animator _animator;
    [SerializeField] private DamageFlash _damageFlash;
    [SerializeField] private AudioClip _sfxDamage;

    private IEnumerator HandleAttackCurrent()
    {
        Vector3 originalPosition = transform.position;
        Vector3 targetPosition = _currentTarget.transform.position;

        bool isPlayingWalk = false;

        // Move towards the target
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            if(!isPlayingWalk) { _animator.Play("Walk"); }
            
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 6);
            yield return null;
        }

        _animator.CrossFade("Attack", .05f);

        yield return new WaitForSeconds(.6f);

        _animator.CrossFade("Idle", .05f);

        _currentTarget.Damage(4);

        _manager.OnMoveExecution(this, _currentTarget);

        // Move back to the original position
        while (Vector3.Distance(transform.position, originalPosition) > 0.1f)
        {
            _animator.Play("Walk");
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, Time.deltaTime * 6);
            yield return null;
        }

        _animator.CrossFade("Idle", .05f);
    }

    public override void OnWin()
    {

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
        if (_damageFlash != null)
        {
            _damageFlash.PlayFlash();
        }

        if (_sfxDamage != null)
        {
            SoundFXManager.instance.PlaySoundAtTransform(_sfxDamage, transform);
        }

        float actualDamage = damage - stats.level * 2;

        _stats.SetHealth(_stats.health - actualDamage);

        Debug.Log("Youch that hurt! " + stats.health.ToString());
    }
}