namespace UniScape
{
    // All the fixed numbers of the game, kept in one place so they're easy to tweak
    public static class Constants
    {
        // Window size in pixels
        public const int ScreenWidth = 1280;
        public const int ScreenHeight = 720;

        // The PNGs get copied into this folder next to the .exe when we build
        public const string AssetsFolder = "Assets";

        // player.png is 160x56, so 4 frames of 40x56 in the order down, left, right, up
        public const int PlayerFrameWidth = 40;
        public const int PlayerFrameHeight = 56;

        // How fast the student walks, in pixels per second
        public const float PlayerSpeed = 150f;

        // Only the feet collide with walls, not the whole sprite.
        // The map is seen from above at an angle, so it's fine if the head overlaps a wall.
        // 24 px wide so the student gets through a 40 px doorway without lining up perfectly.
        public const int PlayerFeetOffsetX = 8;
        public const int PlayerFeetOffsetY = 40;
        public const int PlayerFeetWidth = 24;
        public const int PlayerFeetHeight = 16;

        // Size of a floor tile. Not used anymore, the map now takes its sizes from the PNGs
        public const int TileSize = 40;
    }
}
