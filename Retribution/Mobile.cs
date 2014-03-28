using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Retribution
{
    abstract class Mobile : GameObject
    {

        public Vector2 destination
        {
            get;
            set;
        }

        public int moveSpeed
        {
            get;
            set;
        }

        public int attackRange
        {
            get;
            set;
        }

        public bool isMoving
        {
            get;
            set;
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

        public void mount();



    }
}
