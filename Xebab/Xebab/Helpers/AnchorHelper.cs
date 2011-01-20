using Microsoft.Xna.Framework;

namespace Xebab.Helpers
{
	public enum RectAnchors
	{
		TopLeft, Top, TopRight,
		Left, Center, Right,
		BottomLeft, Bottom, BottomRight
	}

	public static class AnchorHelper
	{
		public static Vector2 GetTopLeftPosition(Size shape, Vector2 anchoredPosition, RectAnchors anchor)
		{
			switch (anchor)
			{
				case RectAnchors.Top:
					return anchoredPosition - new Vector2(shape.Width / 2, 0);
				case RectAnchors.TopRight:
					return anchoredPosition - new Vector2(shape.Width, 0);
				case RectAnchors.Left:
					return anchoredPosition - new Vector2(0, shape.Height / 2);
				case RectAnchors.Center:
					return anchoredPosition - new Vector2(shape.Width / 2, shape.Height / 2);
				case RectAnchors.Right:
					return anchoredPosition - new Vector2(shape.Width, shape.Height / 2);
				case RectAnchors.BottomLeft:
					return anchoredPosition - new Vector2(0, shape.Height);
				case RectAnchors.Bottom:
					return anchoredPosition - new Vector2(shape.Width / 2, shape.Height);
				case RectAnchors.BottomRight:
					return anchoredPosition - new Vector2(shape.Width, shape.Height);
				default:
					return anchoredPosition;
			}
		}

		public static Vector2 GetAnchoredPosition(Size shape, Vector2 topleftPosition, RectAnchors anchor)
		{
			switch (anchor)
			{
				case RectAnchors.Top:
					return topleftPosition + new Vector2(shape.Width / 2, 0);
				case RectAnchors.TopRight:
					return topleftPosition + new Vector2(shape.Width, 0);
				case RectAnchors.Left:
					return topleftPosition + new Vector2(0, shape.Height / 2);
				case RectAnchors.Center:
					return topleftPosition + new Vector2(shape.Width / 2, shape.Height / 2);
				case RectAnchors.Right:
					return topleftPosition + new Vector2(shape.Width, shape.Height / 2);
				case RectAnchors.BottomLeft:
					return topleftPosition + new Vector2(0, shape.Height);
				case RectAnchors.Bottom:
					return topleftPosition + new Vector2(shape.Width / 2, shape.Height);
				case RectAnchors.BottomRight:
					return topleftPosition + new Vector2(shape.Width, shape.Height);
				default:
					return topleftPosition;
			}
		}

	}
}
