using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Xebab.Graphics
{
	public interface IDrawable
	{

		 void Draw(SpriteBatch spriteBatch, Rectangle viewPort);

	}
}
