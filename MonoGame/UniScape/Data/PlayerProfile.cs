using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace UniScape.Data
{
    // How a game ended
    public enum GameResult
    {
        Escaped, // the student reached the exit
        Caught,  // caught by a guard or a camera
        Quit,    // the player quit before the end
    }

    // One game the player played
    public class PlayedGame
    {
        // Read-write properties, these are the ones the serializer saves and loads
        [XmlAttribute("date")] public DateTime Date { get; set; }
        [XmlAttribute("score")] public int Score { get; set; }
        [XmlAttribute("result")] public GameResult Result { get; set; } // enum, the XML gets the name of the value
        [XmlAttribute("floor")] public int Floor { get; set; }

        // Read-only, worked out from the other values so it's never written to the XML
        [XmlIgnore] public bool Won => Result == GameResult.Escaped;
    }

    // The human player's profile with the list of their games
    [XmlRoot("profile")]
    public class PlayerProfile
    {
        [XmlAttribute("name")] public string Name { get; set; }

        // Each game in the list becomes a <game> element inside <games>
        [XmlArray("games"), XmlArrayItem("game")]
        public List<PlayedGame> Games { get; set; } = new List<PlayedGame>();

        // Read-only stuff, recomputed after loading. XmlSerializer only deals with public
        // read-write members anyway, XmlIgnore just makes it clear these aren't saved.
        [XmlIgnore] public int GamesPlayed => Games.Count;
        [XmlIgnore] public int BestScore => Games.Count == 0 ? 0 : Games.Max(g => g.Score);
        [XmlIgnore] public IEnumerable<PlayedGame> BestGames => Games.OrderByDescending(g => g.Score);
    }
}
