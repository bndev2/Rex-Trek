using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Add this if you're using TextMeshPro
using UnityEngine.Events; // Add this to use UnityEvent

public class MultiPageText : MonoBehaviour
{
    [SerializeField] GameObject _pagePrefab;
    [SerializeField] private List<GameObject> _pages = new List<GameObject>();
    private int _currentPageIndex = 0;

    public UnityEvent _onTextComplete;

    public void NextPage()
    {
        _pages[_currentPageIndex].SetActive(false);

        _currentPageIndex++;

        if (_currentPageIndex > _pages.Count - 1)
        {
            _onTextComplete.Invoke();
            GameManager.Instance.ChangeState(LevelState.Overworld);
            _currentPageIndex = _pages.Count - 1;
        }

        _pages[_currentPageIndex].SetActive(true);
    }

    public void PreviousPage()
    {
        _pages[_currentPageIndex].SetActive(false);

        _currentPageIndex--;

        if (_currentPageIndex < 0)
        {
            _currentPageIndex = 0;
        }

        _pages[_currentPageIndex].SetActive(true);
    }

    public void SetText(int pageIndex, string text)
    {
        _pages[pageIndex].transform.Find("Text (TMP)").GetComponent<TMPro.TextMeshProUGUI>().text = text; // Change this line
    }

    public void AddPage()
    {
        GameObject newPage = Instantiate(_pagePrefab, transform.position, transform.rotation);

        newPage.transform.parent = transform;

        _pages.Add(newPage);
    }

    private void Start()
    {

    }
}
