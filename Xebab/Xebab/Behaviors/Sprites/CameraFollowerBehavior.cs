using System;
using Microsoft.Xna.Framework;
using Xebab.Graphics.Camera;
using Xebab.Graphics.Sprites;
using Xebab.Helpers;

namespace Xebab.Behaviors.Sprites
{
	public class CameraFollowerBehavior : Behavior<Sprite>
	{
		private Camera camera;
		private Level level;
		private Vector2 spriteCenter;


		public CameraFollowerBehavior(Sprite subject, Camera camera, Level level)
			:base(subject)
		{
			this.camera = camera;
			this.level = level;
			Rectangle spriteBox = subject.BoundingBox;
			this.spriteCenter = new Vector2(spriteBox.Width / 2, spriteBox.Height / 2);
		}

		
		public override void Update(GameTime gameTime)
		{
			Size viewportSize = camera.Viewport.Size;

			//center subject into view
			Vector2 cameraPos = AnchorHelper.GetTopLeftPosition(viewportSize,
				Subject.Position + spriteCenter, RectAnchors.Center);

			//correct camera position
			if (cameraPos.X < 0)
				cameraPos.X = 0;
			else if (cameraPos.X + viewportSize.Width > level.Width)
				cameraPos.X = level.Width - viewportSize.Width;

			if (cameraPos.Y < 0)
				cameraPos.Y = 0;
			else if (cameraPos.Y + viewportSize.Height > level.Height)
				cameraPos.Y = level.Height - viewportSize.Height;

			camera.SetViewportPosition(cameraPos);
		}
	}
}
