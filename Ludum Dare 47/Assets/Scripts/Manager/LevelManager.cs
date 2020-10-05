using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {
    public static LevelManager instance;

    [SerializeField] private Worlds[] worlds;
    [SerializeField] private Portal[] portals;

    private int currentWorld;
    private int currentPortalIndex;

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
        ChangePortal(true, 0);
    }

    private void UpdateWorlds() {
        for (var i = 0; i < worlds.Length; i++) {
            worlds[i].gameObjects.SetActive(i == currentWorld);
        }

        SwitchSkybox();
    }

    public void ActivateNextWorld() {
        currentWorld++;
        currentWorld %= worlds.Length;
        ChangePortal(false, currentPortalIndex);
        currentPortalIndex++;
        UpdateWorlds();
        ChangePortal(true, currentPortalIndex);
    }

    private void ChangePortal(bool value, int currentPortal) {
        if (currentPortal >= portals.Length) {
            Debug.LogError("Tried to access a portal above the list of portals, returning");
            return;
        }

        portals[currentPortal].portalVFX[currentWorld].SetActive(value);
    }

    public void Reset() {
        ChangePortal(false, currentPortalIndex);
        currentWorld = 0;
        currentPortalIndex = 0;
        ChangePortal(true, currentPortalIndex);
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
