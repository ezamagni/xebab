using System;
using Microsoft.Xna.Framework;

namespace DemoTest
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
		[STAThread]
        static void Main(string[] args)
        {
            //using (Game game = new Editor.GameEditor())
			using (Game game = new Game2())
            {
                game.Run();
            }
        }
    }
#endif
}

