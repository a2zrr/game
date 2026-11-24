namespace UniScape.Map
{
    // One picture placed on the map. X, Y = top-left corner in pixels.
    public record Placement(string Asset, int X, int Y);

    // A picture repeated in a rectangle: "Columns" times to the right and "Rows" times down,
    // starting at (X, Y). The step is the picture's own width and height.
    // A single wall is simply an area of 1 x 1.
    public record Area(string Asset, int X, int Y, int Columns, int Rows);
}
