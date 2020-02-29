using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AudioManager : SingletonBase<AudioManager>
{
    Dictionary<string, AudioClip> _audioClips;
    AudioSource[] _audioSources;

    protected override void Awake()
    {
        base.Awake();
        _audioClips = new Dictionary<string, AudioClip>();

        int audioChannelCount = Enum.GetNames(typeof(Types.AudioChannelType)).Length;
        _audioSources = new AudioSource[audioChannelCount];
        for (int i = 0; i < audioChannelCount; i++)
        {
            _audioSources[i] = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlayAttackStart(AttackData attackData)
    {
        Play(attackData.StartAudioChannelType, attackData.StartAudioName, attackData.AudioVolume);
    }

    public void PlayAttackFire(AttackData attackData)
    {
        Play(attackData.FireAudioChannelType, attackData.FireAudioName, attackData.AudioVolume);
    }

    public void PlayAttackHit(AttackData attackData)
    {
        Play(attackData.HitAudioChannelType, attackData.HitAudioName, attackData.AudioVolume);
    }

    public void Play(Types.AudioChannelType audioChannelType, string audioName, float volume = 1.0f)
    {
        if (audioChannelType == Types.AudioChannelType.None)
        {
            return;
        }

        AudioSource audioSource = _audioSources[(int)audioChannelType];
        AudioClip audioClip;
        if (_audioClips.ContainsKey(audioName))
        {
            audioClip = _audioClips[audioName];
        }
        else
        {
            audioClip = Resources.Load<AudioClip>("Audios/" + audioName);
        }
        audioSource.PlayOneShot(audioClip, volume);
    }
}
