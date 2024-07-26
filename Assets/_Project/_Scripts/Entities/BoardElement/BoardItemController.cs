using MyAssets;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class BoardItemController : MonoBehaviour
{
    [SerializeField] private GameObject _boardElementGo;

    private void OnValidate()
    {

    }


    private void Awake()
    {
        if (_boardElementGo != null)
        {
            foreach (var comp in _boardElementGo.GetComponents<IBoardElement>())
            {
                _boardElement = comp.Clone();
                break;
            }
        }
    }




    private IBoardElement _boardElement;
    [SerializeField] private GameObject _vfxApplyEffect;
    [SerializeField] AudioClip _sfxApplyEffect;
    [SerializeField] private Animator _animator;

    // Add list of effects

    public void ApplyEffect(PlayerController playerController)
    {
        GameObject vfxGO = Instantiate(_vfxApplyEffect, transform.position, Quaternion.identity);

        SoundFXManager.instance.PlaySoundAtTransform(_sfxApplyEffect, transform);

        _boardElement.Apply(playerController);

        _animator.Play("PotionApply");

            // Get the length of the current animation clip
            float animationLength = _animator.GetCurrentAnimatorStateInfo(0).length;

            StartCoroutine(DestroyAfterSeconds(this.gameObject, animationLength));
    }

    // Coroutine to destroy the object after a delay
    IEnumerator DestroyAfterSeconds(GameObject gameObject, float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Now destroy the object
        Destroy(this.gameObject);

        Debug.Log("destroying");
    }

    public void SetElement(BoardElement element)
    {

    }

    private void Start()
    {

    }
}
