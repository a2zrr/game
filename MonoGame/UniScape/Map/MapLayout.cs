using System;
using System.Collections.Generic;
using System.Linq;

namespace UniScape.Map
{
    // Helpers that turn the map pieces into the list of images we actually draw
    public static class MapLayout
    {
        // Splits an area into its tiles, row by row from left to right
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

        // The bottom edge of an object, where it touches the floor
        public static int SortKey(Placement placement, int height)
        {
            return placement.Y + height;
        }

        // Furniture is drawn from top to bottom so things lower on the screen cover what's behind them.
        // OrderBy is a stable sort, so two objects with the same key stay in the floor.xml order.
        // List.Sort doesn't guarantee that.
        public static List<Placement> SortByBottom(IEnumerable<Placement> placements, Func<string, int> heightOf)
        {
            return placements.OrderBy(p => SortKey(p, heightOf(p.Asset))).ToList();
        }
    }
}
