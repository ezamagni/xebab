using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Xebab.Graphics.Camera;
using Xebab.Helpers.Polygons;

namespace Xebab.Graphics.Sprites
{
	public class Game
	{
		public void SonoNelgioco()
		{
			var sprite = new Personaggio();
			sprite.AddBehaviour(new MovingBehaviour(sprite));
		}
	}

	public interface IBehaviour
	{
		void Update(GameTime gametime);
	}

	//generica vale anche per decals
	public abstract class Behaviour<T> : IBehaviour where T : class
	{
		protected T lui;

		public Behaviour(T lui)
		{
			this.lui = lui;
		}

		public abstract void Update(GameTime gametime);
	}

	//lato client
	public class MovingBehaviour : Behaviour<Sprite>
	{
		public MovingBehaviour(Sprite lui)
			: base(lui)
		{ }

		public override void Update(GameTime gametime)
		{
			lui.Position = new Vector2(10, 20);
			//throw new NotImplementedException();
		}
	}

	public class Personaggio : Sprite
	{
		public int PuntiVita { get; set; }


		public override void Update(GameTime gameTime)
		{
			throw new NotImplementedException();
		}
	}

	public abstract class Sprite : ICameraDrawable
	{
		public virtual DrawInterval DrawInterval
		{
			get { return DrawInterval.VerticalSorted; }
		}

		public Rectangle BoundingBox { get; private set; }
		public Vector2 Position { get; set; }
		public int Altitude { get; set; }
		protected IContentHandler contentHandler;
		private List<Polygon> shapeSet;
		private float spriteBatchLevel;

		private List<IBehaviour> behaviours;

		public Sprite()
		{
			behaviours = new List<IBehaviour>();
		}

		public void AddBehaviour(IBehaviour behaviour)
		{
			behaviours.Add(behaviour);
		}

		public void Draw(SpriteBatch spriteBatch, CameraViewport viewport, float layerDepth)
		{
			throw new NotImplementedException();
		}

		public abstract void Update(GameTime gameTime);
	}
}
