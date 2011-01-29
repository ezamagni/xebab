using System;
using Microsoft.Xna.Framework;

namespace Xebab.Helpers.Maths
{
	public class HarmonicPulsation : Harmonic
	{
		public HarmonicPulsation(float magnitude, float frequency, GameTime gameTime)
			: base(magnitude / 2, frequency, (float)(-Math.PI / 2), gameTime) { }

		public override float GetValue(GameTime gameTime)
		{
			return base.GetValue(gameTime) + Magnitude;
		}

	}
}