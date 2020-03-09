using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AudioManager : SingletonBase<AudioManager>
{
    public AudioSource BattleAudioSources;
    public string[] BattleAudioNames;
    public float MaxBattleAudioVolume;

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

    private void Update()
    {
        UpdateBattingAudio();
    }

    void UpdateBattingAudio()
    {
        // 모든 공격에 각각 소리가 나면 시끄럽기 때문에 하나의 유닛이라도 공격중이면 전투 사운드를 재생
        if (IsExistAttackingUnit())
        {
            if (BattleAudioSources.isPlaying == false)
            {
                int index = UnityEngine.Random.Range(0, BattleAudioNames.Length);
                string battleAudioName = BattleAudioNames[index];
                BattleAudioSources.clip = GetAudioClip(battleAudioName);
                BattleAudioSources.Play();
            }
        }
        else
        {
            BattleAudioSources.Stop();
        }
    }

    AudioClip GetAudioClip(string audioName)
    {
        AudioClip audioClip;
        if (_audioClips.ContainsKey(audioName))
        {
            audioClip = _audioClips[audioName];
        }
        else
        {
            audioClip = Resources.Load<AudioClip>("Audios/" + audioName);
            _audioClips.Add(audioName, audioClip);
        }

        return audioClip;
    }

    bool IsExistAttackingUnit()
    {
        foreach (var item in UnitPoolManager.Inst.ActiveUnitsPool)
        {
            var units = item.Value;
            foreach (var unit in units)
            {
                if (unit.HasEnemyUnit() && unit.UnitFSM.CurUnitState.UnitFSMType == Types.UnitFSMType.Attack)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void PlayAttackStart(AttackData attackData)
    {
        Play(Types.AudioChannelType.Effect, attackData.StartAudioName, attackData.AudioVolume);
    }

    public void PlayAttackFire(AttackData attackData)
    {
        Play(Types.AudioChannelType.Effect, attackData.FireAudioName, attackData.AudioVolume);
    }

    public void PlayAttackHit(AttackData attackData)
    {
        Play(Types.AudioChannelType.Effect, attackData.HitAudioName, attackData.AudioVolume);
    }

    public void Play(Types.AudioChannelType audioChannelType, string audioName, float volume = 1.0f)
    {
        if (audioChannelType == Types.AudioChannelType.None)
        {
            return;
        }

        AudioSource audioSource = _audioSources[(int)audioChannelType];
        AudioClip audioClip = GetAudioClip(audioName);
        audioSource.PlayOneShot(audioClip, volume);
    }

    public bool IsPlaying(Types.AudioChannelType audioChannelType)
    {
        if (audioChannelType == Types.AudioChannelType.None)
        {
            return false;
        }

        AudioSource audioSource = _audioSources[(int)audioChannelType];
        return audioSource.isPlaying;
    }

    public void Stop(Types.AudioChannelType audioChannelType)
    {
        if (audioChannelType == Types.AudioChannelType.None)
        {
            return;
        }

        AudioSource audioSource = _audioSources[(int)audioChannelType];
        audioSource.Stop();
    }

    public void SetVolume(Types.AudioChannelType audioChannelType, float volume)
    {
        AudioSource audioSource = _audioSources[(int)audioChannelType];
        audioSource.volume = volume;
    }
}
