using MyAssets;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class PlayerController : BoardPawn
{
    [SerializeField] AudioClip _sfxDamage;

    [SerializeField] private Animator _animator;

    [SerializeField] private Menus_Manager _menusManager;

    private CharacterStats _characterStats;
    public CharacterStats characterStats
    {
        get { return _characterStats; }
    }

    private bool _isPaused = false;

    [SerializeField] private UnityEvent _onTakeDamage;
    [SerializeField] private UnityEvent _onTakeTurns;
    [SerializeField] private UnityEvent _onMoveBack;


    public override bool SetMove(List<Vector3> path, SquareController squareController)
    {
        base.SetMove(path, squareController);

        _path = new List<Vector3>();

        _path = path;
        ChangeState(PawnState.Moving);
        _currentMoveIndex = 0; // Reset the move index when a new path is set
        _onMoveStart.Invoke();

        _animator.CrossFade("Walk", 0.05f);

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
                _animator.StopPlayback();
                break;
            case PawnState.Moving:
                _animator.StopPlayback();
                break;
            case PawnState.Disabled:
                break;
        }

        _currentState = pawnState;

        switch (pawnState)
        {
            case PawnState.Idle:
                _animator.CrossFade("Idle", 0.05f);
                break;
            case PawnState.Moving:
                _animator.CrossFade("Walk", 0.05f);
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
        _onTakeDamage.Invoke();

        GameManager.Instance.DecreasePlayerHealth(this.id, damageAmount);

        //float currentHealth = _entityStats.currentHealth;

        //_entityStats.SetCurrentHealth(Mathf.Clamp(currentHealth - damageAmount, 0, _entityStats.maxHealth));

        SoundFXManager.instance.PlaySoundAtTransform(_sfxDamage, transform);

        UpdateUI();
    }

    public void GiveHealth(float healthToGive)
    {
        GameManager.Instance.IncreasePlayerHealth(this.id, healthToGive);

        //float currentHealth = _entityStats.currentHealth;

        //_entityStats.SetCurrentHealth(Mathf.Clamp(currentHealth + healthToGive, 0, _entityStats.maxHealth));

        SoundFXManager.instance.PlaySoundAtTransform(_sfxDamage, transform);

        UpdateUI();
    }

    public void GiveExperience(float experienceToGive)
    {
        GameManager.Instance.IncreasePlayerExperience(this.id, experienceToGive);
    }

    public void MoveSpaces(int spacesToMove)
    {
        Debug.Log("Heres some spaces");

        // If a coroutine is already running, stop it to delay the finishing of the turn
        if (_waitToFinishRoutine != null)
        {
            StopCoroutine(_waitToFinishRoutine);
            _waitToFinishRoutine = null;
        }

        _turnManager.MoveAdditiveOverride(this, spacesToMove);
    }


    public void GiveTurns(int turnsToAdd)
    {
        Debug.Log("Heres some turns");
        _turnManager.IncreaseTurns(this, turnsToAdd);
    }

    public void Engage(CharacterStats opponentStats)
    {
        GameManager.Instance.EnterBattle(this._characterStats, opponentStats);
    }

    private void UpdateUI()
    {
        if (_menusManager != null)
        {
            if (id == "Player 1")
            {
                _menusManager.UpdateHealthUI(GameManager.Instance.GetPlayerStats(this.id).scaledHealth, true);
            }
            else if (id == "Player 2")
            {
                _menusManager.UpdateHealthUI(GameManager.Instance.GetPlayerStats(this.id).scaledHealth, false);
            }
        }
    }

    public override void PauseMove()
    {
        base.PauseMove();

        _isPaused = true;
    }

    public override void ResumeMove()
    {
        // Add a new method to resume the Rex's movement
        _isPaused = false;
    }


    private void Initialize()
    {
        _characterStats = GameManager.Instance.CreatePlayerStats(this.id);

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
