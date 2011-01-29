using Microsoft.Xna.Framework.Graphics;
using Xebab.Helpers;

namespace Xebab.Resources
{
    public class TextureResource : Resource
    {
        public Texture2D Texture { get; internal set; }
		public Size FrameSize { get; set; }
	}
}
