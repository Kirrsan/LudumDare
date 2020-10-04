using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour {
    [SerializeField, Min(0)] private float baseSpeed = 15f;
    public float speedMultiplier = 1f;

    public float Speed => speedMultiplier * baseSpeed;

    private PlayerController controller;
    public new Rigidbody rigidbody;

    private Vector3 lastPosition;

    private void Awake() {
        controller = GetComponent<PlayerController>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        var stuck = Math.Abs(transform.position.z - lastPosition.z) < 0.01f && !controller.IsGrounded;
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, stuck ? 0f : Speed);
        lastPosition = transform.position;
    }
}
