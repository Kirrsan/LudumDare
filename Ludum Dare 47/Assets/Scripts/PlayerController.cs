using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {
    [SerializeField, Min(0)] private float jumpForce = 20f;
    [SerializeField] private LayerMask ground;
    [SerializeField] private Bounds groundCheckBounds;
    [SerializeField] private Transform[] killLimit;
    [SerializeField] private Vector3 groundCheckCenter;
    [SerializeField] private float groundCheckRadius;

    private ICapacity[] capacities = {new DoubleJump(), new WallRun()};

    private LevelManager levelManager;

    private new Rigidbody rigidbody;
    public Rigidbody Rigidbody => rigidbody;

    private PlayerMovement playerMovement;
    public PlayerMovement PlayerMovement => playerMovement;

    [SerializeField] private bool isGrounded;
    public bool IsGrounded => isGrounded;

    public LayerMask Ground => ground;

    private GravityDirection gravityDirection = GravityDirection.Down;

    private Vector3 _startPosition;

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        levelManager = FindObjectOfType<LevelManager>();
        _startPosition = transform.position;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
            capacities[levelManager.CurrentWorld].UseCapacity(this);
        GameOver(CheckLoseCondition());
    }

    #region WinLoseCondition

    void GameOver(bool isLost) {
        if (isLost) {
            Debug.Log("GAME OVER");
            transform.position = _startPosition;
            GameManager.instance.deathCounter++;
            levelManager.ResetWorld();
            playerMovement.currentSpeed = playerMovement.baseSpeed;
            SetGravity(GravityDirection.Down);
        }
    }

    bool CheckLoseCondition() {
        if (transform.position.y < killLimit[0].position.y || transform.position.x < killLimit[1].position.x || transform.position.x > killLimit[2].position.x)
            return true;
        return false;
    }

    #endregion


    private void FixedUpdate() {
        isGrounded = GroundCheck();
        capacities[levelManager.CurrentWorld].Update(this);
    }

    private bool GroundCheck() =>
        rigidbody.velocity.y <= 0.01f
        && Physics.CheckSphere(transform.position + groundCheckCenter, groundCheckRadius, ground);

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position + groundCheckCenter, groundCheckRadius);
        Gizmos.DrawRay(transform.position, 5 * (transform.forward - transform.right));
        Gizmos.DrawRay(transform.position, 5 * (transform.forward + transform.right));
    }

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
                rigidbody.velocity = new Vector3(0, 0, rigidbody.velocity.z);
                break;
            case GravityDirection.Left:
                Physics.gravity = new Vector3(-Physics.gravity.magnitude, 0, 0);
                rigidbody.velocity = new Vector3(0, 0, rigidbody.velocity.z);
                break;
            case GravityDirection.Right:
                Physics.gravity = new Vector3(Physics.gravity.magnitude, 0, 0);
                rigidbody.velocity = new Vector3(0, 0, rigidbody.velocity.z);
                break;
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("KillZone"))
            GameManager.instance.ChangeState(State.LOOSE);

        else if (other.CompareTag("WinZone"))
            GameManager.instance.ChangeState(State.WIN);
    }
}

public enum GravityDirection {
    Down,
    Left,
    Right
}
