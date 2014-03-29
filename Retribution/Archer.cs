using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Retribution
{

    //class Archer : GameObject
    class Archer : Mobile
    {

        public Archer(Vector2 position, int health = 2, int damage = 3, int attackRange = 2)
            : base(health, position, damage, attackRange)
        {
            this.moveSpeed = 1;
            //this.canMove = true;
        }
        public void makeArrow(GameObject target)
        {
            //ProjectileFactory.create("arrow", target);
        }

        public void LoadContent(ContentManager content)
        {
            this.texture = content.Load<Texture2D>("archer.png");
        }

        public void Update(GameTime gameTime)
        {
            if (this.isAlive())
            {
                // TODO: put code here for when archer is alive
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)this.position.X, (int)this.position.Y, 50, 50), Color.White);
        }

    }
}
