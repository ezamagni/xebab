using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Xebab.Helpers;
using Xebab.Helpers.Polygons;
using Xebab.Graphics.Camera;

namespace Xebab.Graphics.Sprites
{
    public class AnimatedSprite : Sprite
    {
        protected const int DEFAULT_FRAME_TIME = 200;

        public override Rectangle BoundingBox
        {
            get 
            {
                Rectangle frame = animation.GetFrame();
                return new Rectangle((int)tlPosition.X, (int)tlPosition.Y,
                frame.Width, frame.Height); 
            }
        }

        protected Vector2 tlPosition;
        public override Vector2 Position
        {
            get { return tlPosition; }
            set {
				Vector2 translation = value - tlPosition;
				foreach (Polygon p in shapeSet)
					p.Translate(translation);
				tlPosition = value; 
			}
        }

        protected SpriteAnimation animation;
        public SpriteAnimation Animation
        {
            get { return animation; }
        }


        public override int Altitude { get; set; }

        public int TimePerFrame { get; set; }

        protected Texture2D spritesheet;


        public AnimatedSprite(IContentHandler contentHandler, List<Polygon> shapeSet, 
            Texture2D spritesheet, Size frameSize, int timePerFrame = DEFAULT_FRAME_TIME)
            : base(contentHandler, shapeSet)
        {
            //TODO: gli ultimi 3 parametri potrebbero essere collassati in una struttura
            this.spritesheet = spritesheet;
            this.animation = new SpriteAnimation(frameSize, spritesheet, timePerFrame);
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            animation.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, CameraViewport viewport, float layerDepth)
        {
            spriteBatch.Draw(spritesheet, tlPosition - viewport.Position, animation.GetFrame(), 
                Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
        }

    }
}
