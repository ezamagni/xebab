using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Xebab.Model
{
	public interface IContentHandler
	{
		Level Level { get; }
		SpriteHandler SpriteHandler { get; }
		//SoundHandler SoundHandler;
		EffectHandler EffectHandler { get; }
		ResourceHandler ResourceHandler { get; }

		//generazione di una nuova texture (serve GraphicsDevice)
		Texture2D CreateTexture(Size size);
	}
}
