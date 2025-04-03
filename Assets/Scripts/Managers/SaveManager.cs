using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class SaveManager : MonoBehaviour, ISaveManager
{
    private PlayerSave state;
    private string savePath;

    private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "save.xml");
        Load();
    }

    public void Save()
    {
        var serializedData = Serialize<PlayerSave>(state);
        File.WriteAllText(savePath, serializedData);
    }

    public void Load()
    {
        if (File.Exists(savePath))
        {
            var serializedData = File.ReadAllText(savePath);
            state = Deserialize<PlayerSave>(serializedData);
        }
        else
        {
            NewSave();
        }
    }

    public PlayerSave GetCurrentGameState()
    {
        return state;
    }

    public void UpdateGameState(PlayerSave newState)
    {
        state = newState;
    }

    private void NewSave()
    {
        state = new PlayerSave();
        Save();
    }

    private string Serialize<T>(T toSerialize)
    {
        var xml = new XmlSerializer(typeof(T));
        using var writer = new StringWriter();
        xml.Serialize(writer, toSerialize);
        return writer.ToString();
    }

    private T Deserialize<T>(string toDeserialize)
    {
        var xml = new XmlSerializer(typeof(T));
        using var reader = new StringReader(toDeserialize);
        return (T)xml.Deserialize(reader);
    }
}