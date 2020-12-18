using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class FinishLine : MonoBehaviour
{
    public float vanishTimer = 1;
    public GameObject canvas;
    public GameObject winPanel;
    
    private float currentTimer;
    private bool triggered;
    private GameObject player;
    private Vector3 initialPlayerScale;
    
    void PlayerThrowFinishLine()
    {
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        player.GetComponentInChildren<Animator>().speed /= 3;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject;
            initialPlayerScale = player.transform.localScale;
            triggered = true;
            currentTimer = vanishTimer;
            PlayerThrowFinishLine();
        }
    }

    private void Update()
    {
        if (triggered && currentTimer > 0)
        {
            currentTimer -= Time.deltaTime;
            player.transform.localScale = (currentTimer/vanishTimer) * initialPlayerScale;
            if (currentTimer <= 0)
                DisplayScore();
        }
    }

    private void DisplayScore()
    {
        canvas.GetComponent<GraphicRaycaster>().enabled = true;
        winPanel.SetActive(true);
        Debug.Log("FINISH !!!!!!!");
    }
}
