using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Xebab.Model.Polygons;

namespace Xebab.Graphics.Sprites
{
	public abstract class Sprite
	{
		public Rectangle BoundingBox { get; }
		List<Polygon> ShapeSet;
		Vector2 Position;
		int Altitude;
		Level level;


	}
}
