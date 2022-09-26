using System;
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
		}
		else
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;
		}
	}

	private void Start()
	{
		Play("Theme");
	}

	private void Update()
	{
		foreach (Sound s in sounds)
		{
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.mute = s.mute;
		}

	}

	public void Play(string name)
	{
		Sound s = Array.Find(sounds, sound => sound.name == name);

		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.Play();
	}

	public void StopPlaying(string name)
	{
		Sound s = Array.Find(sounds, sound => sound.name == name);

		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.Stop();
	}

	public void ToggleSound()
	{
		foreach (Sound s in sounds)
		{
			s.source.mute = !s.source.mute;
			s.mute = s.source.mute;
		}
	}
}