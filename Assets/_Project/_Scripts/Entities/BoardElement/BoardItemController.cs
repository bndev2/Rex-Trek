using MyAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardItemController : MonoBehaviour
{
    [SerializeField] private BoardElement _boardElement;
    [SerializeField] private GameObject _vfxApplyEffect;
    [SerializeField] AudioClip _sfxApplyEffect;

    public void ApplyEffect(PlayerController playerController)
    {
        // Do some particles and sfx
        Instantiate(_sfxApplyEffect, transform.position, Quaternion.identity);

        SoundFXManager.instance.PlaySoundAtTransform(_sfxApplyEffect, transform);

        _boardElement.Apply(playerController);

        Destroy(this.gameObject);
    }
}
