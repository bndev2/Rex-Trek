using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UINotification : MonoBehaviour
{
    [SerializeField] private TweenScroll _tweenScroll;
    [SerializeField] private TextMeshProUGUI _text;

    private void Start()
    {

    }

    public void Play(string message)
    {
        _text.text = message;
        _tweenScroll.PlayAnimation();
    }
}