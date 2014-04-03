using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Retribution
{

    class Arrow : Projectile
    {

        public Arrow(Vector2 position, int damage = 3)
            : base(position, damage)
        {
            this.position = position;
            this.moveSpeed = 4;
        }

        public override void LoadContent(ContentManager content)
        {
            this.texture = content.Load<Texture2D>("arrow.png");
        }
    }
}
