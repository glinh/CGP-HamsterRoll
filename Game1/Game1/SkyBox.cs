using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1
{
    class SkyBox : BasicModel
    {
        Camera camera;

        public SkyBox(Model model, Camera camera)
            : base (model, camera)
        {
            this.camera = camera;
        }

        public override void Update(GameTime gameTime)
        {
            Vector3 v = camera.cameraPosition;
            v.Y = 0;
            world = Matrix.CreateScale(1000) * Matrix.CreateTranslation(v);
            base.Update(gameTime);
        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {
            // Here is code for sky
            SamplerState ss = new SamplerState();
            ss.AddressU = TextureAddressMode.Clamp;
            ss.AddressV = TextureAddressMode.Clamp;
            device.SamplerStates[0] = ss;

            base.Draw(device, camera);
        }
    }
}
