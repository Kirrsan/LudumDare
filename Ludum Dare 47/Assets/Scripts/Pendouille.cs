using UnityEngine;

public class Pendouille : MonoBehaviour {
    [SerializeField] private Vector3 rotationCenter = new Vector3(0, 6.3f, 0);
    [SerializeField] private float rotationAmplitude = 0.5f;

    private Vector3 center;
    private float timeOffset;
    private float timeScale;

    private void Awake() {
        center = transform.position + rotationCenter;
        timeOffset = Random.Range(0f, Mathf.PI);
        timeScale = Random.Range(0.8f, 1.2f);
    }

    private void FixedUpdate() {
        var angle = rotationAmplitude * Mathf.Sin(timeScale * Time.time + timeOffset);
        var offset = -new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0) * rotationCenter.magnitude;
        transform.position = center + offset;
        transform.eulerAngles = new Vector3(-90, 0, 90 * angle * rotationAmplitude);
        // transform.RotateAround(center, rotationAxis, rotationAmplitude * t);
    }

    private void OnDrawGizmos() {
        if (center != Vector3.zero)
            Gizmos.DrawWireSphere(center, 0.25f);
        else
            Gizmos.DrawWireSphere(transform.position + rotationCenter, 0.25f);
    }
}
