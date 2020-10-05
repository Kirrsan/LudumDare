using System;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerMovement : MonoBehaviour {
    [SerializeField, Min(0)] private float baseSpeed = 15f;
    public float speedMultiplier = 1f;

    public float BaseSpeed => baseSpeed;
    public float Speed => speedMultiplier * baseSpeed;

    private PlayerController controller;

    private Vector3 lastPosition;

    public bool isStuck;

    private void Awake() {
        controller = GetComponent<PlayerController>();
    }

    private void FixedUpdate() {
        isStuck = Math.Abs(transform.position.z - lastPosition.z) < 0.01f && !controller.IsTouchingGround;
        controller.Rigidbody.velocity = new Vector3(controller.Rigidbody.velocity.x, controller.Rigidbody.velocity.y, isStuck ? 0f : Speed);
        lastPosition = transform.position;
    }
}
