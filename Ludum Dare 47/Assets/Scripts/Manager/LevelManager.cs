using UnityEngine;

public class LevelManager : MonoBehaviour {
    [SerializeField] private GameObject[] worlds;

    private int currentWorld;

    public int CurrentWorld => currentWorld;

    private PlayerController player;

    /*private void Awake() {
        player = FindObjectOfType<PlayerController>();
    }*/

    private void Start() {
        UpdateWorlds();
    }

    /*private void Update() {
        if (currentWorld == 0 && player.transform.position.z > 55)
            ActivateNextWorld();
        else if (currentWorld == 1 && player.transform.position.z > 96) {
            ActivateNextWorld();
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 0);
            player.SetGravity(GravityDirection.Down);
            player.PlayerMovement.speedMultiplier += 0.25f;
        }
    }*/

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
}
