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
    [SerializeField] private Button _backButton;

    [SerializeField] private EventSystem _eventSystem;

    private GameObject _lastSelectedObject;
    private GameObject _currentSelectedObject;

    // Start is called before the first frame update
    private void Start()
    {
        GoToBasePanel();
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            _eventSystem.SetSelectedGameObject(_lastSelectedObject);
            _currentSelectedObject = _eventSystem.currentSelectedGameObject;
        }
        else if(_currentSelectedObject != _eventSystem.currentSelectedGameObject)
        {
            _lastSelectedObject = _currentSelectedObject;
            _currentSelectedObject = _eventSystem.currentSelectedGameObject;
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
