using Microsoft.Xna.Framework.Graphics;
using Xebab.Graphics;
using Xebab.Graphics.Camera;
using Xebab.Helpers;
using Microsoft.Xna.Framework;
using System;

namespace Xebab
{
	public class Level : ICameraDrawable
	{
		private Texture2D[,] tiles;
		private int tileRows;
		private int tileCols;

		public int Width
		{
			get { return tiles[0, 0].Width * tileCols; }
		}

		public int Height
		{
			get { return tiles[0, 0].Height * tileRows; }
		}

		public Size Size
		{
			get { return new Size(Width, Height); }
		}

		public DrawInterval DrawInterval
		{
			get { return DrawInterval.Background; }
		}


		public Level(Texture2D[,] tiles)
		{
			this.tiles = tiles;
			this.tileCols = tiles.GetLength(0);
			this.tileRows = tiles.GetLength(1);
		}


		public void Draw(SpriteBatch spriteBatch, CameraViewport viewport, float layerDepth)
		{
			int tileWidth = tiles[0, 0].Width;
			int tileHeight = tiles[0, 0].Height;

			//calculate what tiles we must start drawing with
			int tilex = (int)viewport.Position.X / tileWidth;
			int tiley = (int)viewport.Position.Y / tileHeight;
			if (viewport.Position.X < 0) tilex--;
			if (viewport.Position.Y < 0) tiley--;

			//calculate at wich position we must start drawing tiles
			//Vector2 tileCursor = new Vector2(-viewport.ViewportPosition.X % tileWidth, -viewport.ViewportPosition.Y % tileHeight);
			Vector2 tileCursor = new Vector2(tilex * tileWidth - viewport.Position.X, tiley * tileHeight - viewport.Position.Y);

			//calculate how many tiles we must draw
			int numx = (int)Math.Ceiling((viewport.Size.Width - tileCursor.X) / tileWidth);
			int numy = (int)Math.Ceiling((viewport.Size.Height - tileCursor.Y) / tileHeight);

			//draw sprites row-by-row
			float startX = tileCursor.X;
			for (int r = tiley; r < tiley + numy; r++)
			{
				if (r >= tileRows) break;

				for (int c = tilex; c < tilex + numx; c++)
				{
					if (c >= tileCols) break;

					if (c >= 0 && r >= 0)
						spriteBatch.Draw(tiles[r, c], tileCursor, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);

					tileCursor.X += tileWidth;
				}

				tileCursor.X = startX;
				tileCursor.Y += tileHeight;
			}
		}
	}
}
