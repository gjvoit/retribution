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
            float rotation = (float)Math.Acos(Vector2.Dot(new Vector2(0f, -1f), this.direction));
            rotation *= (Vector2.Dot(new Vector2(0f, -1f), new Vector2(this.direction.Y, -this.direction.X)) > 0f) ? 1f : -1f;
            spriteBatch.Draw(texture, position, null, Color.White, rotation, new Vector2(0, 0), 1, SpriteEffects.None, 0);
           // spriteBatch.Draw(texture, new Rectangle((int)this.position.X, (int)this.position.Y, 32, 32), Color.White);
        }
        public void move()
        {
            

            Vector2 end_point = Vector2.Add(this.destination, new Vector2(2, 2));
            Vector2 prev_point = Vector2.Subtract(this.destination, new Vector2(2, 2));
            this.attackWait--;
            if (this.isAlive())
            {


                if (this.position.X <= end_point.X && this.position.X >= prev_point.X
                    && this.position.Y <= end_point.Y && this.position.Y >= prev_point.Y) //equivalent to IsInRange
                {
                    this.health = -1;
                    collided = true;
                    if (String.Compare(this.type, "ICEBALL", true) == 0 && this.target.isAlive())
                    {
                        if(this.target.attackRange>5)
                        this.target.attackRange -= 5;
                        this.target.attackSpeed += 100;
                    }
                    else
                        if (this.target.isAlive())
                            this.target.health -= this.damage;
                    return;
                }
                
            }

            this.position += direction * moveSpeed;
        }
    }
}
