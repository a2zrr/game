namespace UniScape.Map
{
    // One image placed on the map, X and Y are its top-left corner in pixels
    public record Placement(string Asset, int X, int Y);

    // One image repeated Columns times to the right and Rows times down, starting at X, Y.
    // Each step is the size of the image itself. A single wall is just a 1 x 1 area.
    public record Area(string Asset, int X, int Y, int Columns, int Rows);
}
