[System.Serializable]
public class DoubleJump : ICapacity {
    private int jumps;

    public void Reset() {
        jumps = 0;
    }

    public void FixedUpdate(PlayerController player) {
        player.Rigidbody.useGravity = true;
        if (player.IsTouchingGround)
            jumps = 2;
    }

    public void Use(PlayerController player) {
        if (jumps <= 0)
            return;

        if (jumps == 2) {
            AudioManager.instance.Play("Jump");
            PlayerController.animator.Play("Jump");
        } else {
            AudioManager.instance.Play("DoubleJump");
            PlayerController.animator.Play("DoubleJump");
        }

        player.Jump();
        jumps--;
    }

    public int GetJumpLeft() {
        return jumps;
    }
}
