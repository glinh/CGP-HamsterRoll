using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class HeightmapCollision : Microsoft.Xna.Framework.Game
    {
        HeightMapInfo heightMapInfo;

        public HeightmapCollision(HeightMapInfo heightMapInfo)
        {
            this.heightMapInfo = heightMapInfo;
        }


        public Boolean onMap(Vector3 position)
        {
            if (heightMapInfo.IsOnHeightmap(position))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public Boolean sameHeight(Vector3 position)
        {
            if (heightMapInfo.GetHeight(position) < position.Y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public float heightCollision(Vector3 position)
        {
            if (heightMapInfo.IsOnHeightmap(position))
            {
                System.Diagnostics.Debug.WriteLine("true");
                return (heightMapInfo.GetHeight(position) - position.Y);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("false");
                return 0;
            }
        }
    }
}
