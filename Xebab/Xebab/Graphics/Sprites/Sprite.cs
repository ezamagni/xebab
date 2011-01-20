using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Xebab.Model.Polygons;
using Xebab.Model;

namespace Xebab.Graphics.Sprites
{
	public abstract class Sprite : IDrawable
	{
		IContentHandler contentHandler;
		public Rectangle BoundingBox { get; }
		List<Polygon> ShapeSet;
		Vector2 Position;
		float spriteBatchLevel;
		int Altitude;


	}
}
