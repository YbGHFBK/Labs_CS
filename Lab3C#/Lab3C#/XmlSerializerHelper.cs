using System.IO;
using System.Xml;
using System.Xml.Serialization;

public static class XmlSerializerHelper
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

    public static string DeserializeFromFile1(string filePath)
    {
        var serializer = new XmlSerializer(typeof(Text));
        using (var reader = new StreamReader(filePath))
        {
            Text text = (Text)serializer.Deserialize(reader);
            string sent = null;

            for( int i = 0; i < text.Sentences[0].Words.Count; i++)
            {
                if (text.Sentences[0].Words[i] is Word word)
                {
                    sent += word.Letters;
                }
                else if (text.Sentences[0].Words[i] is Punctuation punc)
                {
                    sent += punc.getPunc();
                }
            }

            return sent;
            
        }
    }
}