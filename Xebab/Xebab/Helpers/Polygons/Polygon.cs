using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Xebab.Model.Polygons
{
	public partial class Polygon
	{
		//vertexes array 
		//(in counter-clockwise order)
		protected Vector2[] verts;

		//edges normals array
		protected Vector2[] norms;

		//projections of the polygon against its normals
		protected Interval[] selfProjections;

		#region PROPERTIES 

		public Vector2[] Vertexes { get { return verts; } }
		public Vector2[] Normals { get { return norms; } }
		public int VertexCount { get { return verts.Length; } }

		#endregion


		public Polygon(Vector2[] verts)
		{
			this.verts = verts;
			ComputeNormals();
			ComputeSelfProjections();
		}


		public bool Hits(Polygon other)
		{
			//First test myself against other polygon...
			for (int i = 0; i < this.VertexCount; i++)
			{
				Interval otherProjection = other.GetProjection(this.norms[i], this.verts[0]);
				if (!this.selfProjections[i].Intersects(otherProjection))
				{
					//other polygon's projection is not intersecting mine
					return false;
				}
			}
			
			//..then test other polygon against me
			for (int i = 0; i < other.VertexCount; i++)
			{
				Interval thisProjection = this.GetProjection(other.norms[i], other.verts[0]);
				if(!other.selfProjections[i].Intersects(thisProjection))
				{
					//my projection is not intersecting other polygon's one
					return false;
				}
			}

			//All projections are respectively intersecting along all axes:
			//polygons are overlapping
			return true;
		}

		public Interval GetProjection(Vector2 direction)
		{
			return GetProjection(direction, Vector2.Zero);
		}

		public Interval GetProjection(Vector2 direction, Vector2 origin)
		{
			float prj = (verts[0] - origin).Project(direction);

			Interval prjInterval = new Interval { start = prj, end = prj };

			for (int v = 1; v < VertexCount; v++)
			{
				prj = (verts[v] - origin).Project(direction);
				if (prj < prjInterval.start) prjInterval.start = prj;
				if (prj > prjInterval.end) prjInterval.end = prj;
			}

			return prjInterval;
		}


		public Vector2 NextVertex(int index)
		{
			if (index == VertexCount - 1)
				return verts[0];
			else
				return verts[index + 1];
		}

		public void Translate(Vector2 shift)
		{
			// simply translate each vertex
			for(int i = 0; i < VertexCount; i++) 
			{
				verts[i] += shift;
			}
		}

		public Vector2 GetTopLeftPosition()
		{
			Vector2 topleft = new Vector2(float.MaxValue);

			foreach (Vector2 v in verts)
			{
				if (v.X < topleft.X) topleft.X = v.X;
				if (v.Y < topleft.Y) topleft.Y = v.Y;
			}

			return topleft;
		}

		public Rectangle GetBoundingBox()
		{
			Vector2 topLeft = GetTopLeftPosition();
			float width = 0, height = 0;

			foreach (Vector2 v in verts)
			{
				if (v.X - topLeft.X > width) width = v.X - topLeft.X;
				if (v.Y - topLeft.Y > height) height = v.Y - topLeft.Y;
			}

			return new Rectangle((int)topLeft.X, (int)topLeft.Y, (int)width, (int)height);
		}


		private void ComputeNormals()
		{
			int n = VertexCount;
			norms = new Vector2[n];

			// compute edges normals
			for (int i = 0; i < n; i++)
			{
				Vector2 edge = NextVertex(i) - verts[i];
				edge.Normalize();
				norms[i] = edge.Perpendicular();
			}
		}

		private void ComputeSelfProjections()
		{
			int n = VertexCount;
			selfProjections = new Interval[n];

			for (int i = 0; i < n; i++)
				selfProjections[i] = GetProjection(norms[i], verts[0]);
		}
	}


	
}
