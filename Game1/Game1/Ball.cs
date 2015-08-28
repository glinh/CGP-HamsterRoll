using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1
{
    class Ball : BasicModel
    {
        Camera camera;
        MouseState previousState;
        public Vector3 ballPosition { get; protected set; }

        //Movement-related variables;
        float xSpeed;
        float zSpeed;
        float speed = 0.03f;
        const float friction = 0.015f;

        //Jump-related variable
        bool onJump = false;
        float jumpSpeed = 0;
        float initialJumpSpeed = .03f;
        Vector3 initialHeight = new Vector3(0, 0, 0);

        public Ball(Model model, Camera camera)
            : base(model,camera)
        {
            this.camera = camera;
        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {
            translation = Matrix.CreateTranslation(0, 3.2f, 0);
            base.Draw(device, camera);
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();
            ballPosition = world.Translation;
            
            //Rotation
            rotation *= Matrix.CreateRotationX((MathHelper.PiOver4 / 15) * (zSpeed));
            rotation *= Matrix.CreateRotationZ((MathHelper.PiOver4 / 15) * (xSpeed));

            /*
            //Scaling
            if (mouseState.ScrollWheelValue > previousState.ScrollWheelValue)
                worldScale *= Matrix.CreateScale(1.1f);
            if (mouseState.ScrollWheelValue < previousState.ScrollWheelValue)
                worldScale *= Matrix.CreateScale(.9f);
            */
            //Translation

            //Stopping the object completely after friction
            if (Math.Abs(xSpeed) < 0.015f)
            {
                xSpeed = 0;
            }
            if (Math.Abs(zSpeed) < 0.015f)
            {
                zSpeed = 0;
            }

            //Movement movement
            if (keyboardState.IsKeyDown(Keys.W))
            {
                if (zSpeed > 0)
                {
                    zSpeed -= speed * 2;
                }
                else
                {
                    zSpeed -= speed;
                }
            }

            if (keyboardState.IsKeyDown(Keys.S))
            {
                if (zSpeed < 0)
                {
                    zSpeed += speed * 2;
                }
                else
                {
                    zSpeed += speed;
                }
            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                if (xSpeed > 0)
                {
                    xSpeed -= speed * 2;
                }
                else
                {
                    xSpeed -= speed;
                }
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                if (xSpeed < 0)
                {
                    xSpeed += speed * 2;
                }
                else
                {
                    xSpeed += speed;
                }
            }

            //friction
            if (xSpeed < 0)
            {
                xSpeed += friction;
            }
            if (xSpeed > 0)
            {
                xSpeed -= friction;
            }
            if (zSpeed < 0)
            {
                zSpeed += friction;
            }
            if (zSpeed > 0)
            {
                zSpeed -= friction;
            }

            world *= Matrix.CreateTranslation(xSpeed, 0, zSpeed);

            camera.passBallPosition(ballPosition);
            //Jumping
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && onJump == false)
            {
                onJump = true;
                jumpSpeed = initialJumpSpeed;
            }
            System.Diagnostics.Debug.Write(world.M41 + world.M42 + world.M43 + world.M44);
            if (onJump)
            {
                world *= Matrix.CreateTranslation(Vector3.Up * jumpSpeed * 
                    gameTime.ElapsedGameTime.Milliseconds);
                jumpSpeed -= .0010f;

                if (ballPosition.Y < 0.5)
                {
                    onJump = false;
                }
            }
            
            previousState = Mouse.GetState();
            base.Update(gameTime);
        }

        //public bool isJumping()
        //{
       //     return onJump;
       // }
    }
}
