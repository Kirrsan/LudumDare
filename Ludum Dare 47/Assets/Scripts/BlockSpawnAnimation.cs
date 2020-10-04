using UnityEngine;

public class BlockSpawnAnimation : MonoBehaviour {
    [SerializeField] private AnimationCurve curve;
    private static PlayerMovement player;

    private Vector3 targetPosition;
    private Vector3 StartingPosition => targetPosition + positionOffset;

    private Vector3 startingAngle;

    private Vector3 positionOffset;
    private Vector3 angleOffset;

    private void Awake() {
        if (!player)
            player = FindObjectOfType<PlayerMovement>();
        targetPosition = transform.position;
        startingAngle = transform.eulerAngles;
        positionOffset = new Vector3(Random.Range(-10, 10), Random.Range(-5, -15), Random.Range(0, 5));
        angleOffset = new Vector3(Random.Range(-90, 90), Random.Range(-90, 90), Random.Range(-90, 90));
    }

    private void Update() {
        var dist = targetPosition.z - player.transform.position.z;

        var distRelativeClamped = Mathf.InverseLerp(20, 60, dist);

        var curved = curve.length > 0 ? curve.Evaluate(distRelativeClamped) : distRelativeClamped;

        transform.position = Vector3.LerpUnclamped(targetPosition, StartingPosition, curved);

        transform.eulerAngles = startingAngle + curved * angleOffset;
    }
}
