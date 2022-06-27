using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource musicSource, sfxSource;

    Dictionary<ESfxType,AudioClip> clips=new Dictionary<ESfxType, AudioClip>();

    private void Awake()
    {
        if (Core.Audio == null)
        {
            Core.Audio = this;
            DontDestroyOnLoad(gameObject);
            AnimationManager.Initialize();
        }
        else
            Destroy(gameObject);

    }

    public float MusicVolume
    {
        get { return musicSource.volume; }
        set
        {
            musicSource.volume = value;
        }
    }

    public float SfxVolume
    {
        get { return sfxSource.volume; }
        set
        {
            sfxSource.volume = value;
        }
    }


    private void Start()
    {
        musicSource = transform.Find("Music").GetComponent<AudioSource>();
        sfxSource = transform.Find("Effects").GetComponent<AudioSource>();
        foreach (AudioClip a in Resources.LoadAll("Sfx", typeof(AudioClip)))
        {
            Enum.TryParse(a.name,out ESfxType type);
            if (type != ESfxType.none && !clips.ContainsKey(type))
                clips.Add(type, a);
        }
    }
    
    public void Play(ESfxType sfx,int multiplier = 0)
    {
        if (!clips.ContainsKey(sfx))
            return;
        sfxSource.clip= clips[sfx];
        sfxSource.pitch=Mathf.Pow(1.2f,multiplier);
        sfxSource.Play();
    }
}