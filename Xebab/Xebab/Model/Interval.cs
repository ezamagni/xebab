using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xebab.Model
{
	public struct Interval
	{
		public float start;
		public float end;
		public float Length { get { return end - start; } }


		public Interval(float start, float end)
		{
			this.start = start;
			this.end = end;
		}

		public bool Intersects(Interval other)
		{
			float a = Math.Max(this.start, other.start);
			float b = Math.Min(this.end, other.end);

			return (b > a);
		}


		public static float Intersection(Interval i1, Interval i2)
		{
			float a = Math.Max(i1.start, i2.start);
			float b = Math.Min(i1.end, i2.end);

			if (b < a)
				return 0;
			else
				return b - a;
		}

	}
}
