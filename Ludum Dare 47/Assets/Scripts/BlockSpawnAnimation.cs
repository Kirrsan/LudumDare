using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlockSpawnAnimation : MonoBehaviour {
    private static PlayerMovement player;

    private enum Mode {
        RotateFromBottom,
        DropFromTop
    }

    [SerializeField] private Mode mode;
    [SerializeField] private AnimationCurve curve;

    private Vector3 targetPosition;
    private Vector3 targetAngle;

    private Renderer[] renderers;

    private Portal portal;

    private float xOffset;

    private float cachedValue = -1;

    private void Start() {
        if (!player)
            player = FindObjectOfType<PlayerMovement>();

        portal = GetComponent<Portal>();
        renderers = GetComponentsInChildren<Renderer>();

        xOffset = Random.Range(-10, 10);

        targetPosition = transform.position;
        targetAngle = transform.eulerAngles;
        SetValue(0);
    }

    private void Update() {
        if (targetPosition.z < player.transform.position.z - 5)
            return;

        var dist = targetPosition.z - player.transform.position.z;
        var speed = player.speedMultiplier == 0f ? player.BaseSpeed : player.Speed;

        if (portal)
            dist += 1.5f * speed;

        var minDist = 2 * speed;
        var maxDist = 5 * speed;

        if (dist > maxDist) {
            SetValue(0);
            return;
        }

        var distRelativeClamped = Mathf.InverseLerp(maxDist, minDist, dist);

        var curved = curve.length > 0 ? curve.Evaluate(distRelativeClamped) : distRelativeClamped;

        SetValue(curved);
    }

    private void SetValue(float value) {
        if (Math.Abs(value - cachedValue) < 0.0001f)
            return;

        cachedValue = value;

        if (portal)
            portal.SetIndex(value > 0 ? LevelManager.instance.CurrentWorld : -1);

        foreach (var renderer in renderers)
            renderer.enabled = value > 0;

        if (portal) {
            transform.position = new Vector3(
                targetPosition.x + (1 - value) * xOffset,
                targetPosition.y + (1 - value) * 50,
                targetPosition.z
            );
            transform.eulerAngles = targetAngle + (1 - value) * 9 * xOffset * Vector3.up;
        } else {
            transform.position = new Vector3(
                targetPosition.x,
                targetPosition.y - 50 * (1 - Mathf.Sin(value * Mathf.PI / 2)),
                targetPosition.z + 50 * Mathf.Cos(value * Mathf.PI / 2)
            );
            transform.eulerAngles = targetAngle + (1 - value) * 90 * Vector3.right;
        }
    }
}
