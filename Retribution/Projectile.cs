using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Retribution
{
    public abstract class Projectile : GameObject
    {
        public Vector2 direction;
        public Vector2 destination;
        public int moveSpeed;
        public bool isMoving;
        public GameObject target;
        public bool collided;

        public Projectile(Vector2 position, int damage, GameObject target, int health = 1, int attackRange = 0)
            : base(health, position, damage, attackRange)
        {
            this.target = target;
            this.isMoving = true;
            this.attackWait = 60;
            collided = false;
        }

        public void setDestination(Vector2 direction, Vector2 destination)
        {
            this.direction = direction;
            this.destination = destination;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 pixelposWorld = Vector2.Normalize(destination - position);
            float dot = Vector2.Dot(Vector2.UnitY, pixelposWorld);
            float theta = (float)Math.Atan2(Vector2.UnitY.Y - destination.Y, Vector2.UnitY.X - destination.X);
            float rotation = (pixelposWorld.X <= 0)
                 ? (1f - dot) / 4f
                 : (dot + 3f) / 4f;
            if (pixelposWorld.Y <= 0)
                theta += 3.14159F;
            //if (pixelposWorld.X < 0)
            //    theta += (1f - dot) / 4f;
            //else
            //    theta -= (dot + 3f) / 4f;
            //if (position.X < destination.X)
            //    rotation -= 1.7F;
            //float rotate =(float)Math.Atan2(position.Y - destination.Y, position.X - destination.X);
            spriteBatch.Draw(texture, position, null, Color.White, theta, new Vector2(0, 0), 1, SpriteEffects.None, 0);
           // spriteBatch.Draw(texture, new Rectangle((int)this.position.X, (int)this.position.Y, 32, 32), Color.White);
        }
        public void move()
        {
            

            Vector2 end_point = Vector2.Add(this.destination, new Vector2(2, 2));
            Vector2 prev_point = Vector2.Subtract(this.destination, new Vector2(2, 2));
            this.attackWait--;
            if (this.isAlive())
            {


                if (this.position.X <= end_point.X && this.destination.X >= prev_point.X
                    && this.position.Y <= end_point.Y && this.destination.Y >= prev_point.Y) //equivalent to IsInRange
                {
                    this.health = -1;
                    collided = true;
                    if (this.target.isAlive())
                        this.target.health -= this.damage;
                    return;
                }
                
            }

            this.position += direction * moveSpeed;
        }
    }
}
