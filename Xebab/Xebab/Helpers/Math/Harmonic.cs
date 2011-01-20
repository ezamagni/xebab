using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Xebab.Helpers.Math
{

    public class Harmonic
    {
        public List<Harmonic> Components { get; set; }
        public float Magnitude {get; set; }
        public float Frequency {get; private set; }
        public float Period { get { return 1 / Frequency; } }
		public float Phase { get; set; }
        protected double startTime;


		public Harmonic(float magnitude, float frequency, float phase, GameTime gameTime)
			: this(magnitude, frequency, phase, gameTime, null)
		{
			Components = new List<Harmonic>();
		}

		public Harmonic(float magnitude, float frequency, GameTime gameTime)
			: this(magnitude, frequency, 0f, gameTime, null)
        {
			Components = new List<Harmonic>();
		}

		public Harmonic(float magnitude, float frequency, float phase, GameTime gameTime, List<Harmonic> components)
        {
            this.Magnitude = magnitude;
			this.startTime = gameTime.TotalGameTime.TotalMilliseconds;
			this.Components = components;
			this.Phase = phase;

			//since time is in ms, we scale frequency 
			this.Frequency = frequency / 1000;
        }


		public virtual float GetValue(GameTime gameTime)
        {
			double t = gameTime.TotalGameTime.TotalMilliseconds - startTime;

            //Compute main armonic
            double amount = Magnitude * Math.Sin(2 * Math.PI * Frequency * t + Phase);

            //Add components contribute (if any)
            foreach (Harmonic a in Components)
            {
                amount += a.GetValue(gameTime);
            }

            return (float)amount;
        }

		public virtual void Restart(double msTotalTime)
        {
            startTime = msTotalTime;
        }

    }
}
