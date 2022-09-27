using UnityEngine;

[System.Serializable]
public class BackgroundSong
{
	public string name;

	public AudioClip clip;

	[Range(0f, 1f)]
	public float volume;

	[Range(0f, 2f)]
	public float pitch;

	public bool loop = false;
	public bool playOnAwake = true;
	public bool mute = false;

	[HideInInspector]
	public AudioSource source;
}
