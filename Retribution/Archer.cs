﻿using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Retribution
{

    class Archer : Mobile
    {

        public Archer(Vector2 position, int health = 2, int damage = 3, int attackRange = 150)
            : base(health, position, damage, attackRange)
        {
            this.type = "ARCHER";
            this.moveSpeed = 1;

        }
        public Arrow makeArrow(GameObject target)
        {
            //ProjectileFactory.create("arrow", target);
            Arrow arrow = new Arrow(this.position, target);
            return arrow;
        }

        public override void LoadContent(ContentManager content)
        {
            this.texture = content.Load<Texture2D>("archer.png");
        }

    }
}