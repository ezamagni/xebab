using System;
using System.Collections.Generic;
using System.Linq;
using Xebab.Graphics.Sprites;
using Xebab.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Xebab.Input;
using Xebab.Graphics.Effects;
using Xebab.Graphics;

namespace Xebab
{
    public partial class ContentHandler : IContentHandler
    {
		public SpriteHandler SpriteHandler { get; private set; }
		public KeyboardHandler KeyboardHandler { get; private set; }
        public CursorHandler CursorHandler { get; private set; }
		public EffectHandler EffectHandler { get; private set; }
		public SpriteGame ClientGame { get; private set; }
        public Level Level { get; private set; }
        public ResourceHandler ResourceHandler { get; private set; }

        public SpriteBatch SpriteBatch { get; private set; }

        private ContentHandler(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, Level level, ResourceHandler resourceHandler)
        {
            SpriteBatch = game.SpriteBatch;
			
			//magnificent 6 (level, effect, keyboard, mouse, sound, effects)
			Level = level;
			SpriteHandler = new SpriteHandler();
			KeyboardHandler = new KeyboardHandler();
            CursorHandler = new CursorHandler();
			EffectHandler = new EffectHandler();
            ResourceHandler = resourceHandler;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

			SpriteHandler.Update(gameTime);
			KeyboardHandler.Update(gameTime);
            CursorHandler.Update(gameTime);
			EffectHandler.Update(gameTime);
        }

    }
}
