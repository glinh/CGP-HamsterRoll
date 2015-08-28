using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Game1
{
    class ModelManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Game game;
        Camera camera;
        List<BasicModel> modelCollection = new List<BasicModel>();

        public ModelManager (Game game, Camera camera)
            : base (game)
        {
            this.game = game;
            this.camera = camera;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {            
            modelCollection.Add(new Bowl(
               Game.Content.Load<Model>(@"Model/Bowl/bowl"), camera));
            modelCollection.Add(new Ground(
                Game.Content.Load<Model>(@"Model/Ground/Ground"),camera)); // ground
            modelCollection.Add(new SkyBox(
                Game.Content.Load<Model>(@"Model/SkyBox/skybox"),camera)); // skybox            
            modelCollection.Add(new Ball(
                Game.Content.Load<Model>(@"Model/Ball/BeachBall"), camera));

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (BasicModel model in modelCollection)
            {
                model.Update(gameTime);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (BasicModel model in modelCollection)
            {
                model.Draw(((Game1)Game).GraphicsDevice, ((Game1)Game).camera);
            }

            base.Draw(gameTime);
        }
    }
}
