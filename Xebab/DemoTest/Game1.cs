using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Xebab;
using Xebab.Graphics.Camera;
using Xebab.Helpers;

namespace DemoTest
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		Level level;
		Camera camera;

		//mouse handling
		Vector2 cursor;
		Vector2 cursorHandle;
		bool cursorHandleSet;


		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			graphics.PreferredBackBufferWidth = 800;
			graphics.PreferredBackBufferHeight = 600;
			IsMouseVisible = true;

		}

		protected override void Initialize()
		{
			camera = new Camera(Vector2.Zero, new Size(800, 600));

			base.Initialize();
		}


		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			Texture2D[,] tiles = new Texture2D[2, 2];
			tiles[0, 0] = Content.Load<Texture2D>("t1");
			tiles[0, 1] = Content.Load<Texture2D>("t2");
			tiles[1, 0] = Content.Load<Texture2D>("t3");
			tiles[1, 1] = Content.Load<Texture2D>("t4");

			level = new Level(tiles);
		}


		protected override void Update(GameTime gameTime)
		{
			KeyboardState keyb = Keyboard.GetState();
			MouseState mouse = Mouse.GetState();
			cursor = new Vector2(mouse.X, mouse.Y);

			// Allows the game to exit
			if (keyb.IsKeyDown(Keys.Escape))
				this.Exit();

			//move camera using keyboard
			const float KEYBOARD_CAMERA_ACCELERATION = 7.5f;
			int xDir = 0, yDir = 0;
			if (keyb.IsKeyDown(Keys.Left)) xDir = -1;
			if (keyb.IsKeyDown(Keys.Right)) xDir = 1;
			if (keyb.IsKeyDown(Keys.Up)) yDir = -1;
			if (keyb.IsKeyDown(Keys.Down)) yDir = 1;
			camera.SetViewportPosition(camera.ViewportPosition + new Vector2(xDir * KEYBOARD_CAMERA_ACCELERATION, yDir * KEYBOARD_CAMERA_ACCELERATION));

			//move camera using mouse
			const float MOUSE_CAMERA_SENSITIVITY = 0.5f;
			if (mouse.LeftButton == ButtonState.Pressed)
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

			//reset camera position
			if(keyb.IsKeyDown(Keys.Space))
				camera.SetViewportPosition(Vector2.Zero);

			//continuously adjust the camera position if outside bounds
			const float CAMERA_BUMP_AMOUNT = 0.17f;
			if ((camera.ViewportPosition.X < 0 || camera.ViewportPosition.Y < 0) && !cursorHandleSet)
			{
				Vector2 affected = Vector2.Zero;
				if (camera.ViewportPosition.X < 0) affected.X = CAMERA_BUMP_AMOUNT;
				if (camera.ViewportPosition.Y < 0) affected.Y = CAMERA_BUMP_AMOUNT;

				if (affected.X > 0 && camera.ViewportPosition.X > -5)
				{
					camera.SetViewportPosition(new Vector2(0, camera.ViewportPosition.Y));
				}
				else if (affected.Y > 0 && camera.ViewportPosition.Y > -5)
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

			base.Update(gameTime);
		}


		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			spriteBatch.Begin();
			level.Draw(spriteBatch, camera.Viewport, 0);
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
