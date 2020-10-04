using UnityEngine;

public class WallRun : ICapacity {
    public void Update(PlayerController player) { }

    public void UseCapacity(PlayerController player) {
        Debug.Log("UseCapacity");
        if (player.IsGrounded)
        {
            AudioManager.instance.Play("Jump");
            player.Jump();
        }
        else
            AttachToWall(player);
    }

    private void AttachToWall(PlayerController player) {
        Debug.Log("AttachToWall");
        var leftRaycast = Physics.Raycast(player.transform.position, player.transform.forward - player.transform.right, 50f, player.Ground);

        if (leftRaycast) {
            Debug.Log("Found left wall");
            AudioManager.instance.Play("Dash");
            player.SetGravity(GravityDirection.Left);
            return;
        }

        var rightRaycast = Physics.Raycast(player.transform.position, player.transform.forward + player.transform.right, 50f, player.Ground);

        if (rightRaycast) {
            Debug.Log("Found right wall");
            AudioManager.instance.Play("Dash");
            player.SetGravity(GravityDirection.Right);
        }
    }
}
