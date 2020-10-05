using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{

    [SerializeField] private Transform _cloudInFront;
    private Vector3 _startPosition;

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;

        LevelManager.instance.onReset += () =>
        {  
            ResetPosition();
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, _cloudInFront.position.z + 465);
        }
    }

    private void ResetPosition()
    {
        transform.position = _startPosition;
    }

}
