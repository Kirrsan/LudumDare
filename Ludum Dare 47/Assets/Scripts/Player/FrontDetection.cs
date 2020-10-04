using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontDetection : MonoBehaviour
{
    private PlayerMovement playerMovement;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8)
        {
            playerMovement.isBugged = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            playerMovement.isBugged = true;
            Debug.Log("trig");
        }
    }
}
