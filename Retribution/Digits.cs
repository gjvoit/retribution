using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Retribution
{
    class Digits : GameObject
    {
        public string state;
        public int theDigit = 0;

        public Digits(Vector2 position, int health = 2, int damage = 0, int attackRange = 150)
            : base(health, position, damage, attackRange)
        {
            this.position = position;
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("numbers_spritesheet.png");
        }
    }
}
