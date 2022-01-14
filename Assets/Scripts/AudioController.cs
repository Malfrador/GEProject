using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource UISource;
    public AudioClip buttonNoise;
    
    public void playButtonSound()
    {
        UISource.clip = buttonNoise;
        UISource.Play();
    }
}
