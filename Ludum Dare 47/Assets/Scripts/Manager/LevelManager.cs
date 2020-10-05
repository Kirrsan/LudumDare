using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static LevelManager instance;

    [SerializeField] private Worlds[] worlds;
    public System.Action onReset;

    private int currentWorld;

    int sectionCount = 1;
    PlayerController playerController;
    public int CurrentWorld => currentWorld;
    public int WorldCount => worlds.Length;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        UpdateWorlds();
        playerController = FindObjectOfType<PlayerController>();

    }

    private void UpdateWorlds()
    {

        for (var i = 0; i < worlds.Length; i++)
        {
            worlds[i].gameObjects.SetActive(i == currentWorld);

        }



        foreach (var portal in FindObjectsOfType<Portal>())
            portal.SetIndex(currentWorld);

        SwitchSkybox();
    }

    public void ActivateNextWorld()
    {
        sectionCount++;

        if ((sectionCount % 2) == 1)
        {
            Debug.Log(playerController.gameObject.transform.position);
            playerController.respawnPosition.z = playerController.gameObject.transform.position.z + 1f;
        }
        currentWorld++;
        currentWorld %= worlds.Length;
        UpdateWorlds();
    }

    public void Reset()
    {
        currentWorld = 0;
        UpdateWorlds();
    }

    private void SwitchSkybox()
    {
        RenderSettings.skybox = worlds[currentWorld].skybox;
        RenderSettings.fogColor = worlds[currentWorld].color;
    }
}

[System.Serializable]
public struct Worlds
{
    public Material skybox;
    public Color color;
    public GameObject gameObjects;
}
