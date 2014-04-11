using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace Retribution
{

    class Builder
    {
        new public static Texture2D texture;
        public int offset = 20;
        public Sprite builderSprite;
        public Boolean selected;
        public ContentManager content;

        public Builder(Sprite newBuilder, ContentManager content)
        {
            this.content = content;
            builderSprite = newBuilder;
            selected = false;
            builderSprite.LoadContent(content, "human2.jpg");
        }
        public Boolean IsSelectable(MouseState mouse)
        {
            if ((mouse.X >= this.builderSprite.spriteX && mouse.X <= this.builderSprite.spriteX + 32)
                && (mouse.Y >= this.builderSprite.spriteY && mouse.Y <= this.builderSprite.spriteY + 32))
                return true;
            else return false;
        }

        public Tower Build(MouseState mouse)
        {
            Vector2 adjacent = new Vector2(this.builderSprite.spriteX, this.builderSprite.spriteY);
            if (adjacent.X + 32 >= mouse.X
                || adjacent.X - 32 <= mouse.X
                || adjacent.Y + 32 >= mouse.Y
                || adjacent.Y - 32 <= mouse.Y)
            {
                Vector2 towerbase = new Vector2(mouse.X, mouse.Y);
                Tower newtower = new Tower(towerbase);
                //newtower.LoadContent(Content);
                return newtower;
            }
            offset += offset;
            return new Tower(new Vector2(600, 0+offset));
        }

        public void Move(MouseState mouse)
        {
            this.builderSprite.spriteX = mouse.X;
            this.builderSprite.spriteY = mouse.Y;
        }

    }
}
