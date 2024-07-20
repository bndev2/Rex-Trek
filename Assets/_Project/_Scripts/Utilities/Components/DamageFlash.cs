using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private int[] _materialIndexes;
    private List<Color> _originalColors = new List<Color>();
    [SerializeField] private float _flashTime;

    private Coroutine _flashCoroutine;

    [SerializeField] private Color _flashColor = Color.red;

    private void Start()
    {
        foreach (int index in _materialIndexes)
        {
            _originalColors.Add(_renderer.materials[index].color);
        }
    }

    private void StartFlash()
    {
        foreach (int index in _materialIndexes)
        {
            _renderer.materials[index].color = _flashColor;
        }
    }

    private void StopFlash()
    {
        for (int i = 0; i < _materialIndexes.Length; i++)
        {
            _renderer.materials[_materialIndexes[i]].color = _originalColors[i];
        }
    }

    private IEnumerator FlashRoutine()
    {
        StartFlash();
        yield return new WaitForSeconds(_flashTime);
        StopFlash();
    }

    public void PlayFlash()
    {
        if (_flashCoroutine != null)
        {
            StopCoroutine(_flashCoroutine);
        }
        _flashCoroutine = StartCoroutine(FlashRoutine());
    }
}
