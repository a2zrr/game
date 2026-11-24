using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace UniScape
{
    // The student: position, facing and how they walk.
    // No drawing or keyboard code in here so it's easy to test.
    public class Player
    {
        // Max distance moved in one go. A long frame gets split into small steps so the student
        // can't skip over a wall, since walls are 40 px thick and the feet 24 px wide.
        private const float MaxStepLength = 10f;

        // Rectangles the feet can't go into. Only walls end up here, see WallCollisions.
        private readonly IReadOnlyList<Rectangle> _walls;

        // Top-left corner of the sprite frame on screen
        public Vector2 Position { get; private set; }
        public Facing Facing { get; private set; } = Facing.Down;

        // If no walls are given the student just walks freely
        public Player(Vector2 startPosition, IReadOnlyList<Rectangle> walls = null)
        {
            Position = startPosition;
            _walls = walls ?? Array.Empty<Rectangle>();
        }

        // move is the direction from the keyboard, like (1, 0) or (1, 1).
        // seconds is the time since the last frame.
        public void Update(Vector2 move, float seconds)
        {
            if (move == Vector2.Zero)
            {
                return; // not moving, keep facing the same way
            }

            Facing = FacingFor(move);

            // Normalize so going diagonally isn't faster than going straight
            move.Normalize();
            Vector2 step = move * Constants.PlayerSpeed * seconds;

            int pieces = Math.Max(1, (int)MathF.Ceiling(step.Length() / MaxStepLength));
            for (int i = 0; i < pieces; i++)
            {
                MoveBy(step / pieces);
            }
        }

        // Move on X first, then on Y. If a wall blocks one axis the other one still moves,
        // so the student slides along the wall instead of stopping dead.
        // Doing both axes at once could also let a diagonal step cut through a wall corner.
        private void MoveBy(Vector2 step)
        {
            float x = Position.X + step.X;
            foreach (Rectangle wall in _walls)
            {
                if (FeetTouch(new Vector2(x, Position.Y), wall))
                {
                    // Snap the feet against the wall on the side we came from
                    x = step.X > 0
                        ? wall.Left - Constants.PlayerFeetOffsetX - Constants.PlayerFeetWidth
                        : wall.Right - Constants.PlayerFeetOffsetX;
                }
            }

            float y = Position.Y + step.Y;
            foreach (Rectangle wall in _walls)
            {
                if (FeetTouch(new Vector2(x, y), wall))
                {
                    y = step.Y > 0
                        ? wall.Top - Constants.PlayerFeetOffsetY - Constants.PlayerFeetHeight
                        : wall.Bottom - Constants.PlayerFeetOffsetY;
                }
            }

            // Keep the whole sprite on screen
            x = Math.Clamp(x, 0, Constants.ScreenWidth - Constants.PlayerFrameWidth);
            y = Math.Clamp(y, 0, Constants.ScreenHeight - Constants.PlayerFrameHeight);
            Position = new Vector2(x, y);
        }

        // True if the feet overlap the wall when the frame is at position.
        // Just touching an edge doesn't count, that's what lets the student walk along walls.
        public static bool FeetTouch(Vector2 position, Rectangle wall)
        {
            float left = position.X + Constants.PlayerFeetOffsetX;
            float top = position.Y + Constants.PlayerFeetOffsetY;
            float right = left + Constants.PlayerFeetWidth;
            float bottom = top + Constants.PlayerFeetHeight;
            return left < wall.Right && right > wall.Left && top < wall.Bottom && bottom > wall.Top;
        }

        // Face along the bigger axis of the movement, a perfect diagonal counts as left or right
        public static Facing FacingFor(Vector2 move)
        {
            if (Math.Abs(move.X) >= Math.Abs(move.Y))
            {
                return move.X < 0 ? Facing.Left : Facing.Right;
            }
            return move.Y < 0 ? Facing.Up : Facing.Down;
        }

        // The part of player.png to draw for a facing.
        // It's its own small method so we can add a walk animation later.
        public static Rectangle FrameFor(Facing facing)
        {
            return new Rectangle((int)facing * Constants.PlayerFrameWidth, 0,
                                 Constants.PlayerFrameWidth, Constants.PlayerFrameHeight);
        }
    }
}
