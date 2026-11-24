using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UniScape.Map;

namespace UniScape
{
    // The ground floor with the student walking around on it.
    // Only walls block the student, furniture and everything else can be walked over.
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
            _player = new Player(GroundFloorMap.PlayerStart, WallCollisions.GroundFloor);
            base.Initialize(); // calls LoadContent()
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _map = new MapRenderer(GraphicsDevice);
            _playerTexture = TextureLoader.Load(GraphicsDevice, "player.png");
        }

        // Runs every frame before Draw, reads the keyboard and moves the student
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keys = Keyboard.GetState();
            if (keys.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // Time since the last frame in seconds, about 0.0167 at 60 FPS.
            // Multiplying the speed by it keeps the speed the same whatever the frame rate.
            float seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _player.Update(InputReader.ReadMove(keys), seconds);

            base.Update(gameTime);
        }

        // Runs every frame after Update and draws everything from back to front
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // AlphaBlend goes with the premultiplied textures from TextureLoader
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp);

            // Map first
            _map.Draw(_spriteBatch);

            // Then the student, so they're always drawn on top of the map.
            // The position is rounded to whole pixels to keep the pixel art sharp,
            // otherwise something like x = 10.4 gets blurred by the linear filter.
            Vector2 screenPosition = new Vector2(
                System.MathF.Round(_player.Position.X),
                System.MathF.Round(_player.Position.Y));
            _spriteBatch.Draw(_playerTexture, screenPosition, Player.FrameFor(_player.Facing), Color.White);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
