﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;
    public AudioMixerGroup mixer;
    [Range(0,1)]
    public float volume = 1;
    [Range(0.1f, 3f)]
    public float pitch = 1;

    public bool loop;
    public bool playOnAwake;

    [HideInInspector] public AudioSource source;
}
