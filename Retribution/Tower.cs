using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Retribution
{
    
    class Tower : GameObject
    {
        public string state;
        public Arrow myArrow;
        public Tower(Vector2 position, int health = 100, int damage = 1, int attackRange = 250)
            : base (health, position, damage, attackRange)
        {
            this.position = position;
            this.state = "Wall";
            this.type = "TOWER";
            this.attackSpeed = 300;
        }

        // change the state of the Tower
        public void Morph(string mobiletype = "Wall")
        {
            
        }
        /*
        public override void Die()
        {
            this.alive = false;
        }
         */

        public override void LoadContent(ContentManager content)
        {
            this.texture = content.Load<Texture2D>("tower.png");
        }

        public void Update(GameTime gameTime)
        {
            if (this.alive)
            {
                // TODO: put code here to update tower when alive

                if (this.health <= 0)
                {
                    //TODO: put code here to handle death
                }
            }
        }
        public override void Attack(GameObject target)
        {
            myArrow = makeArrow(target);
            Vector2 direction = MovementManager.getNormalizedVector(this.myArrow.position, target.position);
            myArrow.setDestination(direction, target.position);
        }
        public Arrow makeArrow(GameObject target)
        {
            Vector2 corrected = Vector2.Add(position, new Vector2(16, 16));
            return new Arrow(corrected, ref target);

        }

       /* public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)this.position.X,(int)this.position.Y, 50, 50), Color.White);
        }*/

    }
}
