using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour {
    /*[SerializeField, Min(0)] private*/public float baseSpeed = 5.0f;

    //public float speedMultiplier = 1f;

    private float Speed => baseSpeed /** speedMultiplier*/;

    private new Rigidbody rigidbody;

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update() {
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, Speed);
    }

    public float GetSpeed()
    {
        return Speed;
    }
}
