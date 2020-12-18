using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _PCCanvas;
    [SerializeField] private GameObject _MobileCanvas;
    [SerializeField] private GameObject[] _basePanel = new GameObject[2];
    [SerializeField] private GameObject[] _optionsPanel = new GameObject[2];

    
    [SerializeField] private Button[] _playButton = new Button[2];
    [SerializeField] private Button[] _creditsButton = new Button[2];
    [SerializeField] private Button[] _backButton = new Button[2];

    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private int _sceneToLoad = 1;
    private GameObject _lastSelectedObject;
    private GameObject _currentSelectedObject;

    //Android Version uses index 1, PC uses 0
    private RuntimePlatform currentPlatform;

    // Start is called before the first frame update
    private void Start()
    {
        DeterminPlatform();
        if (currentPlatform == RuntimePlatform.Android) { DisplayMobileCanvas(); }
        else { DisplayPCCanvas(); }
        Debug.Log(currentPlatform);
        Debug.Log(UnityEditor.EditorUserBuildSettings.activeBuildTarget);
        _currentSelectedObject = _eventSystem.currentSelectedGameObject;
        _lastSelectedObject = _currentSelectedObject;
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

    void DeterminPlatform()
    {
        currentPlatform = Application.platform;
#if UNITY_EDITOR
        if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.Android)
            currentPlatform = RuntimePlatform.Android;
        else
            currentPlatform = RuntimePlatform.WindowsPlayer;
#endif
    }
    void DisplayPCCanvas()
    {
        _PCCanvas.SetActive(true);
        _MobileCanvas.SetActive(false);
        _optionsPanel[0].SetActive(false);
        _basePanel[0].SetActive(true);
        _playButton[0].Select();
    }
    void DisplayMobileCanvas()
    {
        _PCCanvas.SetActive(false);
        _MobileCanvas.SetActive(true);
        _optionsPanel[1].SetActive(false);
        _basePanel[1].SetActive(true);
        _playButton[1].Select();
    }
    public void Play()
    {
        AudioManager.instance.Play("InterfaceSelected");
        StartCoroutine(WaitAndLaunchGame());
    }

    private IEnumerator WaitAndLaunchGame()
    {
        yield return new WaitForSeconds(AudioManager.instance.GetClipLength("InterfaceSelected"));
        SceneManager.LoadScene(_sceneToLoad);
    }

    public void GoToBasePanel()
    {
        if(currentPlatform == RuntimePlatform.Android)
        {
            _basePanel[1].SetActive(true);
            _optionsPanel[1].SetActive(false);
            _backButton[1].GetComponent<ShowText>().ResetRotation();
        }
        else
        {
            _basePanel[0].SetActive(true);
            _optionsPanel[0].SetActive(false);
            _backButton[0].GetComponent<ShowText>().ResetRotation();
            _lastSelectedObject.GetComponent<Button>().Select();
        }
    }

    public void Options()
    {
        if (currentPlatform == RuntimePlatform.Android)
        {
            _basePanel[1].SetActive(false);
            _optionsPanel[1].SetActive(true);
            _creditsButton[1].GetComponent<ShowText>().ResetRotation();
        }
        else
        {
            _basePanel[0].SetActive(false);
            _optionsPanel[0].SetActive(true);
            _creditsButton[0].GetComponent<ShowText>().ResetRotation();
            _backButton[0].Select();
        }
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
