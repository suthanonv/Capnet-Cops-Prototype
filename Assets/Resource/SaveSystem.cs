using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{

    public static void SaveResource(ResourceManagement resource)
    {

        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/resource.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        ResourcesData data = new ResourcesData(resource);
        
        formatter.Serialize(stream, data);
        stream.Close();

    }

    public static ResourcesData LoadResource()
    {
        string path = Application.persistentDataPath + "/resource.data";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            
            ResourcesData data = formatter.Deserialize(stream) as ResourcesData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("Save file not found in " + path);
            return null;
        }
    }
    
}
