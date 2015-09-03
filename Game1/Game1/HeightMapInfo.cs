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
        private float terrainScale=1;
        private Vector3 heightmapPosition;
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
            Vector3 positionOnHeightmap = position - heightmapPosition;
            return (positionOnHeightmap.X > -heightmapWidth/2 &&
                positionOnHeightmap.X < heightmapWidth/2 &&
                positionOnHeightmap.Z > -heightmapHeight/2 &&
                positionOnHeightmap.Z < heightmapHeight/2);
        }

        public float GetHeight(Vector3 position)
        {
            /*
            Vector3 positionOnHeightmap = position - heightmapPosition;

            int left, top;
            left = (int)positionOnHeightmap.X / (int)terrainScale;
            top = (int)positionOnHeightmap.Z / (int)terrainScale;

            float xNormalized = (positionOnHeightmap.X % terrainScale) / terrainScale;
            float zNormalized = (positionOnHeightmap.Z % terrainScale) / terrainScale;

            float topHeight = MathHelper.Lerp(
                heightMapData[left, top],
                heightMapData[left + 1, top],
                xNormalized);

            float bottomHeight = MathHelper.Lerp(
                heightMapData[left, top + 1],
                heightMapData[left + 1, top + 1],
                xNormalized);

            return MathHelper.Lerp(topHeight, bottomHeight, zNormalized);
             */
            return heightMapData[heightmapHeight/2-(int)position.X,heightmapWidth/2-(int)position.Z];
        }
    }
    /*
    public class HeightMapInfoReader : ContentTypeReader<HeightMapInfo>
    {
        protected override HeightMapInfo Read(ContentReader input,
            HeightMapInfo existingInstance)
        {
            float terrainScale = input.ReadSingle();
            int width = input.ReadInt32();
            int height = input.ReadInt32();
            float[,] heights = new float[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    heights[x, z] = input.ReadSingle();
                }
            }
            return new HeightMapInfo(heights, terrainScale);
        }
    }
     */
}
