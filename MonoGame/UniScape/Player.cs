using System;
using Microsoft.Xna.Framework;

namespace UniScape
{
    // The student: a position, a facing direction and the walking rules.
    // No drawing and no keyboard here, so this class is easy to test.
    public class Player
    {
        // Top-left corner of the frame on screen.
        public Vector2 Position { get; private set; }
        public Facing Facing { get; private set; } = Facing.Down;

        public Player(Vector2 startPosition)
        {
            Position = startPosition;
        }

        // move: the wanted direction from the keyboard, for example (1, 0) or (1, 1).
        // seconds: time since the last frame.
        public void Update(Vector2 move, float seconds)
        {
            if (move == Vector2.Zero)
            {
                return; // standing still: keep the last facing
            }

            Facing = FacingFor(move);

            // Normalise so that walking diagonally is not faster than walking straight.
            move.Normalize();
            Vector2 next = Position + move * Constants.PlayerSpeed * seconds;

            // Keep the whole frame inside the screen.
            next.X = Math.Clamp(next.X, 0, Constants.ScreenWidth - Constants.PlayerFrameWidth);
            next.Y = Math.Clamp(next.Y, 0, Constants.ScreenHeight - Constants.PlayerFrameHeight);
            Position = next;
        }

        // Facing = the dominant axis of the movement (a tie goes to left/right).
        public static Facing FacingFor(Vector2 move)
        {
            if (Math.Abs(move.X) >= Math.Abs(move.Y))
            {
                return move.X < 0 ? Facing.Left : Facing.Right;
            }
            return move.Y < 0 ? Facing.Up : Facing.Down;
        }

        // The part of player.png to draw for a facing direction.
        // Kept in one small function so a walk animation can be added later.
        public static Rectangle FrameFor(Facing facing)
        {
            return new Rectangle((int)facing * Constants.PlayerFrameWidth, 0,
                                 Constants.PlayerFrameWidth, Constants.PlayerFrameHeight);
        }
    }
}
