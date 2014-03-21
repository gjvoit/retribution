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

        public Tile(char typeChar)
        {

            graphics = new GraphicsDevice();
            if (typeChar == '0')
            {
                type = "grass";
                texture = Texture2DFromFile(graphics, "Content/grass.jpg");
            }

            else if (typeChar == '1')
            {
                type = "water";
                texture = Texture2DFromFile(graphics, "Content/water.jpg");
            }

            else if (typeChar == '2')
            {
                type = "bridge";
                texture = Texture2DFromFile(graphics, "Content/bridge.png");
            }

            else if (typeChar == '3')
            {
                type = "rock";
                texture = Texture2DFromFile(graphics, "Content/rock.jpg");
            }
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
    }

}