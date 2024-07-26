using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayerInputManager : MonoBehaviour
{
   [SerializeField] private PlayerBattler _player;

    public void Initialize(PlayerBattler player)
    {
        _player = player;
    }

    // Link up button to this
    public void AttackCurrentTarget()
    {
        _player.AttackCurrent();
    }

    public void Run()
    {
        _player.Run();
    }

    public void ShootCurrent()
    {
        _player.Shoot();
    }

    public void SwitchToNextTarget()
    {
        _player.SwitchToNextTarget();
    }

    public void SwitchToPreviousTarget()
    {
        _player.SwitchToPreviousTarget();
    }

    public void SwitchToNextGun()
    {
        _player.SwitchToNextGun();
    }
}
