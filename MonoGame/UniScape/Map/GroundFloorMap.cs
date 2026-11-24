using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace UniScape.Map
{
    // The ground floor, copied by hand from the positions in floor.xml. The XML isn't loaded.
    // Only the decor that never moves is here, so no student, guards, key, coffee or camera.
    // Each list keeps the floor.xml order because the drawing order depends on it.
    public static class GroundFloorMap
    {
        // Where the student spawns, same as the player entry in floor.xml.
        // It's at the end of the corridor, next to the stairs.
        public static readonly Vector2 PlayerStart = new Vector2(1000, 476);

        public static readonly List<Area> Ground = new List<Area>
        {
            // corridor tiles everywhere, wood in the two classrooms
            new Area("floor_corridor", 0, 0, 32, 18),
            new Area("floor_classroom", 40, 0, 9, 11),
            new Area("floor_classroom", 840, 0, 10, 11),
        };

        public static readonly List<Placement> Effects = new List<Placement>
        {
            // shadows under the walls
            new Placement("fx_shadow_south", 40, 100),
            new Placement("fx_shadow_south", 440, 100),
            new Placement("fx_shadow_south", 840, 100),
            new Placement("fx_shadow_south", 40, 460),
            new Placement("fx_shadow_south", 800, 460),

            // shadows on the right of the side walls
            new Placement("fx_shadow_east", 40, 100),
            new Placement("fx_shadow_east", 440, 100),
            new Placement("fx_shadow_east", 840, 100),
            new Placement("fx_shadow_east", 40, 460),

            // moonlight through the six windows
            new Placement("fx_moonlight", 76, 100),
            new Placement("fx_moonlight", 316, 100),
            new Placement("fx_moonlight", 476, 100),
            new Placement("fx_moonlight", 716, 100),
            new Placement("fx_moonlight", 876, 100),
            new Placement("fx_moonlight", 1156, 100),

            // moonlight through the glass doors of the exit
            new Placement("fx_moonlight", 566, 100),
            new Placement("fx_moonlight", 616, 100),

            // warm light under the ceiling lamps
            new Placement("fx_lamp_pool", 120, 502),
            new Placement("fx_lamp_pool", 510, 510),
            new Placement("fx_lamp_pool", 920, 502),
            new Placement("fx_lamp_pool", 510, 190),
        };

        public static readonly List<Area> Walls = new List<Area>
        {
            // wall tops of the north wall, with the exit in the middle, and the south wall
            new Area("wall_top", 0, 0, 14, 1),
            new Area("wall_top", 680, 0, 15, 1),
            new Area("wall_top", 0, 680, 32, 1),

            // wall tops between the classrooms and the corridor
            new Area("wall_top", 0, 360, 5, 1),
            new Area("wall_top", 240, 360, 5, 1),
            new Area("wall_top", 800, 360, 5, 1),
            new Area("wall_top", 1040, 360, 6, 1),

            // wall tops of the side walls
            new Area("wall_top", 0, 40, 1, 8),
            new Area("wall_top", 400, 40, 1, 8),
            new Area("wall_top", 800, 40, 1, 8),
            new Area("wall_top", 1240, 40, 1, 8),
            new Area("wall_top", 0, 400, 1, 7),
            new Area("wall_top", 1240, 400, 1, 7),

            // back wall of classroom A
            new Area("wall_corner_inner_left", 40, 40, 1, 1),
            new Area("wall_window", 80, 40, 1, 1),
            new Area("wall_front", 120, 40, 5, 1),
            new Area("wall_window", 320, 40, 1, 1),
            new Area("wall_corner_inner_right", 360, 40, 1, 1),

            // back wall of the entrance hall, on both sides of the exit
            new Area("wall_corner_inner_left", 440, 40, 1, 1),
            new Area("wall_window", 480, 40, 1, 1),
            new Area("wall_front", 520, 40, 1, 1),
            new Area("wall_front", 680, 40, 1, 1),
            new Area("wall_window", 720, 40, 1, 1),
            new Area("wall_corner_inner_right", 760, 40, 1, 1),

            // back wall of classroom B
            new Area("wall_corner_inner_left", 840, 40, 1, 1),
            new Area("wall_window", 880, 40, 1, 1),
            new Area("wall_front", 920, 40, 6, 1),
            new Area("wall_window", 1160, 40, 1, 1),
            new Area("wall_corner_inner_right", 1200, 40, 1, 1),

            // corridor side of classroom A, with its door
            new Area("wall_corner_inner_left", 40, 400, 1, 1),
            new Area("wall_front", 80, 400, 2, 1),
            new Area("wall_corner_outer_right", 160, 400, 1, 1),
            new Area("wall_doorway", 200, 400, 1, 1),
            new Area("wall_corner_outer_left", 240, 400, 1, 1),
            new Area("wall_front", 280, 400, 3, 1),
            new Area("wall_corner_outer_right", 400, 400, 1, 1),

            // corridor side of classroom B, with its door
            new Area("wall_corner_outer_left", 800, 400, 1, 1),
            new Area("wall_front", 840, 400, 3, 1),
            new Area("wall_corner_outer_right", 960, 400, 1, 1),
            new Area("wall_doorway", 1000, 400, 1, 1),
            new Area("wall_corner_outer_left", 1040, 400, 1, 1),
            new Area("wall_front", 1080, 400, 3, 1),
            new Area("wall_corner_inner_right", 1200, 400, 1, 1),
        };

        public static readonly List<Placement> WallObjects = new List<Placement>
        {
            // one whiteboard per classroom, the camera is left out for now
            new Placement("board", 140, 50),
            new Placement("board", 960, 50),
        };

        // Furniture of the rooms and the corridor together, same order as floor.xml
        public static readonly List<Placement> Furniture = new List<Placement>
        {
            // Classroom A
            new Placement("teacher_desk", 160, 108),
            new Placement("desk", 80, 196),
            new Placement("desk", 80, 280),
            new Placement("desk", 240, 196),
            new Placement("desk", 240, 280),

            // Entrance hall
            new Placement("exit", 560, 0),
            new Placement("plant", 448, 104),
            new Placement("plant", 764, 104),
            new Placement("teacher_desk", 470, 250),
            new Placement("bench", 704, 290),

            // Classroom B
            new Placement("teacher_desk", 980, 108),
            new Placement("desk", 866, 196),
            new Placement("desk", 866, 280),
            new Placement("desk", 1000, 196),
            new Placement("desk", 1000, 280),
            new Placement("desk", 1134, 196),
            new Placement("desk", 1134, 280),

            // Corridor with the lockers, the break corner and the stairs
            new Placement("lockers", 40, 400),
            new Placement("lockers", 80, 400),
            new Placement("lockers", 120, 400),
            new Placement("lockers", 280, 400),
            new Placement("lockers", 320, 400),
            new Placement("lockers", 360, 400),
            new Placement("lockers", 840, 400),
            new Placement("lockers", 880, 400),
            new Placement("lockers", 920, 400),
            new Placement("vending_machine", 1086, 400),
            new Placement("bin", 1140, 444),
            new Placement("stairs", 1100, 532),
        };
    }
}
