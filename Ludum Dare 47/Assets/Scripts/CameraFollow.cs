using UnityEngine;

[ExecuteInEditMode]
public class CameraFollow : MonoBehaviour {
    [SerializeField] private PlayerController playerController;

    [SerializeField] private Vector3[] worldOffsets = {new Vector3(0, 4, -7), new Vector3(0, 1, -7)};

    [SerializeField, Min(0)] private float lerpAmount = 0.25f;

    private LevelManager levelManager;

    private float speedMultiplier;

    private Vector3 WorldOffset => worldOffsets[levelManager.CurrentWorld];
    private Vector3 SmoothWorldOffset => worldOffset = Vector3.Lerp(worldOffset, WorldOffset, 0.1f);
    private Vector3 worldOffset;

    private void Awake() {
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void FixedUpdate() {
        if (!playerController)
            return;

        var targetPosition = playerController.transform.position + SmoothWorldOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpAmount);

        speedMultiplier = Mathf.Lerp(speedMultiplier, playerController.PlayerMovement.speedMultiplier, lerpAmount);

        if (playerController.transform.position.y < -2f || playerController.IsFalling)
            transform.forward = Vector3.Lerp(
                transform.forward,
                playerController.transform.position - transform.position,
                lerpAmount / 10
            );
        else
            transform.forward = Vector3.Lerp(
                transform.forward,
                playerController.transform.position + speedMultiplier * 20 * playerController.transform.forward - transform.position,
                lerpAmount / 10
            );
    }
}
