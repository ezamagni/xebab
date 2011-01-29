using System;
using Microsoft.Xna.Framework;
using Xebab.Helpers;

namespace Xebab.Graphics.Sprites
{
	public class SpriteAnimation
	{
        protected const int DEFAULT_MAX_TIME_PER_FRAME = 200;

		private Size frameSize;

		private int frameCount;
		public int FrameCount { get { return frameCount; } }

		private int currentFrame = 0;
		public int CurrentFrame { get { return currentFrame; } }

		private int sheetRow = 0;
		public int SheetRow 
		{ 
			get 
			{
				return sheetRow;
			}
			set 
			{
				sheetRow = value; 
			}
		}

        public bool Enabled { get; set; }

		private int timeSinceLastFrame = 0;
		private int timePerFrame;
		private int maxTimePerFrame;

        public SpriteAnimation(Size frameSize, int frameCount, int maxTimePerFrame = DEFAULT_MAX_TIME_PER_FRAME)
		{
			this.frameSize = frameSize;
			this.frameCount = frameCount;
            this.maxTimePerFrame = maxTimePerFrame;
            SetSpeedRatio(1f);
            Enabled = true;
		}

		public void Update(GameTime gameTime)
		{
            if (!Enabled)
                return;

			timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
			if (timeSinceLastFrame > timePerFrame)
			{
				//Update animation frame
				timeSinceLastFrame = 0;
				currentFrame++;
				if (currentFrame >= frameCount)
					currentFrame = 0;
			}
		}

		public Rectangle GetFrame()
		{
			return new Rectangle(currentFrame * frameSize.Width, sheetRow * frameSize.Height,
									frameSize.Width, frameSize.Height);
		}

		public void SetFrame(int frameIndex)
		{
			if (frameIndex > frameCount || frameIndex < 0)
			{
				//index out of bounds
				throw new Exception("frame index out of bounds");
			}

			timeSinceLastFrame = 0;
			currentFrame = frameIndex;			
		}

        public void SetSpeed(int msPerFrame)
        {
            this.timePerFrame = msPerFrame;
        }

        public void SetSpeedRatio(float ratio)
        {
			//timePerFrame = (int)(3 * maxTimePerFrame * Math.Pow(ratio, 2) - 6 * maxTimePerFrame * ratio + 4 * maxTimePerFrame); 
			//timePerFrame = (int)(Math.Pow((Math.Log(ratio)), 2) + maxTimePerFrame);
			if (ratio < 0.4) ratio = 0.4f;
			timePerFrame = (int)(maxTimePerFrame / ratio);
        }

		public void Reset()
		{
			timeSinceLastFrame = 0;
			currentFrame = 0;
		}
	}
}
