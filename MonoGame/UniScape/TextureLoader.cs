using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UniScape
{
    // Loads a PNG from the Assets folder
    public static class TextureLoader
    {
        public static Texture2D Load(GraphicsDevice device, string fileName)
        {
            string path = Path.Combine(System.AppContext.BaseDirectory, Constants.AssetsFolder, fileName);
            Texture2D texture = Texture2D.FromFile(device, path);

            // AlphaBlend in MonoGame expects premultiplied colours, meaning R, G and B are
            // already multiplied by A. PNGs aren't stored like that so we convert them once here,
            // otherwise transparent edges get dark outlines.
            Color[] pixels = new Color[texture.Width * texture.Height];
            texture.GetData(pixels);
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = Color.FromNonPremultiplied(pixels[i].R, pixels[i].G, pixels[i].B, pixels[i].A);
            }
            texture.SetData(pixels);
            return texture;
        }
    }
}
