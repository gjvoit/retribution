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
        public Sprite builderSprite;
        public Boolean selected;

        public Builder(Sprite newBuilder, ContentManager content)
        {
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

        public void Move(MouseState mouse)
        {
            this.builderSprite.spriteX = mouse.X;
            this.builderSprite.spriteY = mouse.Y;
        }

    }
}
