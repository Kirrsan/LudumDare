using UnityEngine;

[ExecuteInEditMode]
public class CameraFollow : MonoBehaviour {
    [SerializeField] private PlayerController playerController;

    [SerializeField] private Vector3[] worldOffsets = {new Vector3(0, 4, -7), new Vector3(0, 1, -7)};

    [SerializeField, Min(0)] private float lerpAmount = 0.25f;

    private LevelManager levelManager;

    private float speedMultiplier;

    private void Awake() {
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void FixedUpdate() {
        if (!playerController)
            return;

        var targetPosition = playerController.transform.position + worldOffsets[levelManager.CurrentWorld];
        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpAmount);

        speedMultiplier = Mathf.Lerp(speedMultiplier, playerController.PlayerMovement.speedMultiplier, lerpAmount);

        transform.forward = Vector3.Lerp(
            transform.forward,
            playerController.transform.position + speedMultiplier * 20 * playerController.transform.forward - transform.position,
            lerpAmount / 10
        );
    }
}
