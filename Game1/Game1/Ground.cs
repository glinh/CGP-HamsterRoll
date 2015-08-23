using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class Ground : BasicModel
    {
        public Ground(Model model, Camera camera)
            : base (model, camera)
        {

        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {
            SamplerState ss = new SamplerState();
            ss.AddressU = TextureAddressMode.Wrap;
            ss.AddressV = TextureAddressMode.Wrap;
            device.SamplerStates[0] = ss;
            base.Draw(device, camera);
        }
    }

}
