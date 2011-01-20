using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Xebab.Helpers
{
    public class SimpleTimer
    {
        private int timeSinceLastFrame;
        private int delay;

        public int Interval { get; set; }
        public bool Enabled { get; set; }

		public SimpleTimer(int interval, int delay = 0)
        {
            Interval = interval;
            Enabled = true;
            this.delay = delay;
        }

        public bool CheckExpired(GameTime gameTime)
        {
            if (!Enabled)
                return false;

            if (delay > 0)
            {
                delay -= gameTime.ElapsedGameTime.Milliseconds;
                return false;
            }

            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > Interval)
            {
                timeSinceLastFrame = 0;
                return true;
            }
            return false;
        }
    }
}
