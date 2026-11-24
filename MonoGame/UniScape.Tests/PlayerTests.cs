using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using UniScape;
using Xunit;

public class PlayerTests
{
    private static readonly Vector2 Middle = new Vector2(600, 300);

    [Fact]
    public void Frames_are_in_the_order_down_left_right_up()
    {
        Assert.Equal(new Rectangle(0, 0, 40, 56), Player.FrameFor(Facing.Down));
        Assert.Equal(new Rectangle(40, 0, 40, 56), Player.FrameFor(Facing.Left));
        Assert.Equal(new Rectangle(80, 0, 40, 56), Player.FrameFor(Facing.Right));
        Assert.Equal(new Rectangle(120, 0, 40, 56), Player.FrameFor(Facing.Up));
    }

    [Theory]
    [InlineData(1, 0, Facing.Right)]
    [InlineData(-1, 0, Facing.Left)]
    [InlineData(0, 1, Facing.Down)]
    [InlineData(0, -1, Facing.Up)]
    [InlineData(1, 1, Facing.Right)]   // a perfect diagonal goes to left/right
    [InlineData(-1, -1, Facing.Left)]
    public void Facing_follows_the_dominant_axis(float x, float y, Facing expected)
    {
        Assert.Equal(expected, Player.FacingFor(new Vector2(x, y)));
    }

    [Fact]
    public void Walks_at_its_speed_for_one_second()
    {
        var player = new Player(Middle);
        player.Update(new Vector2(1, 0), 1f);
        Assert.Equal(Middle.X + Constants.PlayerSpeed, player.Position.X, 3);
        Assert.Equal(Middle.Y, player.Position.Y, 3);
    }

    [Fact]
    public void Diagonal_is_not_faster_than_straight()
    {
        var player = new Player(Middle);
        player.Update(new Vector2(1, 1), 1f);
        float distance = Vector2.Distance(Middle, player.Position);
        Assert.Equal(Constants.PlayerSpeed, distance, 3);
    }

    [Fact]
    public void Standing_still_keeps_the_last_facing()
    {
        var player = new Player(Middle);
        player.Update(new Vector2(0, -1), 0.1f);
        player.Update(Vector2.Zero, 0.1f);
        Assert.Equal(Facing.Up, player.Facing);
    }

    [Fact]
    public void Stays_inside_the_screen()
    {
        var player = new Player(new Vector2(10, 10));
        player.Update(new Vector2(-1, -1), 5f);
        Assert.Equal(Vector2.Zero, player.Position);

        player.Update(new Vector2(1, 1), 100f);
        Assert.Equal(Constants.ScreenWidth - Constants.PlayerFrameWidth, player.Position.X, 3);
        Assert.Equal(Constants.ScreenHeight - Constants.PlayerFrameHeight, player.Position.Y, 3);
    }

    [Theory]
    [InlineData(Keys.Z, 0, -1)]
    [InlineData(Keys.Q, -1, 0)]
    [InlineData(Keys.S, 0, 1)]
    [InlineData(Keys.D, 1, 0)]
    [InlineData(Keys.Up, 0, -1)]
    [InlineData(Keys.Left, -1, 0)]
    [InlineData(Keys.Down, 0, 1)]
    [InlineData(Keys.Right, 1, 0)]
    public void Each_key_gives_its_direction(Keys key, float x, float y)
    {
        Assert.Equal(new Vector2(x, y), InputReader.ReadMove(new KeyboardState(key)));
    }

    [Fact]
    public void Zqsd_and_arrows_work_together()
    {
        Assert.Equal(new Vector2(-1, -1), InputReader.ReadMove(new KeyboardState(Keys.Q, Keys.Z)));      // diagonal with ZQSD
        Assert.Equal(new Vector2(1, 1), InputReader.ReadMove(new KeyboardState(Keys.Right, Keys.Down)));  // diagonal with arrows
        Assert.Equal(new Vector2(1, -1), InputReader.ReadMove(new KeyboardState(Keys.D, Keys.Up)));       // mixed schemes
        Assert.Equal(new Vector2(-1, 0), InputReader.ReadMove(new KeyboardState(Keys.Q, Keys.Left)));     // same direction twice: not faster
        Assert.Equal(Vector2.Zero, InputReader.ReadMove(new KeyboardState(Keys.Q, Keys.D)));              // opposite keys cancel
        Assert.Equal(Vector2.Zero, InputReader.ReadMove(new KeyboardState()));                            // nothing pressed
    }

    [Fact]
    public void Movement_does_not_depend_on_the_frame_rate()
    {
        // One second at 30 FPS, at 60 FPS and at 144 FPS must end at the same place.
        float[] frameRates = { 30, 60, 144 };
        foreach (float fps in frameRates)
        {
            var player = new Player(Middle);
            for (int frame = 0; frame < fps; frame++)
            {
                player.Update(new Vector2(1, 0), 1f / fps);
            }
            Assert.Equal(Middle.X + Constants.PlayerSpeed, player.Position.X, 2);
        }
    }

    [Fact]
    public void Player_walks_over_furniture_and_walls()
    {
        // Start on the desk at (80, 280) and walk up through the classroom wall at y = 40:
        // nothing stops the student (there is no collision), only the top of the screen.
        var player = new Player(new Vector2(80, 280));
        player.Update(new Vector2(0, -1), 1f); // 150 px up
        Assert.Equal(130, player.Position.Y, 3);
        player.Update(new Vector2(0, -1), 1f);
        Assert.Equal(0, player.Position.Y, 3);
    }
}
