using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Xebab.Behaviors;
using Xebab.Helpers;

namespace Xebab.Graphics.Sprites
{
	public class SpriteAnimation : IBehavable
	{
        public bool Active { get; protected set; }

        public Size FrameSize { get; protected set; }

        public int FrameCount { get; protected set; }

        protected int curFrame = 0;
        public virtual int CurrentFrame
        {
            get { return curFrame; }
            set
            {
                if (value < 0 || value >= FrameCount)
                    throw new IndexOutOfRangeException(
                        "Invalid frame index selected");
                curFrame = value;
                timeSinceLastFrame = 0;
            }
        }

        public int AnimationCount { get; protected set; }

        protected int curAnimation = 0;
        public virtual int CurrentAnimation
        {
            get { return curAnimation; }
            set
            {
                if (value < 0 || value >= AnimationCount)
                    throw new IndexOutOfRangeException(
                        "Invalid animation index selected");
                curAnimation = value;
            }
        }

        public virtual int TimePerFrame { get; set; }


		protected int timeSinceLastFrame = 0;
        protected List<IBehavior> behaviors;
        protected Queue<IBehavior> behaviorsToRemove;


        public SpriteAnimation(Texture2D spritesheet, int frameCount, int animationCount, int timePerFrame = 0)
            : this(new Size((int)(spritesheet.Width / frameCount), (int)(spritesheet.Height / animationCount)),
            frameCount, animationCount, timePerFrame)
        { }

        public SpriteAnimation(Size frameSize, Texture2D spritesheet, int timePerFrame = 0)
            : this(frameSize, (int)(spritesheet.Width / frameSize.Width), 
            (int)(spritesheet.Height / frameSize.Height), timePerFrame)
        { }

        public SpriteAnimation(Size frameSize, int frameCount, int animationCount, int timePerFrame = 0)
		{
			this.FrameSize = frameSize;
			this.FrameCount = frameCount;
            this.AnimationCount = animationCount;
            this.TimePerFrame = timePerFrame;
            Active = true;

            this.behaviors = new List<IBehavior>();
            this.behaviorsToRemove = new Queue<IBehavior>();
		}


        public virtual Rectangle GetFrame()
		{
			return new Rectangle(curFrame * FrameSize.Width, curAnimation * FrameSize.Height,
									FrameSize.Width, FrameSize.Height);
		}

        public virtual void NextFrame()
        {
            timeSinceLastFrame = 0;
            curFrame++;
            if (curFrame >= FrameCount) 
                curFrame = 0;
        }

        //public void SetSpeed(int msPerFrame)
        //{
        //    this.timePerFrame = msPerFrame;
        //}

        //public void SetSpeedRatio(float ratio)
        //{
        //    //timePerFrame = (int)(3 * maxTimePerFrame * Math.Pow(ratio, 2) - 6 * maxTimePerFrame * ratio + 4 * maxTimePerFrame); 
        //    //timePerFrame = (int)(Math.Pow((Math.Log(ratio)), 2) + maxTimePerFrame);
        //    if (ratio < 0.4) ratio = 0.4f;
        //    timePerFrame = (int)(maxTimePerFrame / ratio);
        //}

        public virtual void Start()
        {
            Active = true;
        }

        public virtual void Stop()
        {
            Active = false;
            timeSinceLastFrame = 0;
        }

        public virtual void Reset()
		{
			timeSinceLastFrame = 0;
			curFrame = 0;
		}

        public virtual void Update(GameTime gameTime)
        {
            if (!Active) return;

            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > TimePerFrame)
            {
                //Update animation frame
                NextFrame();
            }

            //Remove behaviors scheduled for deletion
            while (behaviorsToRemove.Count > 0)
            {
                behaviors.Remove(behaviorsToRemove.Dequeue());
            }

            //Update each behavior
            foreach (IBehavior b in behaviors)
            {
                if (b.Active) b.Update(gameTime);
            }
        }


        public void AddBehavior(IBehavior behavior)
        {
            behaviors.Add(behavior);
        }

        public void RemoveBehavior(IBehavior behavior)
        {
            behaviorsToRemove.Enqueue(behavior);
        }
    }
}
