using Microsoft.Xna.Framework;
using Xebab.Helpers;

namespace Xebab.Graphics.Camera
{
	public class CameraViewport
	{
		public Vector2 Position { get; internal set; }
		public Size Size { get; internal set; }

		internal CameraViewport(Size size)
		{
			Size = size;
			Position = Vector2.Zero;
		}

        internal CameraViewport(Size size, Vector2 position)
        {
            Size = size;
            Position = position;
        }
	}
}
