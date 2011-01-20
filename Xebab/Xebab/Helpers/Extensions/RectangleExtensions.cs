using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Xebab.Helpers.Extensions
{
	public static class RectangleExtensions
	{
		
		public static Rectangle ShiftedPosition(this Rectangle rect, Vector2 Position) 
		{
			return new Rectangle(rect.X + (int)Position.X, rect.Y + (int)Position.Y, rect.Width, rect.Height);
		}

	}
}
