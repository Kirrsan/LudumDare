using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour {
    /*[SerializeField, Min(0)] private*/
    public float baseSpeed = 5.0f;

    public float Speed => baseSpeed;

    public float currentSpeed = 0;

    private new Rigidbody rigidbody;

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        currentSpeed = baseSpeed;
    }

    private void Update() {
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, currentSpeed);
    }
}
