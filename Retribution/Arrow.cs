using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Retribution
{

    class Arrow : Projectile
    {

        public Arrow(Vector2 position, GameObject target, int damage = 3)
            : base(position, damage, ref target)
        {
            this.position = position;
            this.moveSpeed = 1;
        }

        public override void LoadContent(ContentManager content)
        {
            this.texture = content.Load<Texture2D>("arrow.png");
        }
    }
}
