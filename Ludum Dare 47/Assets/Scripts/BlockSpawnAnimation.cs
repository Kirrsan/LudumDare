using UnityEngine;

public class BlockSpawnAnimation : MonoBehaviour {
    [SerializeField] private AnimationCurve curve;
    private static PlayerMovement player;

    private Vector3 targetPosition;
    private Vector3 targetAngle;

    private void Awake() {
        if (!player)
            player = FindObjectOfType<PlayerMovement>();

        targetPosition = transform.position;
        targetAngle = transform.eulerAngles;
        SetValue(0);
    }

    private void Update() {
        if (targetPosition.z < player.transform.position.z) {
            SetValue(1);
            return;
        }

        var dist = targetPosition.z - player.transform.position.z;
        var speed = player.speedMultiplier == 0f ? player.BaseSpeed : player.Speed;
        var minDist = 2 * speed;
        var maxDist = 4 * speed;

        if (dist > maxDist) {
            SetValue(0);
            return;
        }

        var distRelativeClamped = Mathf.InverseLerp(maxDist, minDist, dist);

        var curved = curve.length > 0 ? curve.Evaluate(distRelativeClamped) : distRelativeClamped;

        SetValue(curved);
    }

    private void SetValue(float value) {
        transform.position = new Vector3(
            targetPosition.x,
            targetPosition.y - 30 * (1 - Mathf.Sin(value * Mathf.PI / 2)),
            targetPosition.z + 30 * Mathf.Cos(value * Mathf.PI / 2)
        );
        transform.eulerAngles = targetAngle + (1 - value) * 90 * Vector3.right;
    }
}
