namespace UniScape
{
    // Every fixed number of the prototype, in one place.
    public static class Constants
    {
        // Size of the game window, in pixels.
        public const int ScreenWidth = 1280;
        public const int ScreenHeight = 720;

        // Folder (next to the .exe) where the PNGs are copied at build time.
        public const string AssetsFolder = "Assets";

        // player.png is 160 x 56: 4 frames of 40 x 56, in the order down, left, right, up.
        public const int PlayerFrameWidth = 40;
        public const int PlayerFrameHeight = 56;

        // Walking speed of the student, in pixels per second.
        public const float PlayerSpeed = 150f;

        // floor_corridor.png is a 40 x 40 tile, repeated to fill the background.
        public const int TileSize = 40;
    }
}
