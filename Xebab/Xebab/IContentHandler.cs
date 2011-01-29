using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Xebab.Graphics.Sprites;
using Xebab.Helpers;
using Xebab.Resources;

namespace Xebab
{
	public interface IContentHandler
	{
		Level Level { get; }
		SpriteHandler SpriteHandler { get; }
		//SoundHandler SoundHandler;
		//EffectHandler EffectHandler { get; }
		ResourceHandler ResourceHandler { get; }

		//generazione di una nuova texture (serve GraphicsDevice)
		Texture2D CreateTexture(Size size);

		void Update(GameTime gameTime);
		void Draw();
	}
}
