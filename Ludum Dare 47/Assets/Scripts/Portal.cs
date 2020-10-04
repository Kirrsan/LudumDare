using UnityEngine;

public class Portal : MonoBehaviour {
    
    
    private BoxCollider boxCollider;
    public PlayerMovement playerMovement;
    public LevelManager lvlManager;
    public float addSpeed;
    
    public GameObject[] portalVFX;

    private void Start() {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            AudioManager.instance.Play("Portal");
            playerMovement.speedMultiplier = addSpeed;
            lvlManager.ActivateNextWorld();
            PlayerController playerController = other.GetComponent<PlayerController>();
            playerController.SetActionsAccordingToWorld();
            if(lvlManager.CurrentWorld == 0)
                playerController.SetGravity(GravityDirection.Down);
        }
    }
}
