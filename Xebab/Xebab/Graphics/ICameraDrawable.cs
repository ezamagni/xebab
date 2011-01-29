using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Xebab.Graphics.Camera;

namespace Xebab.Graphics
{
	public enum DrawInterval
	{ 
		Foreground,
		VerticalSorted,
		Background
	}

	public interface ICameraDrawable
	{
		DrawInterval DrawInterval { get; }
		void Draw(SpriteBatch spriteBatch, CameraViewport viewport, float layerDepth);
	}
}
