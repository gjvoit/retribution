using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace Retribution
{
    //  The overarching "Grandpa" class of all objects in the game (projectiles, towers, walls, boulders, etc.)
    abstract class GameObject
    {
        public Vector2 position;
        public Vector2 destination;
        public Vector2 direction;
        public Texture2D texture;
        public int health;
        public int damage;
        public int attackRange;
        public int moveSpeed;
        public bool canMove;
        public bool alive;
        public bool selected;

        public GameObject(int health, Vector2 position, int damage, int attackRange)
        {
            this.health = health;
            this.position = position;
            this.damage = damage;
            this.attackRange = attackRange;
            this.moveSpeed = 1;
            this.canMove = false;
            this.alive = true;
            this.selected = false;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)this.position.X, (int)this.position.Y, 50, 50), Color.White);
        }
        //  A rectangle to represent the object
        public Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, this.texture.Width, this.texture.Height); }
        }
        

        //  Get a vector and move towards the destination
        public void move()
        {
            if (canMove)
            {
                Vector2 end_point = Vector2.Add(this.destination, new Vector2(2, 2));
                Vector2 prev_point = Vector2.Subtract(this.destination, new Vector2(2, 2));
                if (this.position.X <= end_point.X && this.position.X >= prev_point.X
                    && this.position.Y <= end_point.Y && this.position.Y >= prev_point.Y)
                    return;
                // get the distance
                position += direction*moveSpeed;
                Console.WriteLine(string.Format("new {0}", this.position));
                // get the slope
            }
        }

        //  Issue attack. Alpha method that damages target. No other skills or actions are implemented in the Alpha Version
        public void Attack(GameObject target)
        {
            target.health -= this.damage;
        }

        public Boolean IsInRange(GameObject target)
        {
           double distance;
           distance = (int) Math.Sqrt(Math.Pow((this.position.X - target.position.X), 2) + Math.Pow((this.position.Y - target.position.Y), 2));
           if (distance <= this.attackRange)
           {
               return true;
           }
           else return false;
        }

        //public abstract void Die();

        //  Return true if this object collides with target object
        public bool collidesWith(GameObject target)
        {
            if (this.Bounds.Intersects(target.Bounds))
                return true;
            return false;
        }

        //  Returns true if object is alive
        public bool isAlive()
        {
            if (health > 0)
                return true;
            return false;
        }

        //  Getters and Setters
        public Vector2 getPosition()
        {
            return this.position;
        }

        public void setDestination(Vector2 theVector, Vector2 destination)
        {
            this.direction = theVector;
            this.destination = destination;
            Console.WriteLine(string.Format("{0}", this.position));
            Console.WriteLine(string.Format("{0}", this.destination));
        }

        public Boolean isSelectable(MouseState mouse)
        {
            Console.WriteLine("checking selection");
            if ((mouse.X >= this.Bounds.Left && mouse.X <= this.Bounds.Right)
                && (mouse.Y >= this.Bounds.Top && mouse.Y <= this.Bounds.Bottom))
                return true;
            else {
                Console.WriteLine("not selected");
                return false;
            }
        }
    }
}
