using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AmbientSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] songs;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        ChooseSong();
    }

    private void ChooseSong()
    {
        AudioClip song = songs[Random.Range(0, songs.Length)];

        if (song == null)
            return;

        audioSource.clip = song;
        audioSource.Play();
    }
}