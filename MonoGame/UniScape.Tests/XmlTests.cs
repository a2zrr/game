using System;
using System.IO;
using System.Linq;
using UniScape.Data;
using Xunit;

// Finds the xml and xsd folders of the repo by going up from the test bin folder.
// The tests only read those files.
public static class RepoFiles
{
    private static readonly string Root = FindRoot();

    public static string Xml(string name) => Path.Combine(Root, "xml", name);
    public static string Xsd(string name) => Path.Combine(Root, "xsd", name);

    private static string FindRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null && !(Directory.Exists(Path.Combine(dir.FullName, "xml"))
                                && Directory.Exists(Path.Combine(dir.FullName, "xsd"))))
        {
            dir = dir.Parent;
        }
        return dir?.FullName ?? throw new DirectoryNotFoundException("xml/ and xsd/ folders not found");
    }
}

// This is the C# program the project PDF asks for to check the XML documents.
// Each file is validated against its schema with XmlValidator.
public class XmlValidationTests
{
    [Theory]
    [InlineData("game.xml", "game.xsd")]
    [InlineData("assets.xml", "assets.xsd")]
    public void Xml_file_is_valid_against_its_schema(string xml, string xsd)
    {
        var errors = XmlValidator.Validate(RepoFiles.Xml(xml), RepoFiles.Xsd(xsd));
        Assert.True(errors.Count == 0, string.Join(Environment.NewLine, errors));
    }

    [Fact]
    public void Validator_finds_schema_errors()
    {
        // Built in memory, it has an unknown asset name and no height attribute
        const string bad =
            "<assets xmlns='http://www.univ-grenoble-alpes.fr/l3miage/game'>" +
            "<asset name='dragon' file='dragon.png' width='10'/></assets>";
        var errors = XmlValidator.Validate(new StringReader(bad), RepoFiles.Xsd("assets.xsd"));
        Assert.True(errors.Count >= 2, string.Join(Environment.NewLine, errors));
    }

    [Fact]
    public void Validator_reports_a_document_that_is_not_well_formed()
    {
        var errors = XmlValidator.Validate(new StringReader("<assets>"), RepoFiles.Xsd("assets.xsd"));
        Assert.Contains(errors, e => e.StartsWith("not well-formed"));
    }
}

// DOM and XPath on game.xml
public class GameSettingsReaderTests
{
    [Fact]
    public void Reads_every_value_of_game_xml()
    {
        GameSettings s = GameSettingsReader.Read(RepoFiles.Xml("game.xml"));

        Assert.Equal(new[] { "floor_1.xml", "floor_0.xml" }, s.FloorFiles);
        Assert.Equal((1, 150, 5), (s.PlayerSpeed, s.CoffeeBoost, s.CoffeeTime));
        Assert.Equal((1, 220, 30.0, 600, 25),
            (s.GuardSpeed, s.GuardVisionRange, s.GuardVisionAngle, s.GuardSuspicionTime, s.GuardTouchDistance));
        Assert.Equal((220, 30.0, 1000), (s.CameraVisionRange, s.CameraVisionAngle, s.CameraSuspicionTime));
    }
}

// XmlReader on assets.xml
public class AssetCatalogReaderTests
{
    [Fact]
    public void Reads_the_32_assets_with_their_size_and_solid_flag()
    {
        var assets = AssetCatalogReader.Read(RepoFiles.Xml("assets.xml"));

        Assert.Equal(32, assets.Count);
        AssetInfo player = assets.Single(a => a.Name == "player");
        Assert.Equal(("player.png", 160, 56, false), (player.File, player.Width, player.Height, player.Solid));
        Assert.True(assets.Single(a => a.Name == "wall_top").Solid);
        Assert.False(assets.Single(a => a.Name == "wall_doorway").Solid); // no solid attribute so it defaults to false
    }
}

// XmlSerializer on a profile that has a list, an enum, read-write and read-only data.
// Everything stays in memory.
public class ProfileSerializerTests
{
    private static PlayerProfile Sample() => new PlayerProfile
    {
        Name = "Alice",
        Games =
        {
            new PlayedGame { Date = new DateTime(2026, 11, 20, 14, 30, 0), Score = 1200, Result = GameResult.Escaped, Floor = 1 },
            new PlayedGame { Date = new DateTime(2026, 11, 21, 9, 5, 0), Score = 300, Result = GameResult.Caught, Floor = 0 },
        },
    };

    private static string ToXml(PlayerProfile profile)
    {
        var writer = new StringWriter();
        ProfileSerializer.Write(profile, writer);
        return writer.ToString();
    }

    [Fact]
    public void Writes_the_list_and_the_enums_but_not_the_read_only_data()
    {
        string xml = ToXml(Sample());

        Assert.Contains("<profile", xml);
        Assert.Contains("name=\"Alice\"", xml);
        Assert.Equal(2, xml.Split("<game ").Length - 1);   // one <game> per game in the list
        Assert.Contains("result=\"Caught\"", xml);          // the enum is saved by name
        Assert.DoesNotContain("BestScore", xml);            // read-only values aren't saved
        Assert.DoesNotContain("Won", xml);
    }

    [Fact]
    public void Reading_back_gives_the_same_profile()
    {
        PlayerProfile copy = ProfileSerializer.Read(new StringReader(ToXml(Sample())));

        Assert.Equal("Alice", copy.Name);
        Assert.Equal(2, copy.GamesPlayed);
        Assert.Equal(GameResult.Escaped, copy.Games[0].Result);
        Assert.Equal(new DateTime(2026, 11, 21, 9, 5, 0), copy.Games[1].Date);
        Assert.Equal(1200, copy.BestScore);                  // recomputed after loading
        Assert.True(copy.Games[0].Won);
        Assert.Equal(new[] { 1200, 300 }, copy.BestGames.Select(g => g.Score));
    }
}
