using UnityEngine;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour {
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

    private new Rigidbody rigidbody;
    public Rigidbody Rigidbody => rigidbody;

    private PlayerMovement playerMovement;
    public PlayerMovement PlayerMovement => playerMovement;

    public LayerMask Ground => ground;

    [SerializeField] private GameObject[] reactors;

    public bool IsTouchingGround =>
        rigidbody.velocity.y <= 0.01f
        && Physics.CheckSphere(transform.position + transform.rotation * sphereCast.center, sphereCast.radius, ground);

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        levelManager = FindObjectOfType<LevelManager>();
        animator = GetComponent<Animator>();
    }

    private void Start() {
        reactors[levelManager.CurrentWorld].SetActive(true);
    }

    private void Update() {
        if ((playerMovement.isStuck || playerMovement.speedMultiplier == 0f) && IsTouchingGround && levelManager.CurrentWorld == 0) {
            playerMovement.speedMultiplier = 1f;
            playerMovement.isStuck = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !playerMovement.isStuck)
            Capacity.Use(this);

        if (CheckLoseCondition()) {
            AudioManager.instance.Play("Rewind");
            if (levelManager.onReset != null) levelManager.onReset.Invoke();
            FindObjectOfType<CameraFollow>().transform.position += respawnPosition - transform.position;
            Reset();
            levelManager.Reset();
        }

        animator.SetFloat("Velocity Y", rigidbody.velocity.y);
        animator.SetBool("IsGrounded", IsTouchingGround);
        animator.SetBool("IsLanding", playerMovement.speedMultiplier == 0f);
    }

    private void FixedUpdate() {
        if (playerMovement.isStuck)
            rigidbody.useGravity = true;
        else
            Capacity.FixedUpdate(this);
    }

    private void Reset() {
        transform.position = respawnPosition;
        playerMovement.speedMultiplier = 0f;
        Rigidbody.velocity = new Vector3(0, Rigidbody.velocity.y, 0);
        transform.rotation = Quaternion.identity;

        foreach (var capacity in capacities)
            capacity.Reset();
    }

    private bool CheckLoseCondition() => transform.position.y < killLimit.max.y && !killLimit.Contains(transform.position);

    public void Jump() {
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpForce, rigidbody.velocity.z);
    }

    public void GoThroughPortal() {
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

        Gizmos.DrawRay(transform.position, 10 * (transform.forward - transform.right));
        Gizmos.DrawRay(transform.position, 10 * (transform.forward + transform.right));
    }
}
