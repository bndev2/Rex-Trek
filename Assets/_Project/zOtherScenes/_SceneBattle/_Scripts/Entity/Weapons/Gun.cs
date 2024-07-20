using MyAssets;
using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private int _damage = 5;
    [SerializeField] private int _fireAmount = 3;
    [SerializeField] private float _timeBetweenShots = .5f;


    [SerializeField] private GameObject _vfxMuzzleFlash;
    [SerializeField] private AudioClip _sfxGunShot;

    [SerializeField] Transform _bulletSpawnLocation;

    public IEnumerator FireBarrage(Battler origin, Battler target, float dmgMultiplier = 1)
    {
        yield return StartCoroutine(HandleFiring(origin, target, dmgMultiplier));
    }

    public void FireSingle(Battler origin, Battler target, float dmgMultiplier = 1)
    {
        SoundFXManager.instance.PlaySoundAtTransform(_sfxGunShot, transform);

        // Instantiate the VFX and store it in a variable
        GameObject muzzleFlash = Instantiate(_vfxMuzzleFlash, _bulletSpawnLocation.transform.position, _bulletSpawnLocation.transform.rotation);

        // Destroy the VFX after 1 second
        Destroy(muzzleFlash, 1f);

        target.Damage(_damage * dmgMultiplier);
    }


    private IEnumerator HandleFiring(Battler origin, Battler target, float dmgMultiplier)
    {
        for (int i = 0; i < _fireAmount; i++)
        {
            yield return new WaitForSeconds(_timeBetweenShots);
            FireSingle(origin, target, dmgMultiplier);
        }
    }
}
