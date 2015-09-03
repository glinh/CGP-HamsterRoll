using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Game1
{
    public class Camera : Microsoft.Xna.Framework.GameComponent
    {
        // Camera vectors
        public Vector3 cameraPosition { get; protected set; }
        Vector3 cameraDirection;
        Vector3 cameraUp;
        MouseState prevMouseState;

        Vector3 initialHeight = new Vector3(0, 0, 0);
        Vector3 initialDistance = new Vector3(0, 400, 0);
        Vector3 ballPosition;
        Sphere cube;



        public Matrix view { get; protected set; }
        public Matrix projection { get; protected set; }
        public Camera(Game game, Vector3 pos, Vector3 target, Vector3 up)
            : base(game)
        {
            // Build camera view matrix
            cameraPosition = pos;
            cameraDirection = target - pos;
            cameraDirection.Normalize();
            cameraUp = up;
            CreateLookAt();
            CreateProjection();
        }
        public override void Initialize()
        {
            // Set mouse position and do initial get state
            Mouse.SetPosition(Game.Window.ClientBounds.Width / 2,
                Game.Window.ClientBounds.Height / 2);
            
            Vector3 variable = Vector3.Cross(cameraUp, cameraDirection);
            variable.Normalize();

            cameraDirection = Vector3.Transform(cameraDirection,
                 Matrix.CreateFromAxisAngle(variable, (MathHelper.PiOver4 / 100) *
                 (Game.Window.ClientBounds.Height / 2)));
             
            prevMouseState = Mouse.GetState();
            
            base.Initialize();
        }
        
        
        public Vector3 CameraSideVector()
        {
            Vector3 v = Vector3.Cross(cameraUp, cameraDirection);
            v.Normalize();
            return v;
        }
        public Vector3 CameraDirection()
        {
            Vector3 v = cameraDirection;
            v.Y = 0;
            v.Normalize();
            return v;
        }

        public Vector3 CameraUpVector()
        {
            Vector3 v = cameraUp;
            v.Normalize();
            return v;
        }

        public void passBallPosition(Vector3 position)
        {
            ballPosition = position;
        }

        public override void Update(GameTime gameTime)
        {
            // Move forward/backward
            Vector3 v = ballPosition;
            v.Y = 0;
            cameraPosition = initialDistance + v;

            /*
            if(!cube.isJumping())
            cameraDirection = ballPosition - cameraPosition;
            */

            // Reset prevMouseState
            prevMouseState = Mouse.GetState(  );
            // Recreate the camera view matrix
            CreateLookAt();

            base.Update(gameTime);
        }

        private void CreateLookAt()
        {           
            view = Matrix.CreateLookAt(cameraPosition,
                cameraPosition + cameraDirection, cameraUp);            

        }
        public void CreateProjection()
        {
            projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                (float)Game.Window.ClientBounds.Width /
                (float)Game.Window.ClientBounds.Height,
                1, 3000);
        }

        public void setCube(Sphere cube)
        {
            this.cube = cube;
        }
    }
}
