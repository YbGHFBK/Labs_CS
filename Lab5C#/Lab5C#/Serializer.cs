using System.Xml.Serialization;

public static class Serializer
{
    public static void SerializeToFile<T>(T obj, string filePath)
    {
        var serializer = new XmlSerializer(typeof(T));
        using (var writer = new StreamWriter(filePath))
        {
            serializer.Serialize(writer, obj);
        }
    }

    public static T DeserializeFromFile<T>(string filePath)
    {
        var serializer = new XmlSerializer(typeof(T));
        using (var reader = new StreamReader(filePath))
        {
            return (T)serializer.Deserialize(reader);
        }
    }
}