using System.Runtime.Serialization;
using System.Xml.Serialization;

public static class FileWorker
{
    public static void SerializeToFile<T>(T obj, string filePath)
    {
        var namespaces = new XmlSerializerNamespaces();
        namespaces.Add("", "");

        var serializer = new XmlSerializer(typeof(T));
        using (var writer = new StreamWriter(filePath))
        {
            serializer.Serialize(writer, obj, namespaces);
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

    public static string[] ReadFile(string path)
    {
        try
        {
            string[] lines = File.ReadAllLines(path);
            return lines;
        }
        catch (Exception e)
        {
            Console.WriteLine("Ошибка чтения файла");
            throw;
        }
    }
}