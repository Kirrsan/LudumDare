using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnStep : MonoBehaviour
{
    public void PlayStep()
    {
        AudioManager.instance.Play("Step");
    }
}
