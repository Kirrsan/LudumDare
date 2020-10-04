using UnityEngine;

public class Portal : MonoBehaviour {
    private BoxCollider boxCollider;
    public PlayerMovement playerMovement;
    public LevelManager lvlManager;
    public float addSpeed;

    private void Start() {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            playerMovement.speedMultiplier = addSpeed;
            lvlManager.ActivateNextWorld();
        }
    }
}
