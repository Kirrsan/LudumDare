using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [SerializeField] private PlayerController playerController;
    [SerializeField, Min(0)] private float lerpAmount = 0.25f;

    private Vector3 offset, targetOffset;

    private float speedMultiplier;

    private void Awake() {
        if (playerController)
            targetOffset = transform.position - playerController.transform.position;
    }

    private void FixedUpdate() {
        if (!playerController)
            return;

        var targetPosition = playerController.transform.position + targetOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpAmount);

        speedMultiplier = Mathf.Lerp(speedMultiplier, playerController.PlayerMovement.speedMultiplier, lerpAmount);

        transform.forward = Vector3.Lerp(
            transform.forward,
            playerController.transform.position + speedMultiplier * 20 * playerController.transform.forward - transform.position,
            lerpAmount / 10
        );
    }
}
