using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;

	public SoundEffects[] _soundEffects;
	public BackgroundSong _backgroundSong;
	public bool _muted;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);

		_backgroundSong.source = gameObject.AddComponent<AudioSource>();
		_backgroundSong.source.clip = _backgroundSong.clip;
		_backgroundSong.source.loop = _backgroundSong.loop;

		foreach (SoundEffects s in _soundEffects)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;
		}
	}

	private void Update()
	{
		foreach (SoundEffects s in _soundEffects)
		{
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.mute = s.mute;
		}
		_backgroundSong.source.volume = _backgroundSong.volume;
		_backgroundSong.source.pitch = _backgroundSong.pitch;
	}

	public void PlaySFX(string name)
	{
		SoundEffects s = Array.Find(_soundEffects, sound => sound.name == name);

		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.Play();
	}

	public void PlayBGSong()
	{
		_backgroundSong.source.Play();
	}

	public void StopSFX(string name)
	{
		SoundEffects s = Array.Find(_soundEffects, sound => sound.name == name);

		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.Stop();
	}

	public void StopBGSong()
	{
		_backgroundSong.source.Stop();
	}

	public void ToggleSFX()
	{
		foreach (SoundEffects s in _soundEffects)
		{
			s.source.mute = !s.source.mute;
			s.mute = s.source.mute;
		}
	}

	public void ToggleBGSong()
	{
		_backgroundSong.source.mute = !_backgroundSong.source.mute;
		_backgroundSong.mute = _backgroundSong.source.mute;
	}
}