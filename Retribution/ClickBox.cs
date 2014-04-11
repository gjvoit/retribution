using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Retribution
{
    //  A tanky, high healthed, unit. Has armor that takes extra damage from magic
    class ClickBox:GameObject
    {

        public ClickBox(Vector2 position, int health = 0, int damage = 0, int attackRange = 0)
            : base(health, position, damage, attackRange)
        {
           
        }
        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("clickbox.png");
        }
        

    }
}