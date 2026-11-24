using System.IO;
using System.Xml.Serialization;

namespace UniScape.Data
{
    // Converts a PlayerProfile to XML text and back using XmlSerializer.
    // It takes a TextWriter or TextReader so the caller picks where the XML goes.
    // For now only the tests use it, with strings in memory, so no file gets created.
    public static class ProfileSerializer
    {
        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(PlayerProfile));

        public static void Write(PlayerProfile profile, TextWriter writer)
        {
            Serializer.Serialize(writer, profile);
        }

        public static PlayerProfile Read(TextReader reader)
        {
            return (PlayerProfile)Serializer.Deserialize(reader);
        }
    }
}
