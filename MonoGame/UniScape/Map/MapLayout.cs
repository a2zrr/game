using System;
using System.Collections.Generic;
using System.Linq;

namespace UniScape.Map
{
    // Small helpers that turn the map pieces into a list of pictures to draw.
    public static class MapLayout
    {
        // Turns an area into its tiles, row by row, left to right.
        public static List<Placement> Expand(Area area, int tileWidth, int tileHeight)
        {
            var tiles = new List<Placement>();
            for (int row = 0; row < area.Rows; row++)
            {
                for (int column = 0; column < area.Columns; column++)
                {
                    tiles.Add(new Placement(area.Asset, area.X + column * tileWidth, area.Y + row * tileHeight));
                }
            }
            return tiles;
        }

        // Where an object "touches the floor": its bottom edge (y + height).
        public static int SortKey(Placement placement, int height)
        {
            return placement.Y + height;
        }

        // Furniture is drawn from the top of the screen to the bottom, so an object lower
        // on the screen covers the one behind it. OrderBy is a *stable* sort: two objects with
        // the same key keep the order of floor.xml (List.Sort would not guarantee that).
        public static List<Placement> SortByBottom(IEnumerable<Placement> placements, Func<string, int> heightOf)
        {
            return placements.OrderBy(p => SortKey(p, heightOf(p.Asset))).ToList();
        }
    }
}
