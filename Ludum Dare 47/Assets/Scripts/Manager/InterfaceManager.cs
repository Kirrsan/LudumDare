﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InterfaceManager : MonoBehaviour
{

    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _loosePanel;

    [SerializeField] private Button _playButton;
    [SerializeField] private EventSystem _eventSystem;
    private GameObject _lastSelectedObject;


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
            else if (GameManager.instance.state == State.LOSE)
            {
                GoToLoose();
            }
        };
        GoToGame();
    }

    // Update is called once per frame
    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            _eventSystem.SetSelectedGameObject(_lastSelectedObject);
        }
        else
        {
            _lastSelectedObject = _eventSystem.currentSelectedGameObject;
        }
    }

    private void GoInpause()
    {
        Time.timeScale = 0;
        _gamePanel.SetActive(false);
        _pausePanel.SetActive(true);
        _playButton.Select();
    }

    private void GoToGame()
    {
        _pausePanel.SetActive(false);
        _winPanel.SetActive(false);
        _loosePanel.SetActive(false);
        _gamePanel.SetActive(true);
        Time.timeScale = 1;
    }

    private void GoToWin()
    {
        _gamePanel.SetActive(false);
        _pausePanel.SetActive(false);
        _winPanel.SetActive(true);
    }

    private void GoToLoose()
    {
        Time.timeScale = 0;
        _gamePanel.SetActive(false);
        _pausePanel.SetActive(false);
        _loosePanel.SetActive(true);
    }

    #region Button Functions

    public void Resume()
    {
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
