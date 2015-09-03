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
        HeightmapCollision heightmapCollision;

        //Jump-related variable
        bool onJump = false;
        bool verticalityUp = false;
        float jumpSpeed = 0;
        float fallSpeed = 0.03f;
        float initialJumpSpeed = .03f;
        Vector3 initialHeight = new Vector3(0, 0, 0);

        //Fall Down mechanic
        float xFinal = 0;
        float zFinal = 0;
        float xFall = 0;
        float zFall = 0;
        float finalFall = 0;

        float previousBallZ=-140;

        public Ball(Model model, Camera camera, HeightmapCollision heightmapCollision)
            : base(model,camera)
        {
            this.camera = camera;
            this.heightmapCollision = heightmapCollision;
        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {
            translation = Matrix.CreateTranslation(0, 3.2f, 0);
            base.Draw(device, camera);
        }

        public float heightChecker()
        {
            if (xSpeed >= 0 && zSpeed >= 0)
            {
                return xSpeed + zSpeed;
            }

            if (xSpeed < 0 && zSpeed < 0)
            {
                return -xSpeed - zSpeed;
            }

            if (xSpeed >= 0 && zSpeed < 0)
            {
                return xSpeed - zSpeed;
            }

            if (xSpeed < 0 && zSpeed >= 0)
            {
                return -xSpeed + zSpeed;
            }

            return 0;
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();
            ballPosition = world.Translation;
            
            //Rotation
            rotation *= Matrix.CreateRotationX((MathHelper.PiOver4 / 15) * (zSpeed));
            rotation *= Matrix.CreateRotationZ(-(MathHelper.PiOver4 / 15) * (xSpeed));

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

            if (ballPosition.Y<60 && verticalityUp == false)
            {
                xFinal = xSpeed;
                zFinal = zSpeed;
                world *= Matrix.CreateTranslation(xSpeed, 0, zSpeed);
                world *= Matrix.CreateTranslation(0, heightmapCollision.heightCollision(ballPosition),0);
                fallSpeed = initialJumpSpeed;
            }
            else
            {
                verticalityUp = true;
                world *= Matrix.CreateTranslation(0, heightChecker()/5, 0);
                world *= Matrix.CreateTranslation(Vector3.Up * fallSpeed *
          gameTime.ElapsedGameTime.Milliseconds);
               fallSpeed -= 0.0010f;
               finalFall = fallSpeed;

            }
            System.Diagnostics.Debug.WriteLine(ballPosition.ToString());

            if (ballPosition.Y< 60 && verticalityUp == true)
            {
                xFall = -xSpeed - finalFall;
                zFall += -zSpeed - finalFall;
                world *= Matrix.CreateTranslation(xFall, 0, zFall);
                if (heightmapCollision.sameHeight(ballPosition))
                {
                    xFall = 0;
                    zFall = 0;
                    xSpeed = -xSpeed*1.5f;
                    zSpeed = -zSpeed*1.5f;
                    world *= Matrix.CreateTranslation(0, heightChecker() / 4.2f, 0);
                    verticalityUp = false;
                }
            }

            System.Diagnostics.Debug.WriteLine(xFinal + " " + xFall + " " + zFinal + " " + zFall);
          //  world *= Matrix.CreateTranslation(xFinal-xFall, 0, zFinal-zFall);
            if (ballPosition.Z < -140)
            {
             //   movement *= Matrix.CreateTranslation(0,((previousBallZ - ballPosition.Z)/1.75f),0);
                previousBallZ = ballPosition.Z;
            }

            camera.passBallPosition(ballPosition);
            //Jumping
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && onJump == false)
            {
                onJump = true;
                jumpSpeed = initialJumpSpeed;
            }

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
