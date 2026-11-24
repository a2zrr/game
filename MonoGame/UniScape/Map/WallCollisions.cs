using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace UniScape.Map
{
    // Says which wall pieces block the student and where they are.
    // Only the walls section of the map is used. Furniture, wall objects, guards, cameras
    // and items never block, even if assets.xml marks some of them as solid.
    public static class WallCollisions
    {
        // Same as assets.xml, where every wall piece is solid except wall_doorway, the open door.
        // There's a test that reads assets.xml to make sure this list doesn't drift.
        private static readonly HashSet<string> SolidWalls = new HashSet<string>
        {
            "wall_top",
            "wall_front",
            "wall_window",
            "wall_corner_inner_left",
            "wall_corner_inner_right",
            "wall_corner_outer_left",
            "wall_corner_outer_right",
        };

        // All the blocking rectangles of the ground floor, built once
        public static readonly List<Rectangle> GroundFloor = SolidRectangles(GroundFloorMap.Walls);

        public static bool IsSolid(string wallAsset)
        {
            return SolidWalls.Contains(wallAsset);
        }

        // Size of a wall piece in pixels, same as the PNGs and assets.xml.
        // Hardcoded so the collisions also work in unit tests where no texture is loaded.
        public static Point SizeOf(string wallAsset)
        {
            return wallAsset == "wall_top" ? new Point(40, 40) : new Point(40, 60);
        }

        // One rectangle per solid wall tile. Areas are cut into tiles the same way as when drawing.
        public static List<Rectangle> SolidRectangles(IEnumerable<Area> walls)
        {
            var rectangles = new List<Rectangle>();
            foreach (Area area in walls)
            {
                if (!IsSolid(area.Asset))
                {
                    continue; // doorway, the student can walk through
                }

                Point size = SizeOf(area.Asset);
                foreach (Placement tile in MapLayout.Expand(area, size.X, size.Y))
                {
                    rectangles.Add(new Rectangle(tile.X, tile.Y, size.X, size.Y));
                }
            }
            return rectangles;
        }
    }
}
