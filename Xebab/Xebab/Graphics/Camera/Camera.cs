using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Xebab.Graphics.Sprites;
using Xebab.Helpers;

namespace Xebab.Graphics.Camera
{
	
    public partial class Camera 
    {
        protected CameraViewport viewport;
        

        //TODO controllare che il viewport non fuoriesca dallo schermo
        public Vector2 ViewportPosition 
        {
            get { return viewport.Position; }
        }
        
        public Size Size 
        {
            get { return viewport.Size; }
        }

        public CameraViewport Viewport 
        {
            get { return viewport; }
        }

        public Rectangle ViewBox
        {
            get { return new Rectangle((int)ViewportPosition.X, (int)ViewportPosition.Y, Size.Width, Size.Height); }
        }


        public Camera(Vector2 position, Size viewSize)
        {
            this.viewport = new CameraViewport(viewSize);

        }

        public Vector2 GetViewportPosition(RectAnchors anchor)
        {
            return AnchorHelper.GetAnchoredPosition(Size, ViewportPosition, anchor);
        }

        public void SetViewportPosition(Vector2 position)
        {
            this.viewport.Position = position;
        }

        public void SetViewportPosition(Vector2 position, RectAnchors anchor)
        {
            this.viewport.Position = AnchorHelper.GetTopLeftPosition(Size, position, anchor);
        }

        public void Update(GameTime gameTime)
        {
            
        }

    }
}