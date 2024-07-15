using MyAssets;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : BoardPawn
{
    [SerializeField] AudioClip _sfxDamage;

    [SerializeField] private Animator _animator;
    [SerializeField] private EntityStats _entityStats;

    [SerializeField] private Menus_Manager _menusManager;


    public override bool SetMove(List<Vector3> path, SquareController squareController)
    {
        base.SetMove(path, squareController);

        _path = new List<Vector3>();

        _path = path;
        ChangeState(PawnState.Moving);
        _currentMoveIndex = 0; // Reset the move index when a new path is set
        _onMoveStart.Invoke();

        return true;
    }

    // Handle moving along the vector3 points
    private void HandleMove()
    {
        if (_currentState == PawnState.Moving)
        {
            // Check if _currentMoveIndex is within the bounds of the _path list
            if (_currentMoveIndex < _path.Count)
            {
                // Move towards the point 
                transform.position = Vector3.MoveTowards(transform.position, _path[_currentMoveIndex], _moveSpeed * Time.deltaTime);

                // Rotate the player to face the next point
                Vector3 direction = _path[_currentMoveIndex] - transform.position;
                if (direction != Vector3.zero) // Avoid LookRotation creating errors when direction is zero
                {
                    Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
                    transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, _moveSpeed * Time.deltaTime);
                }

                // when point is reached approximately set the position to the point and start moving to next
                if (Vector3.Distance(transform.position, _path[_currentMoveIndex]) < 0.01f)
                {
                    transform.position = _path[_currentMoveIndex];
                    _currentMoveIndex += 1;
                    if (_currentMoveIndex >= _path.Count) // Check if we've reached the end of the path
                    {
                        _onMoveEnd.Invoke();
                        transform.position = _path[_path.Count - 1];
                        ChangeState(PawnState.Idle);
                        _path.Clear();
                        FinishTurn();
                    }
                }
            }
        }
    }



    private void ChangeState(PawnState pawnState)
    {
        switch (pawnState)
        {
            case PawnState.Idle:
                break;
            case PawnState.Moving:
                break;
            case PawnState.Disabled:
                break;
        }

        _currentState = pawnState;

        switch (pawnState)
        {
            case PawnState.Idle:
                _animator.Play("Idle");
                break;
            case PawnState.Moving:
                _animator.Play("Walk");
                break;
            case PawnState.Disabled:
                break;
        }
    }


    public override void FinishTurn()
    {
        base.FinishTurn();
    }

    public void DoDamage(float damageAmount)
    {
        float currentHealth = _entityStats.currentHealth;

        _entityStats.SetCurrentHealth(Mathf.Clamp(currentHealth - damageAmount, 0, _entityStats.maxHealth));

        SoundFXManager.instance.PlaySoundAtTransform(_sfxDamage, transform);

        UpdateUI();
    }

    public void GiveHealth(float healthToGive)
    {
        float currentHealth = _entityStats.currentHealth;

        _entityStats.SetCurrentHealth(Mathf.Clamp(currentHealth + healthToGive, 0, _entityStats.maxHealth));

        SoundFXManager.instance.PlaySoundAtTransform(_sfxDamage, transform);

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (_menusManager != null)
        {
            if (id == "Player 1")
            {
                _menusManager.UpdateHealthUI(_entityStats.currentScaledHealth, true);
            }
            else if (id == "Player 2")
            {
                _menusManager.UpdateHealthUI(_entityStats.currentScaledHealth, false);
            }
        }
    }

    private void Initialize()
    {
        UpdateUI();
    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        HandleMove();
    }
}
