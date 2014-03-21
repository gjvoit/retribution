#region Using Statements
using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace Retribution
{

    public class Map
    {
        public Tile[,] mapTiles;
        public int height;
        public int width;
        // public int xoffset = 0; For use with FullScreen
        // public int yoffset = 0; For use with FullScreen
        public Map(String fileName)
        {
            string[] tiles = File.ReadAllLines(fileName);
            width = tiles[0].Length;
            height = tiles.Length;
            mapTiles = new Tile[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    mapTiles[x, y] = new Tile(tiles[y][x]);
                }
            }
        }

        public void DrawMap(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Texture2D myTexture = mapTiles[x, y].texture;
                    spriteBatch.Begin();
                    spriteBatch.Draw(myTexture, new Vector2(x * myTexture.Width, y * myTexture.Height), Color.White);
                    spriteBatch.End();
                }
            }
        }


    }

}
