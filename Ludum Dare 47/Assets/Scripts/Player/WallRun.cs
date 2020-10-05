using UnityEngine;

public class WallRun : ICapacity {
    private const float Glide = 40f;

    private const float GlideDecrease = 1f;

    private const float DashTargetY = 4f;

    private IState state;

    public WallRun() {
        state = new Gliding(true, Glide, GlideDecrease);
    }

    public void Reset() => state = new Gliding(true, Glide, GlideDecrease);

    public void FixedUpdate(PlayerController player) {
        player.Rigidbody.useGravity = state is Gliding;
        state = state.FixedUpdate(player);
    }

    public void Use(PlayerController player) => state = state.Use(player);

    private interface IState {
        IState FixedUpdate(PlayerController player);
        IState Use(PlayerController player);
    }

    private struct Gliding : IState {
        private bool wasLeftWall;
        private float glide;
        private readonly float glideDecrease;

        public Gliding(bool wasLeftWall, float glide, float glideDecrease) {
            this.wasLeftWall = wasLeftWall;
            this.glide = glide;
            this.glideDecrease = glideDecrease;
        }

        public IState FixedUpdate(PlayerController player) {
            player.Rigidbody.velocity = new Vector3(0, player.Rigidbody.velocity.y, player.Rigidbody.velocity.z);
            if (player.Rigidbody.velocity.y < 0)
                player.Rigidbody.AddForce(glide * Vector3.up, ForceMode.Acceleration);
            glide -= glideDecrease * Time.fixedDeltaTime;
            return this;
        }

        public IState Use(PlayerController player) {
            var result = RaycastWalls(player);
            if (result.wall != Wall.None) {
                var targetPosition = result.hit.point + (result.IsLeftWall ? Vector3.right : Vector3.left);
                return new DashingToWall(result, player.transform.position, targetPosition);
            }

            return this;
        }

        private WallRaycastResult RaycastWalls(PlayerController player) {
            var leftRaycast = Physics.Raycast(player.transform.position, Vector3.left + Vector3.forward, out var leftHit, 10, player.Ground);
            var rightRaycast = Physics.Raycast(player.transform.position, Vector3.right + Vector3.forward, out var rightHit, 10, player.Ground);

            if (leftRaycast && rightRaycast) {
                AudioManager.instance.Play("Dash");
                PlayerController.animator.Play("Dash");
                if (wasLeftWall)
                    return new WallRaycastResult(Wall.Right, rightHit);
                return new WallRaycastResult(Wall.Left, leftHit);
            }

            if (leftRaycast) {
                AudioManager.instance.Play("Dash");
                PlayerController.animator.Play("Dash");
                return new WallRaycastResult(Wall.Left, leftHit);
            }

            if (rightRaycast) {
                AudioManager.instance.Play("Dash");
                PlayerController.animator.Play("Dash");
                return new WallRaycastResult(Wall.Right, rightHit);
            }

            return new WallRaycastResult(Wall.None, leftHit);
        }
    }

    private struct Running : IState {
        private bool isLeftWall;

        public Running(bool isLeftWall) {
            this.isLeftWall = isLeftWall;
        }

        public IState FixedUpdate(PlayerController player) {
            player.Rigidbody.useGravity = !player.IsTouchingGround;
            if (!player.Rigidbody.useGravity)
                player.Rigidbody.velocity = new Vector3(0, 0, player.Rigidbody.velocity.z);

            return this;
        }

        public IState Use(PlayerController player) {
            if (!player.IsTouchingGround)
                return this;

            var duration = Mathf.Abs(player.transform.position.x) / player.PlayerMovement.Speed;
            var targetY = Mathf.Max(DashTargetY, player.transform.position.y);
            var targetPosition = new Vector3(0, targetY, player.transform.position.z + duration * player.Rigidbody.velocity.z);
            return new DashingToCenter(isLeftWall, player.transform.position, targetPosition);
        }
    }

    private struct DashingToWall : IState {
        private WallRaycastResult result;
        private Vector3 startingPosition;
        private Vector3 targetPosition;

        public DashingToWall(WallRaycastResult result, Vector3 startingPosition, Vector3 targetPosition) {
            this.result = result;
            this.startingPosition = startingPosition;
            this.targetPosition = targetPosition;
        }

        public IState FixedUpdate(PlayerController player) {
            var progress = Mathf.InverseLerp(startingPosition.x, targetPosition.x, player.transform.position.x);
            player.transform.eulerAngles = progress * (result.IsLeftWall ? -90 : 90) * Vector3.forward;
            player.Rigidbody.velocity = new Vector3(
                result.IsLeftWall ? -player.PlayerMovement.Speed : player.PlayerMovement.Speed,
                0,
                player.Rigidbody.velocity.z
            );

            if (player.IsTouchingGround) {
                player.transform.eulerAngles = (result.IsLeftWall ? -90 : 90) * Vector3.forward;
                player.Rigidbody.velocity = new Vector3(0, 0, player.Rigidbody.velocity.z);
                return new Running(result.IsLeftWall);
            }

            return this;
        }

        public IState Use(PlayerController player) => this;
    }

    private struct DashingToCenter : IState {
        private bool isLeftWall;
        private Vector3 startingPosition;
        private Vector3 targetPosition;

        public DashingToCenter(bool isLeftWall, Vector3 startingPosition, Vector3 targetPosition) {
            this.isLeftWall = isLeftWall;
            this.startingPosition = startingPosition;
            this.targetPosition = targetPosition;
        }

        public IState FixedUpdate(PlayerController player) {
            var duration = Mathf.Abs(startingPosition.x) / player.PlayerMovement.Speed;
            var progress = Mathf.InverseLerp(startingPosition.x, targetPosition.x, player.transform.position.x);
            player.transform.eulerAngles = (1 - progress) * (isLeftWall ? -90 : 90) * Vector3.forward;
            player.Rigidbody.velocity = new Vector3(isLeftWall ? player.PlayerMovement.Speed : -player.PlayerMovement.Speed, (targetPosition.y - startingPosition.y) / duration, player.Rigidbody.velocity.z);

            if (isLeftWall ? player.transform.position.x > 0 : player.transform.position.x < 0) {
                player.Rigidbody.velocity = new Vector3(0, 0, player.Rigidbody.velocity.z);
                return new Gliding(isLeftWall, Glide, GlideDecrease);
            }

            return this;
        }

        public IState Use(PlayerController player) => this;
    }

    private enum Wall {
        None,
        Left,
        Right
    }

    private struct WallRaycastResult {
        public Wall wall;
        public RaycastHit hit;

        public bool IsLeftWall => wall == Wall.Left;

        public WallRaycastResult(Wall wall, RaycastHit hit) {
            this.wall = wall;
            this.hit = hit;
        }
    }
}
