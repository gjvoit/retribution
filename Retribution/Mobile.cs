﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Retribution
{
    abstract class Mobile : GameObject
    {

        public Vector2 direction;
        public Vector2 destination;
        public int moveSpeed;
        public bool isMoving;

        public Mobile(int health, Vector2 position, int damage, int attackRange)
            : base(health, position, damage, attackRange)
        {
            this.isMoving = false;
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

        public void Update(GameTime gameTime)
        {
            if (this.isAlive())
            {
                // TODO: put code here for when archer is alive
            }
        }

        public abstract void LoadContent(ContentManager content);

    }
}
