﻿using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    public static Animator animator;

    [SerializeField, Min(0)] private float jumpForce = 20f;
    [SerializeField] private Vector3 respawnPosition = new Vector3(0, 50, -20);
    [SerializeField] private LayerMask ground;
    [SerializeField] private Bounds killLimit;

    [System.Serializable]
    private struct Sphere {
        public Vector3 center;
        public float radius;
    }

    [SerializeField] private Sphere[] sphereCasts;

    private readonly ICapacity[] capacities = {new DoubleJump(), new WallRun()};

    private LevelManager levelManager;

    private new Rigidbody rigidbody;

    private PlayerMovement playerMovement;
    public PlayerMovement PlayerMovement => playerMovement;

    [SerializeField] private bool isGrounded;
    public bool IsGrounded => isGrounded;

    public LayerMask Ground => ground;

    private GravityDirection gravityDirection = GravityDirection.Down;

    [SerializeField] private GameObject[] reactors;
    
    private int _numberOfWorlds;

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        levelManager = FindObjectOfType<LevelManager>();
        animator = GetComponent<Animator>();
        _numberOfWorlds = levelManager.GetWorldArrayLength();
    }

    private void Start() {
        reactors[levelManager.CurrentWorld].SetActive(true);
    }

    private void Update() {
        if (playerMovement.speedMultiplier == 0f && isGrounded)
            playerMovement.speedMultiplier = 1f;

        if (Input.GetKeyDown(KeyCode.Space))
            capacities[levelManager.CurrentWorld].UseCapacity(this);

        if (CheckLoseCondition()) {
            FindObjectOfType<CameraFollow>().transform.position += respawnPosition - transform.position;
            transform.position = respawnPosition;
            levelManager.Reset();
            Reset();
        }
        animator.SetFloat("Velocity Y", rigidbody.velocity.y);
        animator.SetBool("IsGrounded", isGrounded);
    }

    private void FixedUpdate() {
        isGrounded = GroundCheck();
        capacities[levelManager.CurrentWorld].Update(this);
    }

    private void Reset() {
        AudioManager.instance.Play("Rewind");
        playerMovement.speedMultiplier = 0f;
        SetGravity(GravityDirection.Down);
    }

    #region WinLoseCondition

    private bool CheckLoseCondition() => !killLimit.Contains(transform.position) && transform.position.y < killLimit.max.y;

    #endregion

    private bool GroundCheck() =>
        rigidbody.velocity.y <= 0.01f
        && sphereCasts.Any(s => Physics.CheckSphere(transform.position + s.center, s.radius, ground));

    public void Jump() {
        switch (gravityDirection) {
            case GravityDirection.Down:
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpForce, rigidbody.velocity.z);
                break;
            case GravityDirection.Left:
                rigidbody.velocity = new Vector3(jumpForce, rigidbody.velocity.y, rigidbody.velocity.z);
                break;
            case GravityDirection.Right:
                rigidbody.velocity = new Vector3(-jumpForce, rigidbody.velocity.y, rigidbody.velocity.z);
                break;
        }
    }

    public void SetGravity(GravityDirection direction) {
        gravityDirection = direction;
        switch (direction) {
            case GravityDirection.Down:
                Physics.gravity = new Vector3(0, -Physics.gravity.magnitude, 0);
                rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, rigidbody.velocity.z);
                break;
            case GravityDirection.Left:
                Physics.gravity = new Vector3(-Physics.gravity.magnitude, 0, 0);
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
                break;
            case GravityDirection.Right:
                Physics.gravity = new Vector3(Physics.gravity.magnitude, 0, 0);
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
                break;
        }
    }
    
    public void SetActionsAccordingToWorld()
    {
        //activate right reactor
        reactors[levelManager.CurrentWorld].SetActive(true);
        //deactivate last reactor
        if(levelManager.CurrentWorld != 0)
            reactors[levelManager.CurrentWorld - 1].SetActive(false);
        else
            reactors[_numberOfWorlds - 1].SetActive(false);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("KillZone"))
            GameManager.instance.ChangeState(State.LOSE);

        else if (other.CompareTag("WinZone")) {
            AudioManager.instance.Play("Win");
            GameManager.instance.ChangeState(State.WIN);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            playerMovement.rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, 0f);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(killLimit.center, killLimit.extents);
        Gizmos.color = Color.white;

        foreach (var sphere in sphereCasts)
            Gizmos.DrawWireSphere(transform.position + sphere.center, sphere.radius);

        Gizmos.DrawRay(transform.position, 5 * (transform.forward - transform.right));
        Gizmos.DrawRay(transform.position, 5 * (transform.forward + transform.right));
    }
}

public enum GravityDirection {
    Down,
    Left,
    Right
}
