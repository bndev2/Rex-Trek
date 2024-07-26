using MyAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattler : Battler, IHasGun
{
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _playerTurnStart;

    [SerializeField] private List<Gun> _guns;
    [SerializeField] private Gun _gun;

    [SerializeField] private DamageFlash _damageFlash;
    [SerializeField] private AudioClip _sfxDamage;

    public override void OnWin()
    {
        _gun?.gameObject.SetActive(false);
    }

    public override void OnPlayerTurnStart()
    {
        _audioSource?.PlayOneShot(_playerTurnStart);
        _canAttack = true;

        if (_currentTarget == null)
        {
            //_currentTarget = _manager.battlers.Peek();
        }
    }

    public override void Attack(Battler battler)
    {
        if (_manager?.currentBattler != this)
        {
            return;
        }

        StartCoroutine(HandleShoot(battler));
    }

    public override void AttackCurrent()
    {
        if (!_canAttack || _manager?.currentBattler != this || _currentTarget == null)
        {
            return;
        }

        _canAttack = false;

        StartCoroutine(HandleShoot(_currentTarget));
    }

    public IEnumerator HandleShoot(Battler target)
    {

        // gun specific behaviour eg firing
        yield return StartCoroutine(_gun.FireBarrage(this, target, dmgMultiplier: 4));

        // Tell the battle manager
        _manager.OnMoveExecution(this, _currentTarget);
    }

    public override void Run()
    {
        // Player specific behaviour eg animation

        // Coroutine

        // Switch to the regular overworld
        GameManager.Instance.ChangeState(LevelState.Overworld);
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

    protected override void UpdateUI()
    {

    }

    public void ShootAtTarget(Battler battler)
    {
        if (_manager?.currentBattler != this)
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

    public void ShootAtCurrentTarget()
    {
        if (_manager?.currentBattler != this)
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
        _damageFlash?.PlayFlash();

        SoundFXManager.instance.PlaySoundAtTransform(_sfxDamage, transform);

        float actualDamage = damage - stats.level * 2;

        actualDamage = actualDamage + Random.Range(.1f, 1);

        _stats.SetHealth(_stats.health - actualDamage);
    }

    public void SwitchToNextTarget()
    {
        if (_manager?.currentBattler != this)
        {
            return;
        }

        _currentTarget = _manager.GetBattlerAdjacent(_currentTarget, true);

        Debug.Log(_currentTarget.stats.id);
    }

    public void SwitchToPreviousTarget()
    {
        if (_manager?.currentBattler != this)
        {
            return;
        }

        _currentTarget = _manager.GetBattlerAdjacent(_currentTarget, false);

        Debug.Log(_currentTarget.stats.id);
    }

    public void SwitchToNextGun()
    {
        if(_canAttack == false)
        {
            return;
        }

        int _currentGunIndex = _guns.IndexOf(_gun);

        if (_currentGunIndex == _guns.Count - 1)
        {
            _gun.Unequip();

            _gun = _guns[0];

            _gun.Equip();

            return;
        }

        _gun.Unequip();

        _gun = _guns[_currentGunIndex + 1];

        _gun.Equip();
    }

    void Start()
    {
        _currentTarget = _manager?.enemy;
    }
}
