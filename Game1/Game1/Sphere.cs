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
    public class Sphere : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public Vector3 cubePosition {get; protected set; }
        
        Matrix worldTranslation = Matrix.Identity;
        Matrix worldRotation = Matrix.Identity;
        Matrix worldScale = Matrix.Identity;
        
        MouseState previousState;

        Texture2D texture;
        VertexPositionNormalTexture[] verts;
        VertexBuffer vertexBuffer;
        BasicEffect effect;
        Camera camera;
        
        //Jump-related variable
        bool onJump = false;
        float jumpSpeed = 0;
        float initialJumpSpeed = .03f;
        Vector3 initialHeight = new Vector3(0, 0, 0);

        //Draw Sphere
        VertexPositionNormalTexture[] vpnt;
        int[] indices;
        float radius = 3;
        int stacks = 9;
        int slices = 6;

        //Texture coordinates 
        Vector2 topLeft, topMiddle, topRight, centerLeft, center, centerRight, bottomLeft, bottomMiddle, bottomRight;

        //Normal vectors
        Vector3 FrontNormal, BackNormal, TopNormal, BottomNormal, LeftNormal, RightNormal;


        public Sphere(Game game, Camera camera)
            : base(game)
        {
            this.camera = camera;
            camera.setCube(this);
        }

        public override void Initialize()
        {
            verts = new VertexPositionNormalTexture[36];

            vpnt = new VertexPositionNormalTexture[(slices + 1) * (stacks + 1)];

            base.Initialize();
        }

        protected override void LoadContent()
        {
            texture = Game.Content.Load<Texture2D>(@"Texture\crate");
            setTextureCoordinates();
            setNormalVectors();
            setCubeVertices();
            //setSphereVertices();
            base.LoadContent();
        }

        public void setTextureCoordinates()
        {
            topLeft = new Vector2(0, 0);     
            topMiddle = new Vector2(.5f, 0);   
            topRight = new Vector2(1, 0);     
            centerLeft = new Vector2(0, .5f);   
            center = new Vector2(.5f, .5f); 
            centerRight = new Vector2(1, .5f);   
            bottomLeft = new Vector2(0, 1);     
            bottomMiddle = new Vector2(.5f, 1);   
            bottomRight = new Vector2(1, 1);     
        }

        public void setNormalVectors()
        {
            FrontNormal = new Vector3(0, 0, 1);     //z axis
            BackNormal = new Vector3(0, 0, -1);     //-z axis
            TopNormal = new Vector3(0, 1, 0);       // y axis
            BottomNormal = new Vector3(0, -1, 0);   // -y axis
            LeftNormal = new Vector3(-1, 0, 0);     // -x axis
            RightNormal = new Vector3(1, 0, 0);     // x axis
        }

        protected void setSphereVertices()
        {

            float phi, theta;
            float dphi = MathHelper.Pi / stacks;
            float dtheta = MathHelper.TwoPi / slices;
            float x, y, z, sc;
            int index = 0;

            for (int stack = 0; stack <= stacks; stack++)
            {
                phi = MathHelper.PiOver2 - stack * dphi;
                y = radius * (float)Math.Sin(phi);
                sc = -radius * (float)Math.Cos(phi);

                for (int slice = 0; slice <= slices; slice++)
                {
                    theta = slice * dtheta;
                    x = sc * (float)Math.Sin(theta);
                    z = sc * (float)Math.Cos(theta);
                    vpnt[index++] = new VertexPositionNormalTexture(new Vector3(x, y, z),
                                                                    new Vector3(x, y, z),
                                                                    new Vector2((float)slice / (float)slices, (float)stack / (float)stacks));
                }
            }

            indices = new int[slices * stacks * 6];
            index = 0;
            int k = slices + 1;

            for (int stack = 0; stack < stacks; stack++)
            {
                for (int slice = 0; slice < slices; slice++)
                {
                    indices[index++] = (stack + 0) * k + slice;
                    indices[index++] = (stack + 1) * k + slice;
                    indices[index++] = (stack + 0) * k + slice + 1;

                    indices[index++] = (stack + 0) * k + slice + 1;
                    indices[index++] = (stack + 1) * k + slice;
                    indices[index++] = (stack + 1) * k + slice + 1;
                }
            }

            effect = new BasicEffect(GraphicsDevice);

            vertexBuffer = new VertexBuffer(
                GraphicsDevice,
                typeof(VertexPositionTexture),
                vpnt.Length,
                BufferUsage.None);

        }

        public void setCubeVertices()
        {
            //Corners of cube
            Vector3 topRightFront = new Vector3(1, 3, 1);
            Vector3 bottomRightFront = new Vector3(1, 1, 1);
            Vector3 topLeftFront = new Vector3(-1, 3, 1);
            Vector3 bottomLeftFront = new Vector3(-1, 1, 1);
            Vector3 topRightBack = new Vector3(1, 3, -1);
            Vector3 bottomRightBack = new Vector3(1, 1, -1);
            Vector3 topLeftBack = new Vector3(-1, 3, -1);
            Vector3 bottomLeftBack = new Vector3(-1, 1, -1);

            //Front face
            verts[0] = new VertexPositionNormalTexture(topLeftFront, FrontNormal, topLeft);
            verts[1] = new VertexPositionNormalTexture(topRightFront, FrontNormal, topMiddle);
            verts[2] = new VertexPositionNormalTexture(bottomLeftFront, FrontNormal, centerLeft);
            verts[3] = new VertexPositionNormalTexture(bottomLeftFront, FrontNormal, centerLeft);
            verts[4] = new VertexPositionNormalTexture(topRightFront, FrontNormal, topMiddle);
            verts[5] = new VertexPositionNormalTexture(bottomRightFront, FrontNormal, center);

            //Back face
            verts[6] = new VertexPositionNormalTexture(topRightBack, BackNormal, topMiddle);
            verts[7] = new VertexPositionNormalTexture(topLeftBack, BackNormal, topRight);
            verts[8] = new VertexPositionNormalTexture(bottomRightBack, BackNormal, center);
            verts[9] = new VertexPositionNormalTexture(bottomRightBack, BackNormal, center);
            verts[10] = new VertexPositionNormalTexture(topLeftBack, BackNormal, topRight);
            verts[11] = new VertexPositionNormalTexture(bottomLeftBack, BackNormal, centerRight);

            //Top face
            verts[12] = new VertexPositionNormalTexture(topLeftBack, TopNormal, centerLeft);
            verts[13] = new VertexPositionNormalTexture(topRightBack, TopNormal, center);
            verts[14] = new VertexPositionNormalTexture(topLeftFront, TopNormal, bottomLeft);
            verts[15] = new VertexPositionNormalTexture(topLeftFront, TopNormal, bottomLeft);
            verts[16] = new VertexPositionNormalTexture(topRightBack, TopNormal, center);
            verts[17] = new VertexPositionNormalTexture(topRightFront, TopNormal, bottomMiddle);

            //Bottom face
            verts[18] = new VertexPositionNormalTexture(bottomLeftFront, BottomNormal, center);
            verts[19] = new VertexPositionNormalTexture(bottomRightFront, BottomNormal, centerRight);
            verts[20] = new VertexPositionNormalTexture(bottomLeftBack, BottomNormal, bottomMiddle);
            verts[21] = new VertexPositionNormalTexture(bottomLeftBack, BottomNormal, bottomMiddle);
            verts[22] = new VertexPositionNormalTexture(bottomRightFront, BottomNormal, centerRight);
            verts[23] = new VertexPositionNormalTexture(bottomRightBack, BottomNormal, bottomRight);

            //Left face
            verts[24] = new VertexPositionNormalTexture(topLeftBack, LeftNormal, topMiddle);
            verts[25] = new VertexPositionNormalTexture(topLeftFront, LeftNormal, topRight);
            verts[26] = new VertexPositionNormalTexture(bottomLeftBack, LeftNormal, center);
            verts[27] = new VertexPositionNormalTexture(bottomLeftBack, LeftNormal, center);
            verts[28] = new VertexPositionNormalTexture(topLeftFront, LeftNormal, topRight);
            verts[29] = new VertexPositionNormalTexture(bottomLeftFront, LeftNormal, centerRight);

            //Right face
            verts[30] = new VertexPositionNormalTexture(topRightFront, RightNormal, topLeft);
            verts[31] = new VertexPositionNormalTexture(topRightBack, RightNormal, topMiddle);
            verts[32] = new VertexPositionNormalTexture(bottomRightFront, RightNormal, centerLeft);
            verts[33] = new VertexPositionNormalTexture(bottomRightFront, RightNormal, centerLeft);
            verts[34] = new VertexPositionNormalTexture(topRightBack, RightNormal, topMiddle);
            verts[35] = new VertexPositionNormalTexture(bottomRightBack, RightNormal, center);

            effect = new BasicEffect(GraphicsDevice);

            vertexBuffer = new VertexBuffer(
                GraphicsDevice,
                typeof(VertexPositionTexture),
                verts.Length,
                BufferUsage.None);
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();

            cubePosition = worldTranslation.Translation;
            /*
            //Rotation
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                worldRotation *= Matrix.CreateRotationX((MathHelper.PiOver4 / 60) * (mouseState.Y - previousState.Y));
                worldRotation *= Matrix.CreateRotationY((MathHelper.PiOver4 / 60) * (mouseState.X - previousState.X));
            }
            
            //Scaling
            if (mouseState.ScrollWheelValue > previousState.ScrollWheelValue)
                worldScale *= Matrix.CreateScale(1.1f);
            if (mouseState.ScrollWheelValue < previousState.ScrollWheelValue)
                worldScale *= Matrix.CreateScale(.9f);
            */
            //Translation
            if (keyboardState.IsKeyDown(Keys.W))
                worldTranslation *= Matrix.CreateTranslation(0, 0, -.1f);
            if (keyboardState.IsKeyDown(Keys.S))
                worldTranslation *= Matrix.CreateTranslation(0, 0, .1f);
            if (keyboardState.IsKeyDown(Keys.A))
                worldTranslation *= Matrix.CreateTranslation(-.1f, 0, 0);
            if (keyboardState.IsKeyDown(Keys.D))
                worldTranslation *= Matrix.CreateTranslation(.1f, 0, 0);

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && onJump == false)
            {
                onJump = true;
                jumpSpeed = initialJumpSpeed;
            }

            if (onJump)
            {
                worldTranslation *= Matrix.CreateTranslation(Vector3.Up * jumpSpeed * gameTime.ElapsedGameTime.Milliseconds);
                jumpSpeed -= .001f;
                if (cubePosition.Y < 1)
                {
                    worldTranslation *= Matrix.CreateTranslation(initialHeight);
                    onJump = false;
                    jumpSpeed = initialJumpSpeed;
                }
            }

            previousState = Mouse.GetState();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetVertexBuffer(vertexBuffer);

            effect.World = worldRotation * worldTranslation * worldScale;
            effect.View = camera.view;
            effect.Projection = camera.projection;

            effect.Texture = texture;
            effect.TextureEnabled = true;
            effect.EnableDefaultLighting();

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, verts, 0, 12);
            }

            base.Draw(gameTime);
        }

        public bool isJumping()
        {
            return onJump;
        }
    }
}
