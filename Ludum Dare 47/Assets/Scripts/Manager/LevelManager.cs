using UnityEngine;

public class LevelManager : MonoBehaviour {

    public static LevelManager instance;

    [SerializeField] private Worlds[] worlds;
    public System.Action onReset;

    private int currentWorld;

    public int CurrentWorld => currentWorld;
    public int WorldCount => worlds.Length;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start() {
        UpdateWorlds();
    }

    private void UpdateWorlds() {
        for (var i = 0; i < worlds.Length; i++) {
            worlds[i].gameObjects.SetActive(i == currentWorld);
        }

        foreach (var portal in FindObjectsOfType<Portal>())
            portal.SetIndex(currentWorld);

        SwitchSkybox();
    }

    public void ActivateNextWorld() {
        currentWorld++;
        currentWorld %= worlds.Length;
        UpdateWorlds();
    }

    public void Reset() {
        currentWorld = 0;
        UpdateWorlds();
    }

    private void SwitchSkybox() {
        RenderSettings.skybox = worlds[currentWorld].skybox;
        RenderSettings.fogColor = worlds[currentWorld].color;
    }
}

[System.Serializable]
public struct Worlds {
    public Material skybox;
    public Color color;
    public GameObject gameObjects;
}
