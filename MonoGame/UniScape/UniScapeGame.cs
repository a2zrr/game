using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UniScape.Map;

namespace UniScape
{
    // Current step: the ground floor map, with the student walking freely on top of it.
    // There is no collision: the map is only a picture.
    public class UniScapeGame : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private MapRenderer _map;

        private Texture2D _playerTexture;
        private Player _player;

        public UniScapeGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = Constants.ScreenWidth;
            _graphics.PreferredBackBufferHeight = Constants.ScreenHeight;
            Window.Title = "UniScape";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _player = new Player(GroundFloorMap.PlayerStart);
            base.Initialize(); // calls LoadContent()
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _map = new MapRenderer(GraphicsDevice);
            _playerTexture = TextureLoader.Load(GraphicsDevice, "player.png");
        }

        // Called once per frame, before Draw: read the keyboard and move the student.
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keys = Keyboard.GetState();
            if (keys.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // Time since the last frame, in seconds (about 0.0167 s at 60 FPS).
            // Moving by "speed x seconds" gives the same speed at any frame rate.
            float seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _player.Update(InputReader.ReadMove(keys), seconds);

            base.Update(gameTime);
        }

        // Called once per frame, after Update: draw everything, back to front.
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // AlphaBlend works with the premultiplied textures made by TextureLoader.
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp);

            // 1. The map first...
            _map.Draw(_spriteBatch);

            // 2. ...then the student, so they are always drawn on top of the map.
            // The position is rounded to whole pixels so the pixel art stays sharp
            // (a sprite drawn at x = 10.4 would be blurred by the linear filter).
            Vector2 screenPosition = new Vector2(
                System.MathF.Round(_player.Position.X),
                System.MathF.Round(_player.Position.Y));
            _spriteBatch.Draw(_playerTexture, screenPosition, Player.FrameFor(_player.Facing), Color.White);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
