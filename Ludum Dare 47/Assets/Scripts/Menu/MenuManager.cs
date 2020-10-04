using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

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

    [SerializeField] private EventSystem _eventSystem;

    private GameObject _lastSelectedObject;

    // Start is called before the first frame update
    private void Start()
    {
        _playButtonAnimator = _playButton.GetComponent<Animator>();
        _creditsButtonAnimator = _creditsButton.GetComponent<Animator>();
        GoToBasePanel();
    }

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

    public void Play()
    {
        SceneManager.LoadScene(_sceneToLoad);
    }

    public void GoToBasePanel()
    {
        _basePanel.SetActive(true);
        _optionsPanel.SetActive(false);
        _playButton.Select();
    }

    public void Options()
    {
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
