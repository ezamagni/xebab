using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Xebab.Input
{
	public class KeyboardHandler
	{
		private List<KeyTrap> Traps;

		public KeyboardHandler()
		{
			Traps = new List<KeyTrap>();
		}

        //TODO: a queue is generally needed to add/remove subscribers
		public void SubscribeTrap(KeyTrap trap)
		{
			Traps.Add(trap);
		}

		public void Update(GameTime gameTime)
		{
			KeyboardState state = Keyboard.GetState();

			foreach (KeyTrap t in Traps)
			{
				foreach (Keys k in state.GetPressedKeys())
				{
					if (t.TrappedKeys.Contains(k))
					{
						t.Action(state);
						break;
					}
				}
			}
		}
	}
}
