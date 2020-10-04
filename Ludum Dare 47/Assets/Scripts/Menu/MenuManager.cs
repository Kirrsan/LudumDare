using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private GameObject basePanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private int sceneToLoad = 1;

    // Start is called before the first frame update
    private void Start()
    {
        GoToBasePanel();
    }

    public void Play()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void GoToBasePanel()
    {
        basePanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void Options()
    {
        basePanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
