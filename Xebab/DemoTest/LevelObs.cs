using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Xebab;
using Xebab.Helpers.Polygons;

namespace DemoTest
{
	public class LevelObs : Level
	{
		private List<Polygon> obstacles;
		public List<Polygon> Obstacles { get { return obstacles; } }

		public LevelObs(Texture2D[,] tiles, List<Polygon> obstacles)
			: base(tiles)
		{
			this.obstacles = obstacles;
		}


		
	}
}
