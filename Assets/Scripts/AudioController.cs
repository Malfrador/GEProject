using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AudioController : MonoBehaviour
{
    public AudioSource UISource;
    public AudioClip buttonNoise;

    public AudioClip peepDie;
    public AudioClip peepSpawn;
    public AudioClip peepFinish;
    public AudioClip gameWon;
    public AudioClip gameLost;


    public void playButtonSound()
    {
        UISource.clip = buttonNoise;
        UISource.Play();
    }

    public void playClip(string name)
    {
        switch (name)
        {
            case "peepDie":
                playClip(peepDie);
                break;
            case "peepSpawn":
                playClip(peepSpawn);
                break;
            case "peepFinish":
                playClip(peepFinish);
                break;
            case "gameWon":
                playClip(gameWon);
                break;
        }
    }

    private void playClip(AudioClip clip)
    {
        UISource.clip = clip;
        UISource.Play();
    }
}
