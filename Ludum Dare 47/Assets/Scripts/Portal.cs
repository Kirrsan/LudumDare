using UnityEngine;

public class Portal : MonoBehaviour {
    public PlayerMovement playerMovement;
    public LevelManager lvlManager;
    public float addSpeed;

    [SerializeField] private GameObject[] portalVFX;
    public DeactivateSections deactivateSectionManager;

    public void SetIndex(int index) {
        for (var i = 0; i < portalVFX.Length; i++)
            portalVFX[i].SetActive(index == i);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            AudioManager.instance.Play("Portal");
            playerMovement.speedMultiplier = addSpeed;
            lvlManager.ActivateNextWorld();
            PlayerController playerController = other.GetComponent<PlayerController>();
            playerController.GoThroughPortal();
            deactivateSectionManager.StartCoroutine(deactivateSectionManager.DeactivateSection());
        }
    }
}
