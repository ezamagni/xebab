using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Xebab.Resources;
using Xebab.Graphics.Camera;

namespace Xebab
{
	public /*abstract*/ class Scene : DrawableGameComponent
	{

		private IContentHandler contentHandler;
        public IContentHandler ContentHandler
        {
            get { return contentHandler; }
        }
		
        private SpriteBatch spriteBatch;

		public Scene(Game game, SpriteBatch spriteBatch, Level level, 
            Camera camera/*, ResourceHandler resourceHandler*/)
			: base(game)
		{
            this.spriteBatch = spriteBatch;
            //TODO hack temporaneo con resourcehandler
			contentHandler = new ContentHandler(game.GraphicsDevice, 
                spriteBatch, level, camera, new ResourceHandler(new System.Collections.Generic.List<Resource>()));
		}


		public override void Update(GameTime gameTime)
		{
			contentHandler.Update(gameTime);
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
            //ANALISI: come regolamentare le chiamate allo
            //spritehandler tra xebab e client?
            spriteBatch.Begin();

			contentHandler.Draw();
			base.Draw(gameTime);

            spriteBatch.End();
		}
	}
}
