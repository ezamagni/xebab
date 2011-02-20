using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Xebab.Graphics.Camera;
using Xebab.Graphics.Sprites;
using Xebab.Helpers;
using Xebab.Input;
using Xebab.Resources;

namespace Xebab
{
    public class ContentHandler : IContentHandler
    {
		public SpriteHandler SpriteHandler { get; private set; }
		public KeyboardHandler KeyboardHandler { get; private set; }
        public CursorHandler CursorHandler { get; private set; }
        public ResourceHandler ResourceHandler { get; private set; }
		//ANALISI: i decal possono alternativamente essere autonomamente gestiti dalle sprite
		//public EffectHandler EffectHandler { get; private set; }

        public Level Level { get; private set; }
		public Camera Camera { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }

		private GraphicsDevice graphicsDevice;


        internal ContentHandler(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, 
            Level level, Camera camera, ResourceHandler resourceHandler)
        {
			SpriteHandler = new SpriteHandler();
			KeyboardHandler = new KeyboardHandler();
            CursorHandler = new CursorHandler();
			//EffectHandler = new EffectHandler();
            ResourceHandler = resourceHandler;

            this.SpriteBatch = spriteBatch;
            this.Level = level;
            this.Camera = camera;
            this.graphicsDevice = graphicsDevice;
        }

        public void Update(GameTime gameTime)
        {
			SpriteHandler.Update(gameTime);
			KeyboardHandler.Update(gameTime);
            CursorHandler.Update(gameTime);
			//EffectHandler.Update(gameTime);
        }

		public void Draw()
		{
            //LAYER 0: level background
            Level.Draw(SpriteBatch, Camera.Viewport, 0);

            //LAYER 1: sprites
			foreach (Sprite sprite in SpriteHandler.Sprites)
			{
				//TODO: calcolare layerDepth
				sprite.Draw(SpriteBatch, Camera.Viewport, 0);
			}

            //LAYER 2: decals
            //TODO
		}

		public Texture2D CreateTexture(Size size)
		{
			throw new System.NotImplementedException();
		}
	}
}
