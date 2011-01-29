using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Xebab.Resources;

namespace Xebab
{
	public abstract class Scene : DrawableGameComponent
	{
		private IContentHandler contentHandler;
		

		public Scene(Game game, SpriteBatch spriteBatch, Level level, ResourceHandler resourceHandler)
			: base(game)
		{
			contentHandler = new ContentHandler(game.GraphicsDevice, spriteBatch, level, resourceHandler);
		}


		public override void Update(GameTime gameTime)
		{
			contentHandler.Update(gameTime);

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			contentHandler.Draw();
			base.Draw(gameTime);
		}
	}
}
