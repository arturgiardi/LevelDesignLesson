using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveManager
{
    string folderPath = "/SaveData";
    string fileFormat = "sv";

    public void Save(string fileName, params ISavableData[] savebleData)
    {
        SaveData data = new SaveData();
        foreach (var item in savebleData)
        {
            item.Save(data);
        }

        string file = GetFile(fileName);

        BinaryFormatter formatter = new BinaryFormatter();

        using (FileStream stream = new FileStream(file, FileMode.Create))
            formatter.Serialize(stream, data);
    }

    public void Load(string fileName, params ISavableData[] savebleData)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string file = GetFile(fileName);

        if (File.Exists(file))
        {
            SaveData data = null;

            using (FileStream stream = new FileStream(file, FileMode.Open))
                data = (SaveData)formatter.Deserialize(stream);

            foreach (var item in savebleData)
            {
                item.Load(data);
            }
        }
    }

    public void DeleteSave(string fileName)
    {
        string file = GetFile(fileName);
        File.Delete(file);
    }

    private string GetFile(string fileName)
    {
        if (!folderPath.EndsWith("/"))
            folderPath = $"{folderPath}/";
        if (!fileFormat.StartsWith("."))
            fileFormat = $".{fileFormat}";

        string completeFolderPath = $"{Application.persistentDataPath}{folderPath}";

        if (!Directory.Exists(completeFolderPath))
            Directory.CreateDirectory(completeFolderPath);
        return $"{completeFolderPath}{fileName}{fileFormat}";
    }
}
