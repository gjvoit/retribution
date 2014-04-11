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
        public Vector2 end_point;
        public Vector2 prev_point;
        public int moveSpeed;
        public bool isMoving;
        public GameObject target;
        public bool collided;
        public String collisionType = "homing";

        public Projectile(Vector2 position, int damage, GameObject target, int health = 1, int attackRange = 0)
            : base(health, position, damage, attackRange)
        {
            this.target = target;
            this.isMoving = true;
            //this.attackWait = 60;
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
            //  Do I need attack wait?
            end_point = Vector2.Add(this.destination, new Vector2(2, 2));
            prev_point = Vector2.Subtract(this.destination, new Vector2(2, 2));
            this.attackWait--;
            this.position += direction * moveSpeed;
        }
    }
}
