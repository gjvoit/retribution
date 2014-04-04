using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Retribution
{
    
    class Archer : Mobile
    {
        public Arrow myArrow;

        public Archer(Vector2 position, int health = 20, int damage = 3, int attackRange = 150)
            : base(health, position, damage, attackRange)
        {
            this.type = "ARCHER";
            this.moveSpeed = 1;
            this.attackWait = 0;
            attackSpeed = 280;

        }
       
        public override void Attack(GameObject target)
        {
            myArrow = makeArrow(target);
            Vector2 direction = MovementManager.getNormalizedVector(this.myArrow.position, target.position);
            myArrow.setDestination(direction, target.position);
        }
        public Arrow makeArrow(GameObject target)
        {
            
            return new Arrow(this.position, ref target);
            
        }

        public override void LoadContent(ContentManager content)
        {
            this.texture = content.Load<Texture2D>("archer.png");
        }

    }
}
