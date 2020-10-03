using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestParticle : MonoBehaviour
{

    public float speed = 5;

    public float[] speedPallier;

    public float[] rateOvertime;
    public float[] startSpeed;

    public bool[] pallierReached;

    public ParticleSystem system;

    public int currentPallier;

    // Start is called before the first frame update
    void Start()
    {
        pallierReached = new bool[speedPallier.Length];
        for (int i = 0; i < pallierReached.Length; i++)
        {
            pallierReached[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPallier < speedPallier.Length)
        {
            if (speed >= speedPallier[currentPallier] && !pallierReached[currentPallier])
            {
                pallierReached[currentPallier] = true;
                system.startSpeed = startSpeed[currentPallier];
                system.emissionRate = rateOvertime[currentPallier];
                currentPallier++;
            }
        }
        if (currentPallier - 1 > 0)
        {
            if (speed < speedPallier[currentPallier - 1] && pallierReached[currentPallier -1])
            {
                currentPallier--;
                pallierReached[currentPallier] = false;
                system.startSpeed = startSpeed[currentPallier - 1];
                system.emissionRate = rateOvertime[currentPallier - 1];
            }
        }
    }
}
