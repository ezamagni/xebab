using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Xebab.Input
{
	public class KeyTrap
	{
		public Keys[] TrappedKeys { get; private set; }
		public Action<KeyboardState> Action { get; private set; }

		public KeyTrap(Keys trappedKey, Action<KeyboardState> action)
			: this(new[] { trappedKey }, action)
		{ }


		public KeyTrap(Keys[] trappedKeys, Action<KeyboardState> action)
		{
			TrappedKeys = trappedKeys;
			Action = action;
		}

        public KeyTrap()
        {
            // TODO: Complete member initialization
        }
	}
}
