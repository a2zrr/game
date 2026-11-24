using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace UniScape.Data
{
    // One <asset> entry from assets.xml
    public record AssetInfo(string Name, string File, int Width, int Height, bool Solid);

    // Reads assets.xml with an XmlReader. It goes through the file node by node
    // and never loads the whole tree in memory like the DOM would.
    public static class AssetCatalogReader
    {
        public static List<AssetInfo> Read(string assetsXmlPath)
        {
            var assets = new List<AssetInfo>();
            using XmlReader reader = XmlReader.Create(assetsXmlPath);

            while (reader.Read())
            {
                if (reader.NodeType != XmlNodeType.Element || reader.LocalName != "asset")
                {
                    continue;
                }

                // solid is optional, the schema defaults it to false
                string solid = reader.GetAttribute("solid");
                assets.Add(new AssetInfo(
                    reader.GetAttribute("name"),
                    reader.GetAttribute("file"),
                    int.Parse(reader.GetAttribute("width"), CultureInfo.InvariantCulture),
                    int.Parse(reader.GetAttribute("height"), CultureInfo.InvariantCulture),
                    solid != null && XmlConvert.ToBoolean(solid)));
            }

            return assets;
        }
    }
}
