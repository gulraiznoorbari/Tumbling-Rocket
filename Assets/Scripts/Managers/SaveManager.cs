using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
	public PlayerSave state;
	public static SaveManager instance;

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
		instance = this;
		Load();
	}

	public void Save()
	{
		PlayerPrefs.SetString("save", Serialize<PlayerSave>(state));
	}

	public void Load()
	{
		if (PlayerPrefs.HasKey("save"))
		{
			state = Deserialize<PlayerSave>(PlayerPrefs.GetString("save"));
		}
		else {
			NewSave();
		}
	}

	private void NewSave()
	{
		state = new PlayerSave();
		Save();
	}

	private string Serialize<T>(T toSerialize)
	{
		XmlSerializer xml = new XmlSerializer(typeof(T));
		StringWriter writer = new StringWriter();
		xml.Serialize(writer, toSerialize);
		return writer.ToString();
	}

	private T Deserialize<T>(string toDeserialize)
	{
		XmlSerializer xml = new XmlSerializer(typeof(T));
		var reader = new StringReader(toDeserialize);
		return (T)xml.Deserialize(reader);
	}
}
