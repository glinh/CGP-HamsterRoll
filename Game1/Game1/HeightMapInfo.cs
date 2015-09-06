using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    public class HeightMapInfo
    {
        private int heightmapWidth;
        private int heightmapHeight;
        private Texture2D heightMap;
        float[,] heightMapData;

        public HeightMapInfo()
        {
        }

        public void setHeight(Texture2D heightMap)
        {
            this.heightMap = heightMap;
            heightmapWidth = heightMap.Width;
            heightmapHeight = heightMap.Height;
            Color[] greyValues = new Color[heightmapWidth * heightmapHeight];
            heightMap.GetData<Color>(greyValues);
            heightMapData = new float[heightmapWidth, heightmapHeight];
            for (int x = 0; x < heightmapWidth; x++)
            {
                for (int y = 0; y < heightmapHeight; y++)
                {
                    heightMapData[x, y] = (255-greyValues[x + y * heightmapWidth].R)/4.2f;
                }
            }
        }

        public bool IsOnHeightmap(Vector3 position)
        {
            Vector3 positionOnHeightmap = position;
            return (positionOnHeightmap.X > -heightmapWidth/2 &&
                positionOnHeightmap.X < heightmapWidth/2 &&
                positionOnHeightmap.Z > -heightmapHeight/2 &&
                positionOnHeightmap.Z < heightmapHeight/2);
        }

        public float GetHeight(Vector3 position)
        {
            return heightMapData[heightmapHeight/2-(int)position.X,heightmapWidth/2-(int)position.Z];
        }
    }
}
