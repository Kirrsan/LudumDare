﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour {
    [SerializeField] private Animator _animator;
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

    [SerializeField] private Sphere sphereCast;

    private readonly ICapacity[] capacities = {new DoubleJump(), new WallRun()};
    private ICapacity Capacity => capacities[levelManager.CurrentWorld];

    private LevelManager levelManager;
    public DeactivateSections deactivateSectionManager;

    private new Rigidbody rigidbody;
    public Rigidbody Rigidbody => rigidbody;

    private PlayerMovement playerMovement;
    public PlayerMovement PlayerMovement => playerMovement;

    private bool isFalling;
    public bool IsFalling => isFalling;

    public LayerMask Ground => ground;

    [SerializeField] private GameObject[] reactors;

    public bool IsTouchingGround =>
        rigidbody.velocity.y <= 0.01f
        && Physics.CheckSphere(transform.position + transform.rotation * sphereCast.center, sphereCast.radius, ground);

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        levelManager = FindObjectOfType<LevelManager>();
        animator = _animator;
    }

    private void Start() {
        reactors[levelManager.CurrentWorld].SetActive(true);
    }

    private void Update() {
        if ((playerMovement.isStuck || isFalling) && IsTouchingGround && levelManager.CurrentWorld == 0) {
            animator.Play("Landing");
            playerMovement.speedMultiplier = 1f;
            playerMovement.isStuck = false;
            isFalling = false;
        }

        if (!playerMovement.isStuck)
        {
            if(Input.GetKeyDown(KeyCode.Space))
                Capacity.Use(this);
#if UNITY_ANDROID
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    Capacity.Use(this);
                    break;
                }
            }
#endif
        }

        if (CheckLoseCondition()) {
            AudioManager.instance.Play("Rewind");
            if (levelManager.onReset != null)
                levelManager.onReset.Invoke();
            Reset();
            levelManager.Reset();
        }

        animator.SetFloat("Velocity Y", rigidbody.velocity.y);
        animator.SetBool("IsGrounded", IsTouchingGround);
    }

    private void FixedUpdate() {
        if (playerMovement.isStuck)
            rigidbody.useGravity = true;
        else
            Capacity.FixedUpdate(this);
    }

    private void Reset() {
        var d = respawnPosition.y - 1;
        var v = -rigidbody.velocity.y;
        var g = -Physics.gravity.y;
        var fallDuration = (-v + Mathf.Sqrt(2 * g * d + v * v)) / g;
        Debug.Log(fallDuration);
        var fallDistanceZ = fallDuration * rigidbody.velocity.z;

        var camera = FindObjectOfType<CameraFollow>();
        var cameraOffset = camera.transform.position - transform.position;
        transform.position = respawnPosition + fallDistanceZ * Vector3.back;
        camera.transform.position = transform.position + cameraOffset;

        deactivateSectionManager.ResetSections();
        isFalling = true;
        transform.rotation = Quaternion.identity;
        reactors[0].SetActive(true);
        reactors[1].SetActive(false);

        foreach (var capacity in capacities)
            capacity.Reset();
    }

    private bool CheckLoseCondition() => transform.position.y < killLimit.max.y && !killLimit.Contains(transform.position);

    public void Jump() {
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpForce, rigidbody.velocity.z);
    }

    public void GoThroughPortal() {
        rigidbody.velocity = new Vector3(0, 0, rigidbody.velocity.z);
        if (levelManager.CurrentWorld == 1)
            Jump();

        transform.position = new Vector3(0, transform.position.y, transform.position.z);
        transform.up = Vector3.up;

        for (var i = 0; i < levelManager.WorldCount; i++)
            if (i != levelManager.CurrentWorld)
                capacities[i].Reset();

        for (var i = 0; i < levelManager.WorldCount; i++)
            reactors[i].SetActive(i == levelManager.CurrentWorld);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("KillZone"))
            GameManager.instance.ChangeState(State.LOSE);

        else if (other.CompareTag("WinZone")) {
            AudioManager.instance.Play("Win");
            GameManager.instance.ChangeState(State.WIN);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Wall")) {
            Rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, 0f);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(killLimit.center, killLimit.extents);
        Gizmos.color = Color.white;

        Gizmos.DrawWireSphere(transform.position + transform.rotation * sphereCast.center, sphereCast.radius);

        Gizmos.DrawRay(transform.position, 10 * (Vector3.left + Vector3.forward));
        Gizmos.DrawRay(transform.position, 10 * (Vector3.right + Vector3.forward));
    }
}
