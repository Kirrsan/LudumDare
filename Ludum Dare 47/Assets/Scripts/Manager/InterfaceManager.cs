using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceManager : MonoBehaviour
{

    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject loosePanel;

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
        };
        GoToGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GoInpause()
    {
        gamePanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    private void GoToGame()
    {
        pausePanel.SetActive(false);
        winPanel.SetActive(false);
        loosePanel.SetActive(false);
        gamePanel.SetActive(true);
    }
}
