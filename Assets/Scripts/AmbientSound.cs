using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AmbientSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] mainSongs;
    [SerializeField] private AudioClip[] bossSongs;
    private AudioClip mainSong, bossSong;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        mainSong = null;
        bossSong = null;

        ChooseSong();
    }

    public void ChooseSong()
    {
        if (mainSong == null)
        {
            mainSong = mainSongs[Random.Range(0, mainSongs.Length)];
        }

        audioSource.clip = mainSong;
        audioSource.Play();
    }
    public void ChooseBossSong()
    {
        if (bossSong == null)
        {
            bossSong = bossSongs[Random.Range(0, bossSongs.Length)];
        }

        audioSource.clip = bossSong;
        audioSource.Play();
    }
}