using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{

    [SerializeField] private Transform _cloudInFront;
    [SerializeField] private GameObject[] _cloudCycle;
    private Vector3 _startPosition;

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;

        LevelManager.instance.onReset += () =>
        {  
            ResetPosition();
        };

        LevelManager.instance.onWorldChange += (int worldIndex) =>
        {
            ChangeFog(worldIndex);
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

    private void ChangeFog(int worldIndex)
    {
        for(int i = 0; i < _cloudCycle.Length; i++)
        {
            if(i == worldIndex)
                _cloudCycle[i].SetActive(true);
            else
            {
                _cloudCycle[i].SetActive(false);
            }
        }
    }

}
