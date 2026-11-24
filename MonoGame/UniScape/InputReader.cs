using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace UniScape
{
    // Turns the keyboard state into a movement direction.
    // Arrow keys and ZQSD for AZERTY both work, even mixed together:
    // Z or Up, Q or Left, S or Down, D or Right.
    public static class InputReader
    {
        public static Vector2 ReadMove(KeyboardState keys)
        {
            Vector2 move = Vector2.Zero;
            if (keys.IsKeyDown(Keys.Q) || keys.IsKeyDown(Keys.Left)) move.X -= 1;
            if (keys.IsKeyDown(Keys.D) || keys.IsKeyDown(Keys.Right)) move.X += 1;
            if (keys.IsKeyDown(Keys.Z) || keys.IsKeyDown(Keys.Up)) move.Y -= 1;   // Y goes down on screen so up is -1
            if (keys.IsKeyDown(Keys.S) || keys.IsKeyDown(Keys.Down)) move.Y += 1;
            return move;
        }
    }
}
