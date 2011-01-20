using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Xebab.Model.Polygons
{
	public partial class Polygon
	{
		// Creates a Polygon by computing the convex hull of a given set of vertexes
		public static Polygon ConvexHull(Vector2[] inputVerts)
		{
			int n = inputVerts.Length;
			int k = 0;
			Vector2[] hull = new Vector2[2 * n];

			// sort vertexes lexicographically
			Array.Sort<Vector2>(inputVerts, Vector2Extensions.CompareLefToRight);

			// build lower hull
			for (int i = 0; i < n; i++)
			{
				while (k >= 2 && Vector2Extensions.Cross(hull[k - 2], hull[k - 1], inputVerts[i]) <= 0) k--;
				hull[k] = inputVerts[i];
				k++;
			}

			// build upper hull
			for (int i = n - 2, t = k + 1; i >= 0; i--)
			{
				while (k >= t && Vector2Extensions.Cross(hull[k - 2], hull[k - 1], inputVerts[i]) <= 0) k--;
				hull[k] = inputVerts[i];
				k++;
			}

			//resize the resulting array to the correct size
			k--;
			Vector2[] resHull = new Vector2[k];
			Array.ConstrainedCopy(hull, 0, resHull, 0, k);

			// return new polygon with the resulting hull
			return new Polygon(resHull);
		}

	}

}
