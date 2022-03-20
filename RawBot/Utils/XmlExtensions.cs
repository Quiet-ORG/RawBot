using System.IO;
using System.Xml.Serialization;

namespace RawBot.Utils
{
    public static class XmlExtensions
    {
        public static T Parse<T>(this Stream stream)
        {
            var xml = new XmlSerializer(typeof(T));
            return (T)xml.Deserialize(stream);
        }

        public static T Parse<T>(this string content)
        {
            var xml = new XmlSerializer(typeof(T));
            using var reader = new StringReader(content);
            return (T)xml.Deserialize(reader);
        }

        public static string ToXml<T>(this T obj)
        {
            var xml = new XmlSerializer(typeof(T));
            using var writer = new StringWriter();
            xml.Serialize(writer, obj);
            return writer.ToString();
        }
    }
}