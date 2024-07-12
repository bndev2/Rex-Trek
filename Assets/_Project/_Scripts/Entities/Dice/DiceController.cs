using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DiceController : MonoBehaviour
{
    private Dice_Manager _diceManager;

    Rigidbody _rb;

    private bool _isDetermining = false;

    private float _stillTimer = 0;

    [SerializeField] private int _finalValue;
    public int finalValue
    {
        get
        {
            return _finalValue;
        }
    }

    [SerializeField] private int _sideUpValue;

    public void Initialize(Dice_Manager manager)
    {
        _diceManager = manager;
        _isDetermining = true;
    }

    public int sideUpValue
    {
        get
        {
            return _sideUpValue;
        }
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        transform.rotation = Quaternion.Euler(new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360)));
        _rb.velocity = new Vector3(0, 0, UnityEngine.Random.Range(8, 15));
    }


    private void HandleSideUpDetermination()
    {
        if (_isDetermining)
        {
            Vector3 up = Vector3.up;

            float highestDot = -Mathf.Infinity;
            int side = 0;

            for (int i = 1; i <= 6; i++)
            {
                Vector3 diceFaceDirection = transform.TransformDirection(GetDiceFaceNormal(i));
                float dot = Vector3.Dot(up, diceFaceDirection);

                if (dot > highestDot)
                {
                    highestDot = dot;
                    side = i;
                }
            }

            _sideUpValue = side;

            if(_rb.velocity == Vector3.zero)
            {
                _stillTimer += Time.deltaTime;
            }
            else
            {
                _stillTimer = 0;
            }

            if (_stillTimer > .2f)
            {
                _finalValue = _sideUpValue;
                _isDetermining = false;
                _diceManager.ReportRoll(_finalValue);
            }

        }
    }

    private Vector3 GetDiceFaceNormal(int faceIndex)
    {
        switch (faceIndex)
        {
            case 1: return new Vector3(0, 0, 1); // 1 is opposite to 6
            case 2: return new Vector3(0, 0, -1); // 2 is opposite to 5
            case 3: return new Vector3(-1, 0, 0); // 3 is opposite to 4
            case 4: return new Vector3(0, -1, 0);  // 4 is opposite to 3
            case 5: return new Vector3(1, 0, 0);  // 5 is opposite to 2
            case 6: return new Vector3(0, 1, 0);  // 6 is opposite to 1
            default: return Vector3.zero;
        }
    }

    private void Update()
    {
        HandleSideUpDetermination();
    }
}
