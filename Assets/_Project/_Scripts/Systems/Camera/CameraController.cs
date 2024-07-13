using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _currentTarget;
    [SerializeField] private float _smoothSpeed = 0.125f;
    [SerializeField] private Vector3 _offset = new Vector3(0, 4.705406f, -3.07099f);

    public void SwitchTarget(Transform target)
    {
        _currentTarget = target;
    }

    private void LateUpdate()
    {
        if (_currentTarget == null)
            return;

        Vector3 desiredPosition = _currentTarget.position + _offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);
        transform.position = smoothedPosition;
    }
}
