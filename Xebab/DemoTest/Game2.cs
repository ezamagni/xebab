using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Xebab;
using Xebab.Behaviors.Sprites;
using Xebab.Graphics.Camera;
using Xebab.Graphics.Sprites;
using Xebab.Helpers;
using Xebab.Helpers.Polygons;
using C3.XNA;

namespace DemoTest
{
	class Game2 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		Scene scene;
		Camera camera;
		AnimatedSprite sprite;
		LevelObs level;

		public Game2()
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = 800;
			graphics.PreferredBackBufferHeight = 600;
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			//Initialize level
			Texture2D[,] tiles = new Texture2D[2, 2];
			tiles[0, 0] = Content.Load<Texture2D>("t1");
			tiles[0, 1] = Content.Load<Texture2D>("t2");
			tiles[1, 0] = Content.Load<Texture2D>("t3");
			tiles[1, 1] = Content.Load<Texture2D>("t4");

			level = new LevelObs(tiles, LoadObstacles());

			//Initialize scene
			camera = new Camera(Vector2.Zero, new Size(800, 600));
			scene = new Scene(this, spriteBatch, level, camera);
			Components.Add(scene);

			//Temporary sprite shapeset
			Polygon shape = 
				new Polygon(new Vector2[] { new Vector2(5, 38),
											new Vector2(24, 38),
											new Vector2(26, 48),
											new Vector2(1, 48)});

			List<Polygon> shapeSet = new List<Xebab.Helpers.Polygons.Polygon>(1);
			shapeSet.Add(shape);

			//Initialize sprite
			Texture2D spritesheet = Content.Load<Texture2D>("guy01");
			sprite = new AnimatedSprite(scene.ContentHandler, shapeSet,
				spritesheet, new Size(31, 48));
			scene.ContentHandler.SpriteHandler.SpawnSprite(sprite);
			sprite.Position = new Vector2(300);
			sprite.AddBehavior(new CameraFollowerBehavior(sprite, camera, level));
		}

		protected override void Update(GameTime gameTime)
		{
			KeyboardState keyb = Keyboard.GetState();
			MouseState mouse = Mouse.GetState();

			// Allows the game to exit
			if (keyb.IsKeyDown(Keys.Escape))
				this.Exit();

			//Move sprite or camera
			const float SPRITE_SPEED = 2.4f;
			const float CAMERA_SPEED = 12f;
			const int STILL_FRAME = 1;
			int xDir = 0, yDir = 0;

			if (keyb.IsKeyDown(Keys.Left))
				xDir = -1;
			else if (keyb.IsKeyDown(Keys.Right))
				xDir = 1;
			if (keyb.IsKeyDown(Keys.Up))
				yDir = -1;
			else if (keyb.IsKeyDown(Keys.Down))
				yDir = 1;

			Vector2 movement = new Vector2(xDir, yDir);

			if (keyb.IsKeyDown(Keys.LeftShift))
			{
				//move camera
				camera.SetViewportPosition(camera.ViewportPosition +
					 movement * CAMERA_SPEED);
				System.Console.WriteLine("X: {0}, Y: {1}", camera.ViewportPosition.X, camera.ViewportPosition.Y);
			}
			else
			{
				sprite.Animation.Start();
				if (xDir == 0 && yDir == 0)
				{
					//standing still
					sprite.Animation.CurrentFrame = STILL_FRAME;
					sprite.Animation.Stop();
				}
				else if (xDir == 1 && yDir == -1)
				{
					sprite.Animation.CurrentAnimation = 3;
				}
				else if (xDir == 1)
				{
					sprite.Animation.CurrentAnimation = 0;
				}
				else if (xDir == -1 && yDir == -1)
				{
					sprite.Animation.CurrentAnimation = 2;
				}
				else if (xDir == -1)
				{
					sprite.Animation.CurrentAnimation = 1;
				}
				else if (yDir == -1)
				{
					sprite.Animation.CurrentAnimation = 2;
				}
				else
				{
					sprite.Animation.CurrentAnimation = 0;
				}

				//Apply sprite translation
				sprite.Position += movement * SPRITE_SPEED;

				//Check for collisions
				bool collide = false;
				foreach (Polygon p in sprite.ShapeSet)
				{
					foreach (Polygon obstacle in level.Obstacles)
					{
						if (p.Hits(obstacle))
						{
							collide = true;
							break;
						}
					}
					if (collide) break;
				}

				//If sprite collides revert its movement
				if(collide) sprite.Position -= movement * SPRITE_SPEED;
			}

			if (keyb.IsKeyDown(Keys.Space)) camera.SetViewportPosition(Vector2.Zero);

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Purple);
			base.Draw(gameTime);

			//DEBUG
			if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
			{
				spriteBatch.Begin();

				// Draw polygons
				foreach (Polygon p in level.Obstacles)
				{
					DrawPolygon(p, Color.Purple, 2);
				}
				foreach (Polygon s in sprite.ShapeSet)
				{
					DrawPolygon(s, Color.Red);
				}

				spriteBatch.End();
			}
		}


		private List<Polygon> LoadObstacles()
		{
			XmlTextReader xmlReader = new XmlTextReader(@"C:\Users\Enrico\Documents\Visual Studio 2010\Projects\xebab\Xebab\DemoTestContent\obstacles.xml");

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

			return pols;
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
	}
}
