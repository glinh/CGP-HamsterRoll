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
        float speed = 0.035f;
        const float friction = 0.0175f;
        HeightmapCollision heightmapCollision;

        //Jump-related variable
        bool onJump = false;
        bool verticalityUp = false;
        float jumpSpeed = 0;
        float fallSpeed = 0.03f;
        float initialJumpSpeed = .03f;
        Vector3 initialHeight = new Vector3(0, 0, 0);

        //Fall Down mechanic
        float xRecord = 0;
        float zRecord = 0;
        float xFinal = 0;
        float zFinal = 0;
        float xFall = 0;
        float zFall = 0;
        float xFallBack = 0;
        float zFallBack = 0;
        float finalFall = 0;
        float xGravity = 0;
        float zGravity = 0;
        bool removeSpeed = false;
        float slopeCurve = 2.5f;
        bool movementDeactivated = false;
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

        /*
         * Is used to check the current position of the ball and
         * returns a speed so that the ball will always curve back into the 
         * bowl.
         * The divider is to take into account the decrement in speed for the upwards
         * curvature.
        */ 
        public float heightChecker()
        {
            if (ballPosition.X == 0)
            {
                if (ballPosition.Z > 0)
                {
                    return zSpeed/slopeCurve;
                }
                else
                {
                    return -zSpeed/slopeCurve;
                }
            }

            if (ballPosition.Z == 0)
            {
                if (ballPosition.X > 0)
                {
                    return xSpeed/slopeCurve;
                }
                else
                {
                    return -xSpeed/slopeCurve;
                }
            }

            if (ballPosition.X > 0 && ballPosition.Z > 0)
            {
                return (xSpeed + zSpeed)/slopeCurve;
            }

            if (ballPosition.X < 0 && ballPosition.Z < 0)
            {
                return (-xSpeed- zSpeed)/slopeCurve;
            }

            if (ballPosition.X > 0 && ballPosition.Z < 0)
            {
                return (xSpeed - zSpeed)/slopeCurve;
            }

            if (ballPosition.X < 0 && ballPosition.Z > 0)
            {
                return (-xSpeed + zSpeed)/slopeCurve;
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

            //Translation
            //Stopping the object completely after friction
            if (Math.Abs(xSpeed) < 0.0175f)
            {
                xSpeed = 0;
            }
            if (Math.Abs(zSpeed) < 0.0175f)
            {
                zSpeed = 0;
            }

            //Movement movement
            if (movementDeactivated == false)
            {
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

            //Setting up values to be used for the slope moving and upwards movement.
            if (ballPosition.Y == 0)
            {
                xRecord = 0;
                zRecord = 0;
                xFallBack = xSpeed;
                zFallBack = zSpeed;

            }
            //Resetting the gravity so that the ball can roll back to the y=0 ground
            if (ballPosition.Y < 0.5f)
            {
                xGravity = 0;
                zGravity = 0;
            }

            //Resets the speed to zero once the above gravity is reset.
            //I've implemented it so that the ball will just stop at y=0 instead of rolling 
            //further towards the center
            if (xGravity == 0 && zGravity == 0 && removeSpeed == true)
            {
                removeSpeed = false;
                xSpeed = 0;
                zSpeed = 0;
            }

            //This is primarily for when the ball is on the ground and while it is travelling up the slope
            // before it has jumped over the top.
            if (ballPosition.Y<55 && verticalityUp == false)
            {
                xFinal = xSpeed;
                zFinal = zSpeed;
                //The regular movement at the regular speed when the ball is rolling on the ground
                if (ballPosition.Y == 0)
                {
                    world *= Matrix.CreateTranslation(xSpeed, 0, zSpeed);
                }
                else
                {
                    //The implementation of gravity on the slope which is dependent on the current ball position.
                    if (ballPosition.X > 0)
                    {
                        xGravity -= 0.01f;
                    }
                    if (ballPosition.X < 0)
                    {
                        xGravity += 0.01f;
                    }

                    if (ballPosition.Z > 0)
                    {
                        zGravity -= 0.01f;
                    }
                    if (ballPosition.Z < 0)
                    {
                        zGravity += 0.01f;
                    }
                    
                    //If the gravity is bigger than the speed, the ball will not be 
                    //able to reach the top and be forced to roll down.
                    //I implemented this to prevent the ball from being able to jump off the top
                    //at all times.
                    if ((Math.Abs(xSpeed) + Math.Abs(zSpeed)) / 2.5f < Math.Abs(xGravity) + Math.Abs(zGravity))
                    {
                        if (xSpeed > 0)
                        {
                            xSpeed -= 0.01f;
                        }
                        if (xSpeed < 0)
                        {
                            xSpeed += 0.01f;
                        }
                        if (zSpeed > 0)
                        {
                            zSpeed -= 0.01f;
                        }
                        if (zSpeed < 0)
                        {
                            zSpeed += 0.01f;                   
                        }                            
                        removeSpeed = true;
                    }
                    //This is for the slower movement up the slope with the gravity implemented in it as well.
                    world *= Matrix.CreateTranslation((xSpeed / slopeCurve) + xGravity, 0, (zSpeed / slopeCurve) + zGravity);

                }
                //Moves the ball on the y-plane according to the height map.
                world *= Matrix.CreateTranslation(0, heightmapCollision.heightCollision(ballPosition),0);
                fallSpeed = initialJumpSpeed;
            }
            else
            {
                //The jumping off the top of the arena.
                movementDeactivated = true;
                verticalityUp = true;
                world *= Matrix.CreateTranslation(0, heightChecker()/5, 0);
                world *= Matrix.CreateTranslation(Vector3.Up * fallSpeed *
          gameTime.ElapsedGameTime.Milliseconds);
               fallSpeed -= 0.0010f;
               finalFall = fallSpeed;

            }
            System.Diagnostics.Debug.WriteLine(ballPosition.ToString());

            //For the down slope after the jump.
            if (ballPosition.Y< 55 && verticalityUp == true)
            {
                movementDeactivated = false;
                if (xSpeed > 0)
                {
                    xFall += -xFinal - finalFall;
                }
                else if (xSpeed < 0)
                {
                    xFall += -xFinal + finalFall;
                }

                if (zSpeed > 0)
                {
                    zFall += -zFinal - finalFall;
                }
                else if (zSpeed < 0)
                {
                    zFall += -zFinal + finalFall;
                }
                world *= Matrix.CreateTranslation(xFall/10, 0, zFall/10);
                if (heightmapCollision.sameHeight(ballPosition))
                {
                    xFall = 0;
                    zFall = 0;
                    xSpeed = -xFinal*0.75f;
                    zSpeed = -zFinal*0.75f;
                    world *= Matrix.CreateTranslation(0, heightChecker() /5, 0);
                    verticalityUp = false;
                }               

            }

            System.Diagnostics.Debug.WriteLine(xSpeed + " " + xRecord + " " + zSpeed + " " + zRecord + " " + finalFall);

            //The ball position is passed to the camera which is transformed to follow the ball.

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
                if (ballPosition.Y < 0.5f)
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
