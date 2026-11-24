using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UniScape.Map
{
    // Draws the ground floor map. The pictures are loaded once; their sizes come from the PNGs.
    public class MapRenderer
    {
        private readonly Dictionary<string, Texture2D> _textures = new Dictionary<string, Texture2D>();
        private readonly List<Placement> _ground = new List<Placement>();
        private readonly List<Placement> _walls = new List<Placement>();
        private readonly List<Placement> _furniture;

        public MapRenderer(GraphicsDevice device)
        {
            // Every asset file is named "<asset>.png".
            foreach (string asset in AllAssetNames())
            {
                if (!_textures.ContainsKey(asset))
                {
                    _textures[asset] = TextureLoader.Load(device, asset + ".png");
                }
            }

            foreach (Area area in GroundFloorMap.Ground)
            {
                _ground.AddRange(Expand(area));
            }
            foreach (Area area in GroundFloorMap.Walls)
            {
                _walls.AddRange(Expand(area));
            }

            // Nothing moves on this map, so the furniture only needs to be sorted once.
            _furniture = MapLayout.SortByBottom(GroundFloorMap.Furniture, asset => _textures[asset].Height);
        }

        // Draw order: ground, effects, walls, things on the walls, then the furniture
        // sorted by its bottom edge. Inside each layer, the order of floor.xml is kept.
        public void Draw(SpriteBatch spriteBatch)
        {
            DrawAll(spriteBatch, _ground);
            DrawAll(spriteBatch, GroundFloorMap.Effects);
            DrawAll(spriteBatch, _walls);
            DrawAll(spriteBatch, GroundFloorMap.WallObjects);
            DrawAll(spriteBatch, _furniture);
        }

        private void DrawAll(SpriteBatch spriteBatch, List<Placement> placements)
        {
            foreach (Placement p in placements)
            {
                spriteBatch.Draw(_textures[p.Asset], new Vector2(p.X, p.Y), Color.White);
            }
        }

        private List<Placement> Expand(Area area)
        {
            Texture2D texture = _textures[area.Asset];
            return MapLayout.Expand(area, texture.Width, texture.Height);
        }

        private static IEnumerable<string> AllAssetNames()
        {
            foreach (Area a in GroundFloorMap.Ground) yield return a.Asset;
            foreach (Placement p in GroundFloorMap.Effects) yield return p.Asset;
            foreach (Area a in GroundFloorMap.Walls) yield return a.Asset;
            foreach (Placement p in GroundFloorMap.WallObjects) yield return p.Asset;
            foreach (Placement p in GroundFloorMap.Furniture) yield return p.Asset;
        }
    }
}
