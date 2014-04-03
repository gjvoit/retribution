using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Retribution
{
    abstract class Projectile : GameObject
    {
        public Vector2 direction;
        public Vector2 destination;
        public int moveSpeed;
        public bool isMoving;

        public Projectile(Vector2 position, int damage, int health = 0, int attackRange = 0)
            : base(health, position, damage, attackRange)
        {
            this.isMoving = true;
        }

        public void setDestination(Vector2 direction, Vector2 destination)
        {
            this.direction = direction;
            this.destination = destination;
        }

        public void move()
        {
            Vector2 end_point = Vector2.Add(this.destination, new Vector2(2, 2));
            Vector2 prev_point = Vector2.Subtract(this.destination, new Vector2(2, 2));

            if (this.position.X <= end_point.X && this.position.X >= prev_point.X
                && this.position.Y <= end_point.Y && this.position.Y >= prev_point.Y)
                return;

            position += direction * moveSpeed;
        }
    }
}
