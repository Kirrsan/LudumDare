using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{

    BoxCollider boxCollider;
    public PlayerMovement playerMovement;
    public LevelManager lvlManager;
    public float addSpeed;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    /*private void on
    {
        if (collision.gameObject.name == "Player")
        {
            Debug.Log("yes");
            lvlManager.ActivateNextWorld();
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerMovement.baseSpeed *= addSpeed;
            lvlManager.ActivateNextWorld();

        }
    }
}
