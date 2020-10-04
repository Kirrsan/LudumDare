using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private GameObject _basePanel;
    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] private int _sceneToLoad = 1;
    
    [SerializeField] private Button _playButton;
    private Animator _playButtonAnimator;
    [SerializeField] private Button _creditsButton;
    private Animator _creditsButtonAnimator;
    [SerializeField] private Button _backButton;

    // Start is called before the first frame update
    private void Start()
    {
        _playButtonAnimator = _playButton.GetComponent<Animator>();
        _creditsButtonAnimator = _creditsButton.GetComponent<Animator>();
        GoToBasePanel();
    }

    public void Play()
    {
        SceneManager.LoadScene(_sceneToLoad);
    }

    public void GoToBasePanel()
    {
        _basePanel.SetActive(true);
        _optionsPanel.SetActive(false);
        _playButton.Select();
        _playButtonAnimator.SetTrigger("Selected");
    }

    public void Options()
    {
        _creditsButtonAnimator.SetTrigger("Normal");
        _basePanel.SetActive(false);
        _optionsPanel.SetActive(true);
        _backButton.Select();
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
