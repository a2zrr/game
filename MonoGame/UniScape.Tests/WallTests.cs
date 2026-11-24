using System.Linq;
using Microsoft.Xna.Framework;
using UniScape;
using UniScape.Data;
using UniScape.Map;
using Xunit;

// Wall collisions on the real ground floor map.
// The feet box is 24 x 16, starting 8 px right and 40 px down from the frame's top-left corner.
public class WallTests
{
    private static Player OnGroundFloor(float x, float y)
    {
        return new Player(new Vector2(x, y), WallCollisions.GroundFloor);
    }

    // Walk for a number of seconds at 60 FPS like in the game
    private static void Walk(Player player, Vector2 move, float seconds)
    {
        for (int frame = 0; frame < seconds * 60; frame++)
        {
            player.Update(move, 1f / 60);
        }
    }

    private static bool FeetInAnyWall(Player player)
    {
        return WallCollisions.GroundFloor.Any(wall => Player.FeetTouch(player.Position, wall));
    }

    [Fact]
    public void The_back_wall_of_the_classroom_blocks_the_student()
    {
        // In classroom A the front of the back wall goes from y 40 to y 100
        var player = OnGroundFloor(200, 200);
        Walk(player, new Vector2(0, -1), 2f);
        Assert.Equal(60, player.Position.Y, 3); // feet top at 60 + 40 = 100, right against the wall
        Assert.Equal(200, player.Position.X, 3);
    }

    [Fact]
    public void The_student_walks_along_a_wall()
    {
        // Feet against the back wall, walking right should only stop at the side wall at x 400
        var player = OnGroundFloor(200, 60);
        Walk(player, new Vector2(1, 0), 2f);
        Assert.Equal(368, player.Position.X, 3); // feet right edge at 368 + 8 + 24 = 400
        Assert.Equal(60, player.Position.Y, 3);
    }

    [Fact]
    public void Walking_diagonally_into_a_wall_slides_along_it()
    {
        // In the corridor, going up-right into the wall of classroom B, whose bottom is at y 460.
        // Going up gets blocked but going right carries on until the east wall.
        var player = OnGroundFloor(1150, 500);
        Walk(player, new Vector2(1, -1), 3f);
        Assert.Equal(420, player.Position.Y, 3);  // feet top at 420 + 40 = 460
        Assert.Equal(1208, player.Position.X, 3); // feet right edge at 1208 + 32 = 1240
    }

    [Fact]
    public void Diagonal_movement_never_goes_inside_a_wall()
    {
        // Try all 4 diagonals from a few spots, the feet should never end up inside a wall
        Vector2[] starts = { new(200, 200), new(1150, 500), new(600, 300), new(1000, 476) };
        Vector2[] diagonals = { new(1, 1), new(1, -1), new(-1, 1), new(-1, -1) };
        foreach (Vector2 start in starts)
        {
            foreach (Vector2 direction in diagonals)
            {
                var player = OnGroundFloor(start.X, start.Y);
                for (int frame = 0; frame < 300; frame++)
                {
                    player.Update(direction, 1f / 60);
                    Assert.False(FeetInAnyWall(player), $"from {start} going {direction}, frame {frame}");
                }
            }
        }
    }

    [Fact]
    public void A_long_frame_cannot_jump_over_a_wall()
    {
        // A single 1 second frame is 150 px, way more than a wall is thick
        var player = OnGroundFloor(200, 200);
        player.Update(new Vector2(0, -1), 1f);
        Assert.Equal(60, player.Position.Y, 3);
    }

    [Fact]
    public void Doorways_and_furniture_do_not_block_the_student()
    {
        // Start in the corridor and go straight up through the classroom A doorway, x 200 to 240,
        // then over the teacher's desk until the back wall.
        var player = OnGroundFloor(200, 500);
        Walk(player, new Vector2(0, -1), 4f);
        Assert.Equal(60, player.Position.Y, 3);
    }

    [Fact]
    public void Lockers_and_the_vending_machine_are_walked_over()
    {
        // The lockers at x 840 to 960 and the vending machine at x 1086 are furniture,
        // so walking left along the corridor wall shouldn't be blocked by them.
        var player = OnGroundFloor(1150, 420);
        Walk(player, new Vector2(-1, 0), 2f);
        Assert.Equal(1150 - 300, player.Position.X, 3);
    }

    [Fact]
    public void Only_solid_wall_pieces_become_collision_rectangles()
    {
        var walls = WallCollisions.GroundFloor;
        // Both doorways are open
        Assert.DoesNotContain(new Rectangle(200, 400, 40, 60), walls);
        Assert.DoesNotContain(new Rectangle(1000, 400, 40, 60), walls);
        // and some real walls are in the list
        Assert.Contains(new Rectangle(0, 0, 40, 40), walls);       // wall_top
        Assert.Contains(new Rectangle(120, 40, 40, 60), walls);    // wall_front
        Assert.Contains(new Rectangle(160, 400, 40, 60), walls);   // corner next to the door
        // No furniture, wall objects or effects in there, every rectangle is a wall tile
        Assert.All(walls, w => Assert.True(w.Width == 40 && (w.Height == 40 || w.Height == 60)));
    }

    [Fact]
    public void The_start_position_is_not_inside_a_wall()
    {
        Assert.False(FeetInAnyWall(OnGroundFloor(GroundFloorMap.PlayerStart.X, GroundFloorMap.PlayerStart.Y)));
    }

    [Fact]
    public void Solid_walls_match_assets_xml()
    {
        // Read assets.xml with the XmlReader and compare it with WallCollisions
        var wallAssets = AssetCatalogReader.Read(RepoFiles.Xml("assets.xml"))
                                           .Where(a => a.Name.StartsWith("wall_"))
                                           .ToList();
        Assert.Equal(8, wallAssets.Count);
        foreach (AssetInfo asset in wallAssets)
        {
            Assert.Equal(asset.Solid, WallCollisions.IsSolid(asset.Name));
            Assert.Equal(new Point(asset.Width, asset.Height), WallCollisions.SizeOf(asset.Name));
        }
    }
}
