using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    [SerializeField] public UnityEvent onTurnStart;

    public void Initialize(CharacterStats stats, BattleManager manager)
    {
        //if(stats != null)
        //{
            //Debug.LogError("Battler has already been initialized!");
            //return;
        //}

        _stats = stats;
        _manager = manager;
        UpdateUI();
    }

    public abstract void OnPlayerTurnStart();

    public abstract void Attack(Battler battler);

    public abstract void AttackCurrent();

    public abstract void Run();

    public abstract void Damage(float damageAmount);

    public void ChangeTarget(Battler battler)
    {
        _currentTarget = battler;
    }

    protected abstract void UpdateUI();

    private void Awake()
    {
        onTurnStart.AddListener(OnPlayerTurnStart);
    }
}

public interface IHasGun
{
    void Shoot(Battler battler);
    void Shoot();
}

public class Attack
{
    public GameObject _vfxGO;
    public AudioClip _audioClip;
    public Vector2 damageRange = new Vector2(0, 0);
    public int hitCount = 1;
    public int pp = 20;
    public int maxPP = 20;

    public void Apply(Battler origin, Battler target)
    {
        // Calculate total damage
        int totalDamage = 0;
        for (int i = 0; i < hitCount; i++)
        {
            int damage = Random.Range((int)damageRange.x, (int)damageRange.y + 1);
            totalDamage += damage;
        }

        // Apply damage to the target
        target.Damage(totalDamage);

        // Play visual effects
        if (_vfxGO != null)
        {
            GameObject vfx = GameObject.Instantiate(_vfxGO, target.transform.position, Quaternion.identity);
            GameObject.Destroy(vfx, 1.0f); // Destroy the VFX after 1 second
        }

        // Play audio clip
        if (_audioClip != null)
        {
            AudioSource.PlayClipAtPoint(_audioClip, target.transform.position);
        }
    }
}
