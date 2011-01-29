using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Xebab.Graphics.Camera;
using Xebab.Helpers.Polygons;

namespace Xebab.Graphics.Sprites
{
	public abstract class Sprite : ICameraDrawable
	{
		public virtual DrawInterval DrawInterval
		{
			get { return DrawInterval.VerticalSorted; }
		}

		IContentHandler contentHandler;
		public Rectangle BoundingBox { get; private set; }
		List<Polygon> ShapeSet;
		Vector2 Position;
		float spriteBatchLevel;
		int Altitude;

		public void Draw(SpriteBatch spriteBatch, CameraViewport viewport, float layerDepth)
		{
			throw new NotImplementedException();
		}

		public abstract void Update(GameTime gameTime);
	}
}
