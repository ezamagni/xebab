using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Xebab.Input
{
	internal enum TrapStatus
	{
		Idle,
		Raised
	}

    public class ScreenTrap
    {
        public Rectangle TrappedFrame { get; set; }
        public Action<Point, MouseState> HoverAction { get; private set; }
		public Action<Point, MouseState> EnterAction { get; private set; }
		public Action<Point, MouseState> LeaveAction { get; private set; }

		private bool active;
		public bool Active {
			get { return active; }
			set
			{
				active = value;
				Status = TrapStatus.Idle;
			}
		}

		internal TrapStatus Status { get; set; }


		public ScreenTrap(Rectangle trappedFrame, bool active, Action<Point, MouseState> cursorEntersAction = null,
			Action<Point, MouseState> cursorLeavesAction = null, Action<Point, MouseState> cursorHovers = null)
        {
            this.TrappedFrame = trappedFrame;
            this.Active = active;
            this.HoverAction = cursorHovers;
			this.EnterAction = cursorEntersAction;
			this.LeaveAction = cursorLeavesAction;
        }

		public ScreenTrap(Rectangle trappedFrame, Action<Point, MouseState> cursorEntersAction = null,
			Action<Point, MouseState> cursorLeavesAction = null, Action<Point, MouseState> cursorHovers = null)
			: this(trappedFrame, true, cursorEntersAction, cursorLeavesAction, cursorHovers) 
		{ }

    }
}
