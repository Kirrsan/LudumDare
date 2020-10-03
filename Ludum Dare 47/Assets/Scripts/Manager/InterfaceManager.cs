using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InterfaceManager : MonoBehaviour
{

    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _loosePanel;

    // Start is called before the first frame update
    void Start()
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
            else if (GameManager.instance.state == State.LOOSE)
            {
                GoToLoose();
            }
        };
        GoToGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GoInpause()
    {
        _gamePanel.SetActive(false);
        _pausePanel.SetActive(true);
    }

    private void GoToGame()
    {
        _pausePanel.SetActive(false);
        _winPanel.SetActive(false);
        _loosePanel.SetActive(false);
        _gamePanel.SetActive(true);
    }

    private void GoToWin()
    {
        _gamePanel.SetActive(false);
        _pausePanel.SetActive(false);
        _winPanel.SetActive(true);
    }

    private void GoToLoose()
    {
        _gamePanel.SetActive(false);
        _pausePanel.SetActive(false);
        _loosePanel.SetActive(true);
    }

    #region Button Functions

    public void Resume()
    {
        GameManager.instance.ChangeState(State.INGAME);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    #endregion
}
