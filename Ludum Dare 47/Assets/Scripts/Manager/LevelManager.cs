using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public static LevelManager instance;

    [SerializeField] private Worlds[] worlds;

    private int currentWorld;

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
