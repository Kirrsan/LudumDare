using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{

    BoxCollider boxCollider;
    public LevelManager lvlManager;

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
        if (other.gameObject.name == "Player")
        {
            
            lvlManager.ActivateNextWorld();
        }
    }
}
