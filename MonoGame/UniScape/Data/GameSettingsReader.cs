using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace UniScape.Data
{
    // Everything we read from game.xml
    public record GameSettings(
        IReadOnlyList<string> FloorFiles,
        int PlayerSpeed, int CoffeeBoost, int CoffeeTime,
        int GuardSpeed, int GuardVisionRange, double GuardVisionAngle, int GuardSuspicionTime, int GuardTouchDistance,
        int CameraVisionRange, double CameraVisionAngle, int CameraSuspicionTime);

    // Reads game.xml with the DOM and XPath. XmlDocument loads the whole tree in memory
    // and then we just query the values we need.
    public static class GameSettingsReader
    {
        public const string Namespace = "http://www.univ-grenoble-alpes.fr/l3miage/game";

        public static GameSettings Read(string gameXmlPath)
        {
            var document = new XmlDocument();
            document.Load(gameXmlPath);

            // game.xml has a default namespace, so XPath only finds the nodes if we give it a prefix, here g
            var ns = new XmlNamespaceManager(document.NameTable);
            ns.AddNamespace("g", Namespace);

            var floors = new List<string>();
            foreach (XmlNode file in document.SelectNodes("/g:game/g:floors/g:floor/@file", ns))
            {
                floors.Add(file.Value);
            }

            int Int(string xpath) => int.Parse(Text(document, ns, xpath), CultureInfo.InvariantCulture);
            double Number(string xpath) => double.Parse(Text(document, ns, xpath), CultureInfo.InvariantCulture);

            return new GameSettings(
                floors,
                Int("/g:game/g:player/@speed"),
                Int("/g:game/g:player/@coffeeBoost"),
                Int("/g:game/g:player/@coffeeTime"),
                Int("/g:game/g:guard/@speed"),
                Int("/g:game/g:guard/@visionRange"),
                Number("/g:game/g:guard/@visionAngle"),
                Int("/g:game/g:guard/@suspicionTime"),
                Int("/g:game/g:guard/@touchDistance"),
                Int("/g:game/g:camera/@visionRange"),
                Number("/g:game/g:camera/@visionAngle"),
                Int("/g:game/g:camera/@suspicionTime"));
        }

        private static string Text(XmlDocument document, XmlNamespaceManager ns, string xpath)
        {
            XmlNode node = document.SelectSingleNode(xpath, ns)
                ?? throw new XmlException($"game.xml: nothing found at {xpath}");
            return node.Value;
        }
    }
}
