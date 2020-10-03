[System.Serializable]
public class DoubleJump : ICapacity {
    private int jumps;

    public void Update(PlayerController player) {
        if (player.IsGrounded)
            jumps = 2;
    }

    public void UseCapacity(PlayerController player) {
        if (jumps <= 0)
            return;

        player.Jump();
        jumps--;
    }
}
