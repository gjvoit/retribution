using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Retribution
{
    class Warrior : Mobile
    {

        public Warrior(Vector2 position, int health = 4, int damage = 1, int attackRange = 1)
            : base(health, position, damage, attackRange)
        {
            this.moveSpeed = 2;
        }

        public override void LoadContent(ContentManager content)
        {
            this.texture = content.Load<Texture2D>("warrior.png");
        }

    }
}
