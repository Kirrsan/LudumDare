using System;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerMovement : MonoBehaviour {
    [SerializeField, Min(0)] private float baseSpeed = 15f;
    public float speedMultiplier = 1f;

    public float Speed => speedMultiplier * baseSpeed;

    private PlayerController controller;

    private Vector3 lastPosition;

    private void Awake() {
        controller = GetComponent<PlayerController>();
    }

    private void FixedUpdate() {
        var stuck = Math.Abs(transform.position.z - lastPosition.z) < 0.01f && !controller.IsTouchingGround;
        if (stuck)
            Debug.Log("Player is stuck!");
        controller.Rigidbody.velocity = new Vector3(controller.Rigidbody.velocity.x, controller.Rigidbody.velocity.y, stuck ? 0f : Speed);
        lastPosition = transform.position;
    }
}
