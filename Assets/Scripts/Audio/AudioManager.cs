using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour, IAudioManager
{
    [SerializeField] private SoundEffects[] _soundEffects;
    [SerializeField] private BackgroundSong _backgroundSong;
    [SerializeField] private bool _muted = false;

    private Dictionary<string, SoundEffects> _soundEffectsDict;

    private void Awake()
    {
        InitializeAudioSources();
    }

    private void InitializeAudioSources()
    {
        if (_backgroundSong != null && _backgroundSong.clip != null)
        {
            _backgroundSong.source = gameObject.AddComponent<AudioSource>();
            _backgroundSong.source.clip = _backgroundSong.clip;
            _backgroundSong.source.loop = _backgroundSong.loop;
            _backgroundSong.source.volume = _backgroundSong.volume;
            _backgroundSong.source.pitch = _backgroundSong.pitch;
        }

        _soundEffectsDict = new Dictionary<string, SoundEffects>();

        foreach (var soundEffect in _soundEffects)
        {
            if (soundEffect.clip == null) continue;

            soundEffect.source = gameObject.AddComponent<AudioSource>();
            soundEffect.source.clip = soundEffect.clip;
            soundEffect.source.loop = soundEffect.loop;
            soundEffect.source.volume = soundEffect.volume;
            soundEffect.source.pitch = soundEffect.pitch;
            soundEffect.source.mute = soundEffect.mute;

            _soundEffectsDict[soundEffect.name] = soundEffect;
        }
    }

    public void PlaySFX(string name)
    {
        if (_soundEffectsDict.TryGetValue(name, out SoundEffects s))
        {
            s.source.PlayOneShot(s.source.clip);
            _muted = false;
        }
    }

    public void StopSFX(string name)
    {
        if (_soundEffectsDict.TryGetValue(name, out SoundEffects s))
        {
            s.source.Stop();
        }
    }

    public void PlayBGSong()
    {
        if (_backgroundSong?.source != null)
        {
            _backgroundSong.source.Play();
            _muted = false;
        }
    }

    public void StopBGSong()
    {
        _backgroundSong?.source?.Stop();
    }

    public void ToggleSFX()
    {
        _muted = !_muted;

        foreach (var s in _soundEffectsDict.Values)
        {
            s.source.mute = _muted;
        }
    }

    public void ToggleBGSong()
    {
        if (_backgroundSong?.source != null)
        {
            _muted = !_muted;
            _backgroundSong.source.mute = _muted;
        }
    }
}
