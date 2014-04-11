using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;

namespace Retribution
{
    
    class Tower : GameObject
    {
        public string state;
        public Arrow myArrow;
        SoundEffect soundEffect;
       
        public Tower(Vector2 position, int health = 5, int damage = 1, int attackRange = 250)
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
            texture = content.Load<Texture2D>("tower.png");
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
        public override void attackSound(ContentManager content)
        {
            SoundEffect soundEffect = content.Load<SoundEffect>("bow.wav");
            soundEffect.Play();
        }
        public override void Attack(GameObject target, ContentManager content, ProjectileManager projMan)
        {
            attackSound(content);
            Vector2 corrected = Vector2.Add(position, new Vector2(16, 16));
            Projectile projectile = new Arrow(corrected, 100, target, 100, 0);
            Vector2 direction = MovementManager.getNormalizedVector(projectile.position, target.position);
            projectile.setDestination(direction, target.position);
            projectile.LoadContent(content);
            projMan.proj.Add(projectile);
        }

       /* public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)this.position.X,(int)this.position.Y, 50, 50), Color.White);
        }*/

    }
}
