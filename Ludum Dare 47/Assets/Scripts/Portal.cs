using UnityEngine;

public class Portal : MonoBehaviour {
    public PlayerMovement playerMovement;
    public LevelManager lvlManager;
    public float addSpeed;

    public GameObject[] portalVFX;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            AudioManager.instance.Play("Portal");
            playerMovement.speedMultiplier = addSpeed;
            lvlManager.ActivateNextWorld();
            PlayerController playerController = other.GetComponent<PlayerController>();
            playerController.GoThroughPortal();
        }
    }
}
