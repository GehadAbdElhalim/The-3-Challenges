using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioListener))]
public class AudioController : MonoBehaviour
{
    public static AudioController instance;

    private void Awake()
    {
        instance = this;
    }

    public void Mute()
    {
        GetComponent<AudioListener>().enabled = false;
    }

    public void Unmute()
    {
        GetComponent<AudioListener>().enabled = true;
    }
}
