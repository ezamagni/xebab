using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Xebab.Input
{

    public class CursorHandler
    {

		private LinkedList<ScreenTrap> Traps;

        public CursorHandler()
        {
			Traps = new LinkedList<ScreenTrap>();
        }


        public void Update(GameTime gameTime)
        {
            //Obtain cursor status
            MouseState cursor = Mouse.GetState();

            //Check if cursor is upon a trap frame
			foreach (ScreenTrap trap in Traps)
            {
				if (!trap.Active) continue;

				if (isInside(cursor.X, cursor.Y, trap.TrappedFrame))
				{
					//cursor is inside this trap
					Point relativePoint = new Point(cursor.X - trap.TrappedFrame.Left, cursor.Y - trap.TrappedFrame.Top);

					if (trap.Status == TrapStatus.Idle)
					{
						//cursor raised a previously idle trap
						trap.Status = TrapStatus.Raised;
						call(trap.EnterAction, relativePoint, cursor);
					}
					else
					{
						//cursor hovered over an altready raised trap
						call(trap.HoverAction, relativePoint, cursor);
					}
				}
				else
				{
					//cursor is not inside this trap
					if (trap.Status == TrapStatus.Raised)
					{
						//cursor left a raised trap
						trap.Status = TrapStatus.Idle;
						call(trap.LeaveAction, new Point(cursor.X - trap.TrappedFrame.Left, cursor.Y - trap.TrappedFrame.Top), cursor);
					}
				}
            }
        }

        //TODO: a queue is generally needed to add/remove subscribers
        public void Subscribe(ScreenTrap trap)
        {
            Traps.AddLast(trap);
        }

        public void UnSubscribe(ScreenTrap trap)
        {
            Traps.Remove(trap);
        }

		private void call(Action<Point, MouseState> action, Point pointParam, MouseState mouseParam)
		{
			if (action != null) action(pointParam, mouseParam);
		}

        private bool isInside(int X, int Y, Rectangle frame)
        {
            return (X > frame.Left && X < (frame.Left + frame.Width)
                && Y > frame.Top && Y < (frame.Top + frame.Height));
        }
    }
}
