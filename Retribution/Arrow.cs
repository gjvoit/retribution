using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Retribution
{

    class Arrow : Projectile
    {

        public Arrow(Vector2 position, int damage, GameObject target, int health = 1, int attackRange = 0)
            : base(position, damage, target, health, attackRange)
        {
            this.position = position;
            this.moveSpeed = 3;
            attackWait = 300;
        }

        public override void LoadContent(ContentManager content)
        {
            this.texture = content.Load<Texture2D>("arrow.png");
        }
    }
}
