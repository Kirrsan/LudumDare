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

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
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
        ChangePortal(false, currentPortalIndex);
        currentPortalIndex++;
        currentWorld %= worlds.Length;
        UpdateWorlds();
        ChangePortal(true, currentPortalIndex);
    }

    private void ChangePortal(bool value, int currentPortal)
    {
        if (currentPortal >= portals.Length)
        {
            Debug.LogError("Tried to access a portal above the list of portals, returning");
            return;
        }
        portals[currentPortal].portalVFX[currentWorld].SetActive(value);
    }

    public void Reset()
    {
        currentWorld = 0;
        ChangePortal(false, currentPortalIndex);
        currentPortalIndex = 0;
        ChangePortal(true, currentPortalIndex);
        UpdateWorlds();
    }

    private void SwitchSkybox()
    {
        RenderSettings.skybox = worlds[currentWorld].skybox;
        RenderSettings.fogColor = worlds[currentWorld].color;
    }

    public int GetWorldArrayLength()
    {
        return worlds.Length;
    }
}

[System.Serializable]
public struct Worlds
{
    public Material skybox;
    public Color color;
    public GameObject gameObjects;
}
