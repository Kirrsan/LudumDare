using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class DisplayFPS : MonoBehaviour
{
    private Text guiText;
    public float updateInterval = 0.5F;

    private float accum = 0; // FPS accumulated over the interval
    private int frames = 0; // Frames drawn over the interval
    private float timeleft; // Left time for current interval

    void Start()
    {
        guiText = GetComponent<Text>();
        timeleft = updateInterval;
    }

    void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        // Interval ended - update GUI text and start new interval
        if (timeleft <= 0.0)
        {
            int fps = (int)accum / frames;
            guiText.text = fps.ToString() + " FPS";

            if (fps >= 30)
                guiText.color = Color.green;
            else if (fps >= 10)
                guiText.color = Color.yellow;
            else
                guiText.color = Color.red;
            timeleft = updateInterval;
            accum = 0.0F;
            frames = 0;
        }
    }
}
