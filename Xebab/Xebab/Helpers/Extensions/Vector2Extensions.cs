using Microsoft.Xna.Framework;

namespace Xebab.Helpers.Extensions
{
	public static class Vector2Extensions
	{

		/// <summary>
		/// Calculates projection of a vector along a given direction
		/// </summary>
		/// <param name="vector">The vector whose projection is to compute</param>
		/// <param name="direction">The direction of the projection</param>
		public static float Project(this Vector2 vector, Vector2 direction)
		{
			//we need direction to be a unit vector
			if (direction.Length() != 1) direction.Normalize();

			return Vector2.Dot(vector, direction);
		}

		/// <summary>
		/// Calculates the perpendicular vector of a given Vector2
		/// </summary>
		/// <param name="vector">The vector whose perpendicular is to compute</param>
		public static Vector2 Perpendicular(this Vector2 vector)
		{
			return new Vector2(vector.Y, -vector.X);
		}


		/// <summary>
		/// 2D cross product of OA and OB vectors, i.e. z-component of their 3D cross product.
		/// Returns a positive value, if OAB makes a counter-clockwise turn,
		/// negative for clockwise turn, and zero if the points are collinear.
		/// </summary>
		public static float Cross(Vector2 O, Vector2 A, Vector2 B)
		{
			return (A.X - O.X) * (B.Y - O.Y) - (A.Y - O.Y) * (B.X - O.X);
		}

		/// <summary>
		/// Vector2 comparer for the Convex Hull monotone chain algorithm
		///</summary>
		public static int CompareLeftToRight(Vector2 a, Vector2 b)
		{
			if (a.X < b.X)
			{
				return -1;
			}
			else if (a.X > b.X)
			{
				return 1;
			}
			else
			{
				if (a.Y < b.Y)
				{
					return -1;
				}
				else
				{
					return 1;
				}
			}
		}

		/// <summary>
		/// Turns the current Vector2 into a Point structure
		/// </summary>
		public static Point ToPoint(this Vector2 vect)
		{
			return new Point((int)vect.X, (int)vect.Y);
		}

	}
}
