using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace UniScape
{
    // Turns the keyboard into a movement direction.
    // Two control schemes work at the same time:
    //   arrow keys          and   Z Q S D (AZERTY keyboard)
    //   Up    = move up           Z = move up
    //   Left  = move left         Q = move left
    //   Down  = move down         S = move down
    //   Right = move right        D = move right
    public static class InputReader
    {
        public static Vector2 ReadMove(KeyboardState keys)
        {
            Vector2 move = Vector2.Zero;
            if (keys.IsKeyDown(Keys.Q) || keys.IsKeyDown(Keys.Left)) move.X -= 1;
            if (keys.IsKeyDown(Keys.D) || keys.IsKeyDown(Keys.Right)) move.X += 1;
            if (keys.IsKeyDown(Keys.Z) || keys.IsKeyDown(Keys.Up)) move.Y -= 1;   // screen Y goes down, so "up" is -1
            if (keys.IsKeyDown(Keys.S) || keys.IsKeyDown(Keys.Down)) move.Y += 1;
            return move;
        }
    }
}
