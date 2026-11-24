using System.Collections.Generic;
using System.Linq;
using UniScape.Map;
using Xunit;

public class MapTests
{
    [Fact]
    public void Corridor_area_gives_576_tiles_ending_at_1240_680()
    {
        List<Placement> tiles = MapLayout.Expand(new Area("floor_corridor", 0, 0, 32, 18), 40, 40);
        Assert.Equal(576, tiles.Count);
        Assert.Equal(new Placement("floor_corridor", 0, 0), tiles.First());
        Assert.Equal(new Placement("floor_corridor", 1240, 680), tiles.Last());
    }

    [Fact]
    public void Area_is_expanded_row_by_row_left_to_right()
    {
        List<Placement> tiles = MapLayout.Expand(new Area("wall_top", 10, 20, 2, 2), 40, 40);
        Assert.Equal(new Placement("wall_top", 10, 20), tiles[0]);
        Assert.Equal(new Placement("wall_top", 50, 20), tiles[1]);
        Assert.Equal(new Placement("wall_top", 10, 60), tiles[2]);
    }

    [Fact]
    public void Sort_key_is_the_bottom_edge()
    {
        // desk.png is 60 px high
        Assert.Equal(340, MapLayout.SortKey(new Placement("desk", 80, 280), 60));
    }

    [Fact]
    public void Sort_is_by_bottom_edge_and_keeps_the_order_on_ties()
    {
        var input = new List<Placement>
        {
            new Placement("lockers", 40, 400),   // bottom 470
            new Placement("desk", 80, 196),      // bottom 256
            new Placement("lockers", 80, 400),   // bottom 470 (tie: stays after the first lockers)
        };
        var heights = new Dictionary<string, int> { ["lockers"] = 70, ["desk"] = 60 };

        List<Placement> sorted = MapLayout.SortByBottom(input, a => heights[a]);

        Assert.Equal(input[1], sorted[0]);
        Assert.Equal(input[0], sorted[1]);
        Assert.Equal(input[2], sorted[2]);
    }

    [Fact]
    public void Map_has_no_moving_things()
    {
        string[] movable = { "player", "guard", "key", "coffee", "camera" };
        IEnumerable<string> used = GroundFloorMap.Furniture.Select(p => p.Asset)
            .Concat(GroundFloorMap.WallObjects.Select(p => p.Asset))
            .Concat(GroundFloorMap.Effects.Select(p => p.Asset));
        Assert.DoesNotContain(used, movable.Contains);
    }
}
