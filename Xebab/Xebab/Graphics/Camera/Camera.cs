using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Xebab.Graphics.Sprites;
using Xebab.Helpers;

namespace Xebab.Graphics.Camera
{
    public enum CameraMovementMode
    {
        Linear,
        Bicubic
    }

	/*
	 * TODO:
	 * Separari i diversi comportamenti per il movimento
	 * dalla classe principale
	 */
	
    public class Camera 
    {
        public int spritesDrawn = 0; //DEBUG

        //current camera speed
        //protected float speed = 0f;

        //current distance from target
        //protected float targetDistance = 0f;

        //current target direction
        protected Vector2 direction;

        private CameraMovementMode curMovementMode;
        public CameraMovementMode MovementMode { get; set; }

        //CONSTANTS
        const float MAX_CAMERA_SPEED = 12f;
        const float DEFAULT_ACCELERATION = 0.08f;

        //ACCELERATION PARAMETERS
        private int curCycle;
        private int totCycles;
        private int accCycles;
        private int decCycles;
        private float speedMax;
        private double space = 0;
        private double spaceTraveled = 0;
        private double startSpeed = 0;

        #region PROPERTIES

        private bool isDiscretePositionable = false;
        public bool IsDiscretePositionable
        {
            get
            {
                return isDiscretePositionable;
            }
            set
            {
                isDiscretePositionable = value;
                if (isDiscretePositionable)
                    ViewPortPosition = viewportPosition;
            }
        }

        public Vector2 Position { get; set; } //TODO controllare che il viewport non esca dallo schermo

        private Vector2 viewportPosition;
        public Vector2 ViewPortPosition
        {
            get
            {
                return viewportPosition;
            }
            protected set
            {
                if (isDiscretePositionable)
                {
                    viewportPosition = new Vector2((int)value.X, (int)value.Y);
                }
                else
                {
                    viewportPosition = value;
                }

            }
        }

        //camera'e target
        public Vector2 Target { get; protected set; }

        //dimension of the viewport rectangle drawn by the camera
        public Size ViewportSize { get; set; }

        //current camera View box
        public CameraViewport Viewport
        {
            get
            {
				throw new NotImplementedException();
            }
        }

        //acceleration of the camera
        public float Acceleration { get; set; }

        //tells wether camera is currently targeting a point
        public bool IsTargeting { get; private set; }

        #endregion

        /*
         * CONSTRUCTORS
         */

        public Camera(Scene scene, Vector2 position, Size viewSize)
        {
            this.ViewportSize = viewSize;
            this.Acceleration = DEFAULT_ACCELERATION;
            this.Position = position;
            this.MovementMode = CameraMovementMode.Bicubic;

            //initializes viewport at the requested target
            SetViewportPosition(Vector2.Zero);
        }

        public void Update(GameTime gameTime)
        {
            if (!IsTargeting)
            {
                curMovementMode = MovementMode;
                return;
            }

            switch (curMovementMode)
            {
                case CameraMovementMode.Bicubic:
                    BicubicMovement();
                    break;
                case CameraMovementMode.Linear:
                    LinearMovement();
                    break;
            }

            curCycle++;
            ViewPortPosition += direction * (float)space;
            spaceTraveled += space;
        }

        private void BicubicMovement()
        {
            if (curCycle < accCycles / 2)
            {
                space = (speedMax - startSpeed) / Math.Pow(accCycles, 3) * (4 * Math.Pow(curCycle, 3) + 6 * Math.Pow(curCycle, 2) + 4 * curCycle + 1) + startSpeed;
            }
            else if (curCycle < accCycles)
            {
                space = (speedMax - startSpeed) * ((4 * Math.Pow(curCycle, 3) + 6 * Math.Pow(curCycle, 2) * (1 - 2 * accCycles) + 4 * curCycle * (1 - 3 * accCycles + 3 * Math.Pow(accCycles, 2)) + 1 - 4 * accCycles + 6 * Math.Pow(accCycles, 2)) / Math.Pow(accCycles, 3) - 3) + startSpeed;
            }
            else if (curCycle < decCycles)
            {
                space = speedMax;
            }
            else if (curCycle < (decCycles + totCycles) / 2)
            {
                space = speedMax * ((-4 * Math.Pow(curCycle, 3) - 6 * Math.Pow(curCycle, 2) * (1 - 2 * decCycles) - 4 * curCycle * (1 - 3 * decCycles + 3 * Math.Pow(decCycles, 2)) - 1 + 4 * decCycles - 6 * Math.Pow(decCycles, 2) + 4 * Math.Pow(decCycles, 3)) / Math.Pow(totCycles - decCycles, 3) + 1);
            }
            else if (curCycle < totCycles)
            {
                space = speedMax * (-4 * Math.Pow(curCycle, 3) - 6 * Math.Pow(curCycle, 2) * (1 - 2 * totCycles) - 4 * curCycle * (1 - 3 * totCycles + 3 * Math.Pow(totCycles, 2)) - 1 + 4 * totCycles - 6 * Math.Pow(totCycles, 2) + 4 * Math.Pow(totCycles, 3)) / Math.Pow(totCycles - decCycles, 3);
            }
            else
            {
                viewportPosition = Target;
                ResetParameters();
            }
        }

        private void LinearMovement()
        {
            if (curCycle < accCycles)
            {
                space = (speedMax - startSpeed) / (2 * accCycles) * (2 * curCycle + 1) + startSpeed;
            }
            else if (curCycle < decCycles)
            {
                space = speedMax;
            }
            else if (curCycle < totCycles)
            {
                space = -speedMax / (2 * (totCycles - decCycles)) * (2 * (curCycle - decCycles) + 1) + speedMax;
            }
            else
            {
                viewportPosition = Target;
                ResetParameters();
            }
        }

        public Vector2 GetViewportPosition(RectAnchors anchor)
        {
            return AnchorHelper.GetAnchoredPosition(ViewportSize, ViewPortPosition, anchor);
        }

        //set the target of the top-left corner of the camera overriding any actual target
        public void SetViewportPosition(Vector2 position)
        {
            //instantly move the camera to new target
            ViewPortPosition = position;

            //reset speed and override target
            ResetParameters();
        }

        private void ResetParameters()
        {
            IsTargeting = false;
            space = 0;
            spaceTraveled = 0;
            startSpeed = 0;
        }

        public void SetViewportPosition(Vector2 position, RectAnchors anchor)
        {
            SetViewportPosition(AnchorHelper.GetTopLeftPosition(ViewportSize, position, anchor));
        }

        public void ShiftViewportPosition(Vector2 shift)
        {
            SetViewportPosition(ViewPortPosition + shift);
        }

        public void SetTarget(Vector2 target)
        {
            //Set target
            Target = target;

            //Compute direction and distance
            direction = target - ViewPortPosition;
            float targetDistance = direction.Length();
            direction.Normalize();

            //Compute speed parameters
            ComputeParameters(targetDistance);

        }

        public void SetTarget(Vector2 target, RectAnchors anchor)
        {
            SetTarget(AnchorHelper.GetTopLeftPosition(ViewportSize, target, anchor));
        }

        private void ComputeParameters(float distance)
        {
            const float pxPerSecond = 600;
            const float timeYDilat = 1000;
            const float timeXDilat = 9 / pxPerSecond;
            const float accAdjust = 350f;
            const float decAdjust = 1400f;

            float timeTot;
            float timeAcc;
            float timeDec;

            if (IsTargeting)
            {
                if (MovementMode == CameraMovementMode.Bicubic)
                {
                    //we're already following a target
                    if (curCycle < accCycles / 2)
                    {
                        startSpeed = 4 * (speedMax - startSpeed) * Math.Pow(curCycle, 3) / Math.Pow(accCycles, 3) + startSpeed;
                    }
                    else if (curCycle < accCycles)
                    {
                        startSpeed = (speedMax - startSpeed) * (4 * Math.Pow(curCycle - accCycles, 3) / Math.Pow(accCycles, 3) + 1) + startSpeed;
                    }
                    else if (curCycle < decCycles)
                    {
                        startSpeed = speedMax;
                    }
                    else if (curCycle < (decCycles + totCycles) / 2)
                    {
                        startSpeed = speedMax * (-4 * Math.Pow(curCycle - decCycles, 3) / Math.Pow(totCycles - decCycles, 3) + 1);
                    }
                    else
                    {
                        startSpeed = -4 * speedMax * Math.Pow(curCycle - totCycles, 3) / Math.Pow(totCycles - decCycles, 3);
                    }

                }
                else
                {
                    if (curCycle < accCycles)
                    {
                        startSpeed = (speedMax - startSpeed) * curCycle / accCycles + startSpeed;
                    }
                    else if (curCycle < decCycles)
                    {
                        startSpeed = speedMax;
                    }
                    else
                    {
                        startSpeed = speedMax * (1 - (curCycle - decCycles) / (totCycles - decCycles));
                    }
                }

                timeTot = (timeYDilat * (float)Math.Log10(timeXDilat * (distance + spaceTraveled) + 1)) - curCycle / 0.06f;
            }
            else
            {
                startSpeed = 0;
                timeTot = (timeYDilat * (float)Math.Log10(timeXDilat * distance + 1));
            }

            timeAcc = accAdjust * (float)Math.Log(timeTot * (1 / (2 * accAdjust)) + 1);
            timeDec = timeTot - decAdjust * (float)Math.Log(timeTot * (1 / (2 * decAdjust)) + 1);
            totCycles = (int)(timeTot * 0.06f);
            curCycle = 0;
            IsTargeting = true;
            spaceTraveled = 0;

            if (totCycles < 5)
            {
                SetViewportPosition(Target);
                totCycles = 0;
                spaceTraveled = 0;
                return;
            }

            accCycles = (int)(timeAcc * 0.06f);
            decCycles = (int)(timeDec * 0.06f);

            if (accCycles <= 3)
            {
                accCycles = 2;
            }
            else
            {
                timeAcc = (int)(timeAcc / 2) * 2;
            }

            if (decCycles >= totCycles - 2)
            {
                decCycles = totCycles - 2;
            }
            else
            {
                decCycles = totCycles - 2 * ((int)((totCycles - decCycles) / 2) + 1);
            }

            speedMax = (2 * distance - (float)startSpeed * accCycles) / (totCycles + decCycles - accCycles);

        }
    }
}