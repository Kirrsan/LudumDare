﻿using UnityEngine;

public class LevelManager : MonoBehaviour {
    public static LevelManager instance;

    [SerializeField] private GameObject[] worlds;

    [SerializeField] int currentWorld = 0;

    public int CurrentWorld => currentWorld;

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
            worlds[i].SetActive(i == currentWorld);
        }
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
}
