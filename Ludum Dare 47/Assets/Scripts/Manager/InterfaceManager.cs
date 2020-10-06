using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class InterfaceManager : MonoBehaviour
{

    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _tutoPanel;

    [SerializeField] private Button _playButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _tutoButton;
    [SerializeField] private EventSystem _eventSystem;
    private GameObject _lastSelectedObject;
    private GameObject _currentSelectedObject;


    // Start is called before the first frame update
    private void Start()
    {
        GameManager.instance.onStateChange += () => {
            if (GameManager.instance.state == State.PAUSE)
            {
                GoInpause();
            }
            else if (GameManager.instance.state == State.INGAME)
            {
                GoToGame();
            }
            else if (GameManager.instance.state == State.WIN)
            {
                GoToWin();
            }
        };
        _eventSystem.SetSelectedGameObject(_playButton.gameObject);
        _currentSelectedObject = _eventSystem.currentSelectedGameObject;
        _lastSelectedObject = _currentSelectedObject;
        GoToGame();
    }

    // Update is called once per frame
    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            _eventSystem.SetSelectedGameObject(_lastSelectedObject);
            _currentSelectedObject = _eventSystem.currentSelectedGameObject;
        }
        else if (_currentSelectedObject != _eventSystem.currentSelectedGameObject)
        {
            _lastSelectedObject = _currentSelectedObject;
            _currentSelectedObject = _eventSystem.currentSelectedGameObject;
        }
    }

    public void GoInpause()
    {
        Time.timeScale = 0;
        _tutoPanel.SetActive(false);
        _gamePanel.SetActive(false);
        _pausePanel.SetActive(true);
        _lastSelectedObject.GetComponent<Button>().Select();
        _lastSelectedObject.GetComponent<Button>().GetComponent<Animator>().SetTrigger("Selected");
    }

    private void GoToGame()
    {
        _pausePanel.SetActive(false);
        _tutoPanel.SetActive(false);
        _winPanel.SetActive(false);
        _gamePanel.SetActive(true);
        Time.timeScale = 1;
    }

    private void GoToWin()
    {
        Time.timeScale = 0;
        _gamePanel.SetActive(false);
        _pausePanel.SetActive(false);
        _winPanel.SetActive(true);
    }

    public void GoToTuto()
    {
        _pausePanel.SetActive(false);
        _tutoPanel.SetActive(true);
        _backButton.Select();
    }

    #region Button Functions

    public void Resume()
    {
        _lastSelectedObject = _playButton.gameObject;
        GameManager.instance.ChangeState(State.INGAME);
        Time.timeScale = 1;
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    #endregion
}
