using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] sounds;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
            s.source.pitch = s.pitch;
        }
    }

    public void Play(String sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        s.source.Play();
    }

    internal void Mute()
    {
        foreach (Sound s in sounds)
        {
            s.source.mute = true;
        }
    }

    internal void UnMute()
    {
        foreach (Sound s in sounds)
        {
            s.source.mute = false;
        }
    }
}
