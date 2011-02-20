using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Xebab.Behaviors;
using Xebab.Graphics.Camera;
using Xebab.Helpers.Polygons;

namespace Xebab.Graphics.Sprites
{
	public abstract class Sprite : ICameraDrawable , IBehavable
	{
		public virtual DrawInterval DrawInterval
		{
			get { return DrawInterval.VerticalSorted; }
		}

        public abstract Rectangle BoundingBox { get; }
        public abstract Vector2 Position { get; set; }
        public abstract int Altitude { get; set; }

        protected IContentHandler contentHandler;
        protected List<Polygon> shapeSet;
		public List<Polygon> ShapeSet { get { return shapeSet; } }
        protected List<IBehavior> behaviors;
        protected Queue<IBehavior> behaviorsToRemove;

		public Sprite(IContentHandler contentHandler, List<Polygon> shapeSet)
		{
            this.contentHandler = contentHandler;
            this.shapeSet = shapeSet;

            behaviors = new List<IBehavior>();
            behaviorsToRemove = new Queue<IBehavior>();
		}

        public abstract void Draw(SpriteBatch spriteBatch, CameraViewport viewport, float layerDepth);

        public virtual void Update(GameTime gameTime)
        {
            //Remove behaviors scheduled for deletion
            while (behaviorsToRemove.Count > 0)
            {
                behaviors.Remove(behaviorsToRemove.Dequeue());
            }
            
            //Update each behavior
            foreach (IBehavior b in behaviors)
            {
                if (b.Active) b.Update(gameTime);
            }
        }

        public void AddBehavior(IBehavior behavior)
        {
            behaviors.Add(behavior);
        }

        public void RemoveBehavior(IBehavior behavior)
        {
            behaviorsToRemove.Enqueue(behavior);
        }

    }
}
