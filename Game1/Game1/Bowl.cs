using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class Bowl : BasicModel
    {
        public Bowl(Model model, Camera camera)
            : base (model, camera)
        {

        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {            
            translation = Matrix.CreateTranslation(-38,-190, -80);
            world = Matrix.CreateScale(2f);

            base.Draw(device, camera);
        }
        
        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

    }
}
