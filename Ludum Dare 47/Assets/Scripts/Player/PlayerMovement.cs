using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour {
    [SerializeField, Min(0)] private float baseSpeed = 15f;
    public float speedMultiplier = 1f;
    public bool isBugged = false;

    public float Speed => speedMultiplier * baseSpeed;

    private new Rigidbody rigidbody;

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update() {
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, isBugged ? 0f : Speed);
        
        
    }

    public void speedZero()
    {
        speedMultiplier = 0f;
    }
}
