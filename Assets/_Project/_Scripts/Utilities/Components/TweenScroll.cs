using UnityEngine;
using UnityEngine.UI;

public class TweenScroll : MonoBehaviour
{
    public Image _image; // Your UI Image

    public Vector3 targetPosition; // Target position to move to
    public float speed = 50; // Speed in units per second
    public float delayInSeconds = 1; // Delay before moving back

    private Vector3 _originalPosition;
    private LTDescr _tweenObject;

    private void Awake()
    {
        _originalPosition = transform.localPosition;
    }

    void Start()
    {

    }

    void MoveBackToOriginalPosition()
    {
        float timeInSeconds = Vector3.Distance(_originalPosition, gameObject.transform.localPosition) / speed;
        _tweenObject = LeanTween.moveLocal(gameObject, _originalPosition, timeInSeconds).setDelay(delayInSeconds);
        LeanTween.alpha(_image.rectTransform, 0, timeInSeconds).setDelay(delayInSeconds); // Fade out after delay
    }

    public void PlayAnimation()
    {
        if (_tweenObject != null)
        {
            LeanTween.cancel(gameObject, _tweenObject.id);
            gameObject.transform.localPosition = _originalPosition;
        }

        LeanTween.alpha(_image.rectTransform, 0, 0); // Immediately set alpha to 0 (invisible)
        float timeInSeconds = Vector3.Distance(targetPosition, _originalPosition) / speed;
        _tweenObject = LeanTween.moveLocal(gameObject, targetPosition, timeInSeconds).setOnComplete(MoveBackToOriginalPosition);
        LeanTween.alpha(_image.rectTransform, 1, timeInSeconds); // Fade in over the duration of the animation
    }
}
