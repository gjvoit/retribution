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

        public Tower(Vector2 position, int health = 2, int damage = 100, int attackRange = 200)
            : base (health, position, damage, attackRange)
        {
            this.position = position;
            this.state = "Wall";
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

        public void LoadContent(ContentManager content)
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

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)this.position.X,(int)this.position.Y, 50, 50), Color.White);
        }
    }
}
