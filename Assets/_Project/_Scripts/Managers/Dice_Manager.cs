using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Dice_Manager : MonoBehaviour
{
    [SerializeField] TurnManager _turnManager;

    // Static singleton property
    public static Dice_Manager Instance { get; private set; }

    private bool _isRolling = false;

    [SerializeField] private UnityEvent _onDiceStart;
    [SerializeField] private UnityEvent _onDiceFinish;
    public bool isRolling
    {
        get
        {
            return _isRolling;
        }
    }
    [SerializeField] private GameObject _dicePrefab;
    private List<GameObject> _diceRefs = new List<GameObject>();

    [SerializeField] private int _diceCount = 1;
    private int _finishedCount = 0;

    private int _totalValue;

    public int finalNumber = -1;

    public void Roll()
    {
        StartCoroutine(SpawnDice());
    }

    IEnumerator SpawnDice()
    {
        if (_isRolling)
        {
            Debug.LogError("Cannot start a roll when dice are already rolling!");
            yield break;
        }

        _onDiceStart.Invoke();

        _isRolling = true;

        for (int i = 0; i < _diceCount; i++)
        {
            GameObject diceRef = Instantiate(_dicePrefab, transform.position + Vector3.right * Random.Range(1, 5), Quaternion.identity);

            diceRef.GetComponent<DiceController>().Initialize(this);

            _diceRefs.Add(diceRef);

            yield return new WaitForSeconds(.4f);
        }
    }


    public void ReportRoll(int rollNumber)
    {
        _totalValue += rollNumber;

        _finishedCount += 1;

        if (_finishedCount == _diceCount)
        {
            foreach (GameObject diceRef in _diceRefs)
            {
                Destroy(diceRef.gameObject);
            }

            finalNumber = _totalValue;

            _totalValue = 0;
            _finishedCount = 0;

            _onDiceFinish.Invoke();

            Debug.Log("Final number is " + finalNumber.ToString());
            _isRolling = false;

            _turnManager.MoveAdditiveCurrent(finalNumber);
        }
    }

    private void Awake()
    {
        // If the singleton hasn't been initialized yet
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

    }
}
