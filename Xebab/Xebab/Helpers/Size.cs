
namespace Xebab.Helpers
{
	public struct Size
	{
		public int Width;
		public int Height;

		public Size(int Width, int Height)
		{
			this.Width = Width;
			this.Height = Height;
		}

        public Size(int value)
            : this(value, value) { }
	}

}
