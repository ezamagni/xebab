using System.Collections.Generic;
using System.Xml;
using C3.XNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Xebab;
using Xebab.Graphics.Camera;
using Xebab.Helpers;
using Xebab.Helpers.Extensions;
using Xebab.Helpers.Polygons;
using System;
using System.Globalization;

namespace DemoTest.Editor
{
	class GameEditor : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		Scene scene;
		Camera camera;
		Level level;

		List<Polygon> polys;

		Polygon activePolygon;
		bool cursorHandleSet = false;
		Vector2 cursorHandle;

		//crosshair
		Vector2 cursor;
		Texture2D crosshair;
		Vector2 crosshairCenter;


		public GameEditor()
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = 800;
			graphics.PreferredBackBufferHeight = 600;
			Content.RootDirectory = "Content";
		}

		protected override void Initialize()
		{
			polys = new List<Polygon>();
			base.Initialize();
		}

		protected override void LoadContent()
		{
			// Load crosshair
			crosshair = Content.Load<Texture2D>("crosshair");
			crosshairCenter = new Vector2(crosshair.Width / 2, crosshair.Height / 2);

			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			//Initialize level
			Texture2D[,] tiles = new Texture2D[2, 2];
			tiles[0, 0] = Content.Load<Texture2D>("t1");
			tiles[0, 1] = Content.Load<Texture2D>("t2");
			tiles[1, 0] = Content.Load<Texture2D>("t3");
			tiles[1, 1] = Content.Load<Texture2D>("t4");
			level = new Level(tiles);

			//Initialize scene
			camera = new Camera(Vector2.Zero,
				new Size(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
			scene = new Scene(this, spriteBatch, level, camera);
			Components.Add(scene);
		}

		protected override void Update(GameTime gameTime)
		{
			KeyboardState keyb = Keyboard.GetState();
			MouseState mouse = Mouse.GetState();
			cursor = new Vector2(mouse.X, mouse.Y);

			// Allows the game to exit
			if (keyb.IsKeyDown(Keys.Escape))
				this.Exit();

			//reset camera position
			if (keyb.IsKeyDown(Keys.Space))
				camera.SetViewportPosition(Vector2.Zero);

			//Handle camera dragging
			MoveCamera(mouse);

			Vector2 relativeCursor = cursor + camera.ViewportPosition;
			//If user clicked the canvas...
			if (mouse.LeftButton == ButtonState.Pressed)
			{
				if ((keyb.IsKeyDown(Keys.LeftShift) || keyb.IsKeyDown(Keys.RightShift))
					&& activePolygon != null)
				{
					// move the entire polygon to mouse cursor
					activePolygon.Translate(relativeCursor - activePolygon.GetTopLeftPosition());
				}
				else
				{
					BuildPolys(relativeCursor, keyb);
				}
			}

			//If user is inserting poly and clicked enter, finish drawing poly (if it's valid)
			if (keyb.IsKeyDown(Keys.Enter) && activePolygon != null 
				&& activePolygon.VertexCount >= 3)
			{
				activePolygon = null;
			}

			//If user has an active poly and presses cancel, remove the selecte polygon
			if (keyb.IsKeyDown(Keys.Delete) && activePolygon != null)
			{
				polys.Remove(activePolygon);
				activePolygon = null;
			}

			if (keyb.IsKeyDown(Keys.Back))
			{
				activePolygon = null;
			}

			//Save work when user presses F8
			if (keyb.IsKeyDown(Keys.F8))
			{
				SaveLevel();
			}

			//Load level when user presses F6
			if (keyb.IsKeyDown(Keys.F6))
			{
				LoadLevel();
			}

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);
			base.Draw(gameTime);

			spriteBatch.Begin();

			// Draw polygons
			foreach (Polygon p in polys)
			{
				DrawPolygon(p, Color.Purple);
			}

			if (activePolygon != null)
			{
				DrawPolygon(activePolygon, Color.OrangeRed);
			}

			// Draw crosshair
			spriteBatch.Draw(crosshair, cursor - crosshairCenter, Color.White);

			spriteBatch.End();
		}

		private void DrawPolygon(Polygon p, Color color, int thickness = 1)
		{
			int n = p.Vertexes.Length;

			for (int i = 0; i < n; i++)
			{
				Primitives2D.DrawLine(spriteBatch, p.Vertexes[i] - camera.ViewportPosition,
					p.NextVertex(i) - camera.ViewportPosition, color, thickness);
			}
		}

		private void BuildPolys(Vector2 rCursor, KeyboardState keyb)
		{
			if (keyb.IsKeyDown(Keys.LeftControl))
			{
				//did user clicked over a pre-existent polygon?
				foreach (Polygon p in polys)
				{
					if (p.GetBoundingBox().Contains(rCursor.ToPoint()))
					{
						activePolygon = p;
						break;
					}
				}
			}
			//are we building a polygon from scratch?
			else if (activePolygon == null)
			{
				activePolygon = new Polygon(new Vector2[1] { rCursor });
			}
			else if (activePolygon != null)
			{
				//.. add another vertex to the polygon vertex list...
				Vector2[] vertexes = new Vector2[activePolygon.VertexCount + 1];
				activePolygon.Vertexes.CopyTo(vertexes, 0);
				vertexes[vertexes.Length - 1] = rCursor;

				//.. and recreate it
				polys.Remove(activePolygon);
				activePolygon = Polygon.ConvexHull(vertexes);
				polys.Add(activePolygon);
			}
		}

		private void MoveCamera(MouseState mouse)
		{
			//move camera using mouse
			const float MOUSE_CAMERA_SENSITIVITY = 0.5f;
			if (mouse.RightButton == ButtonState.Pressed)
			{
				if (cursorHandleSet)
				{
					Vector2 cameraMovement = cursorHandle - cursor;
					camera.SetViewportPosition(camera.ViewportPosition + cameraMovement * MOUSE_CAMERA_SENSITIVITY);
				}
				cursorHandleSet = true;
				cursorHandle = cursor;
			}
			else
			{
				cursorHandleSet = false;
			}

			if(cursorHandleSet) return;
			//continuously adjust the camera position if outside bounds
			const float CAMERA_BUMP_AMOUNT = 0.17f;
			const float CAMERA_THRESHOLD = 5f;
			Vector2 affected = Vector2.Zero;
			if (camera.ViewportPosition.X < 0 || camera.ViewportPosition.Y < 0)
			{
				if (camera.ViewportPosition.X < 0) affected.X = CAMERA_BUMP_AMOUNT;
				if (camera.ViewportPosition.Y < 0) affected.Y = CAMERA_BUMP_AMOUNT;

				if (affected.X > 0 && camera.ViewportPosition.X > -CAMERA_THRESHOLD)
				{
					camera.SetViewportPosition(new Vector2(0, camera.ViewportPosition.Y));
				}
				else if (affected.Y > 0 && camera.ViewportPosition.Y > -CAMERA_THRESHOLD)
				{
					camera.SetViewportPosition(new Vector2(camera.ViewportPosition.X, 0));
				}
				else
				{
					Vector2 adjustedPos = camera.ViewportPosition;
					adjustedPos += -adjustedPos * affected;
					camera.SetViewportPosition(adjustedPos);
				}
			}
			else if (camera.ViewportPosition.X + camera.Viewport.Size.Width > level.Width
			  || camera.ViewportPosition.Y + camera.Viewport.Size.Height > level.Height)
			{
				if (camera.ViewportPosition.X + camera.Viewport.Size.Width > level.Width) 
					affected.X = CAMERA_BUMP_AMOUNT;
				if (camera.ViewportPosition.Y + camera.Viewport.Size.Height > level.Height) 
					affected.Y = CAMERA_BUMP_AMOUNT;

				Vector2 margin = new Vector2(camera.ViewportPosition.X + camera.Viewport.Size.Width - level.Width,
					camera.ViewportPosition.Y + camera.Viewport.Size.Height - level.Height);

				if (affected.X > 0 && margin.X < CAMERA_THRESHOLD)
				{
					camera.SetViewportPosition(
						new Vector2(level.Width - camera.Viewport.Size.Width, camera.ViewportPosition.Y));
				}
				else if (affected.Y > 0 && margin.Y < CAMERA_THRESHOLD)
				{
					camera.SetViewportPosition(
						new Vector2(camera.ViewportPosition.X, level.Height - camera.Viewport.Size.Height));
				}
				else
				{
					Vector2 adjustedPos = camera.ViewportPosition;
					adjustedPos += -margin * affected;
					camera.SetViewportPosition(adjustedPos);
				}
			}
		}

		private void SaveLevel()
		{
			System.Windows.Forms.SaveFileDialog fileDialog = new System.Windows.Forms.SaveFileDialog();
			fileDialog.Filter = "XML Document|*.xml";
			fileDialog.Title = "Save level";
			fileDialog.ShowDialog();

			if (fileDialog.FileName == "") return;

			// Create a new XML file
			XmlTextWriter xmlWriter = new XmlTextWriter(fileDialog.FileName, null);
			xmlWriter.WriteStartDocument();
			xmlWriter.WriteStartElement("Obstacles");
			foreach (Polygon p in polys)
			{
				xmlWriter.WriteStartElement("Polygon");
				foreach (Vector2 v in p.Vertexes)
				{
					xmlWriter.WriteStartElement("Vertex");
						xmlWriter.WriteStartElement("X");
							xmlWriter.WriteValue(v.X);
						xmlWriter.WriteEndElement();
						xmlWriter.WriteStartElement("Y");
							xmlWriter.WriteValue(v.Y);
						xmlWriter.WriteEndElement();
					xmlWriter.WriteEndElement();
				}
				xmlWriter.WriteEndElement();
			}
			xmlWriter.WriteEndElement();

			// Close XML file
			xmlWriter.WriteEndDocument();
			xmlWriter.Close();
		}

		private void LoadLevel()
		{
			System.Windows.Forms.OpenFileDialog fileDialog = new System.Windows.Forms.OpenFileDialog();
			fileDialog.Filter = "XML Document|*.xml";
			fileDialog.Title = "Load level";
			fileDialog.ShowDialog();

			if (fileDialog.FileName == "") return;

			XmlTextReader xmlReader = new XmlTextReader(fileDialog.FileName);
			
			List<Polygon> pols = new List<Polygon>();
			xmlReader.Read();
			xmlReader.Read();
			xmlReader.Read();
			while (xmlReader.Name == "Polygon") //Read polygon
			{
				List<Vector2> vertexes = new List<Vector2>();
				xmlReader.Read();	//Read Vertex
				while (xmlReader.Name == "Vertex")
				{
					Vector2 v = new Vector2();
					xmlReader.Read();	//Read X
					xmlReader.Read();
					//v.X = float.Parse(xmlReader.Value, System.Globalization.NumberStyles.);
					v.X = (float)Convert.ToDouble(xmlReader.Value, CultureInfo.InvariantCulture.NumberFormat);
					xmlReader.Read();	//Read Y
					xmlReader.Read();
					xmlReader.Read();
					v.Y = (float)Convert.ToDouble(xmlReader.Value, CultureInfo.InvariantCulture.NumberFormat);
					vertexes.Add(v);

					xmlReader.Read();
					xmlReader.Read();
					xmlReader.Read();	//Read next node
				}

				//polygon ends here
				pols.Add(new Polygon(vertexes.ToArray()));
				xmlReader.Read();
			}

			//EOF
			xmlReader.Close();

			polys = pols;
			activePolygon = null;
			camera.SetViewportPosition(Vector2.Zero);
		}

	}
}
