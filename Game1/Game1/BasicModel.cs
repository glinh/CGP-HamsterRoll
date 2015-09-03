using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class BasicModel
    {
        Camera camera;

        public Model model { get; protected set; }

        protected Matrix world = Matrix.Identity;
        protected Matrix translation = Matrix.Identity;
        protected Matrix rotation = Matrix.Identity;
        protected Matrix movement = Matrix.Identity;

        public BasicModel(Model model, Camera camera)
        {
            this.model = model;
            this.camera = camera;
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(GraphicsDevice device, Camera camera)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = mesh.ParentBone.Transform * rotation * world * movement  * translation; // * scaling matrix
                    effect.View = camera.view;
                    effect.Projection = camera.projection;
                    effect.TextureEnabled = true;
                }
                mesh.Draw();
            }
        }
        /*
        public bool CollidesWith(Model otherModel, Matrix otherWorld)
        {
            // Loop through each ModelMesh in both objects and compare
            // all bounding spheres for collisions
            foreach (ModelMesh myModelMeshes in model.Meshes)
            {
                foreach (ModelMesh hisModelMeshes in otherModel.Meshes)
                {
                    if (myModelMeshes.BoundingSphere.Transform(
                        GetWorld()).Intersects(
                        hisModelMeshes.BoundingSphere.Transform(otherWorld)))
                        return true;
                }
            }
            return false;
        }
         */

        public virtual Matrix GetWorld ()
        {
            return world;
        }

        public virtual Matrix GetTranslation()
        {
            return translation;
        }

        protected virtual Matrix GetRotation()
        {
            return rotation;
        }

    }
}
