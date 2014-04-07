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

    public class Tile
    {
        public string type;
        public Texture2D texture;
        GraphicsDevice graphics;
        public Vector2 origin;
        public int height;
        public int width;
        public Boolean isWalkable;
        public Tile parentTile;
        public int xPosition;
        public int yPosition;
        public int gScore;
        public int hScore;
        public int fScore;

        //  A rectangle to represent the object
        public Rectangle Bounds
        {
            get { return new Rectangle((int)origin.X, (int)origin.Y, this.width, this.height); }
        }

        public Tile(char typeChar)
        {
            gScore = 0;
            hScore = 0;
            fScore = 0;
            parentTile = null;
            graphics = new GraphicsDevice();
            if (typeChar == '0')
            {
                type = "grass";
                texture = Texture2DFromFile(graphics, "Content/grass.png");
                isWalkable = true;
            }

            else if (typeChar == '1')
            {
                type = "water";
                texture = Texture2DFromFile(graphics, "Content/water.png");
                isWalkable = false;
            }

            else if (typeChar == '2')
            {
                type = "bridge";
                texture = Texture2DFromFile(graphics, "Content/bridge.png");
                isWalkable = true;
            }

            else if (typeChar == '3')
            {
                type = "rock";
                texture = Texture2DFromFile(graphics, "Content/rock.jpg");
                isWalkable = false;
            }
            else if (typeChar == '4')
            {
                type = "select";
                texture = Texture2DFromFile(graphics, "Content/grass2.png");
                isWalkable = true;
            }
            height = texture.Height;
            width = texture.Width;
        }

        public Texture2D Texture2DFromFile(GraphicsDevice myGraphics, string fileName)
	    {
	    Texture2D newTexture;
	 
	    FileStream stream = new FileStream(fileName, FileMode.Open);
	    newTexture = Texture2D.FromStream(myGraphics, stream);
        //newTexture.
	 
	    stream.Close();
	    return newTexture;
	    }

        public void SetOrigin(Vector2 newOrigin)
        {
            origin = newOrigin;
        }

    }

}