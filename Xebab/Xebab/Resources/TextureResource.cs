using Microsoft.Xna.Framework.Graphics;
using Xebab.Model;

namespace Xebab.Graphics.Sprites
{
    public class TextureResource : IResource
    {
        public Texture2D Texture { get; internal set; }
        public string AssetName { get; internal set; }

		public Size FrameSize { get; set; }
	}
}
