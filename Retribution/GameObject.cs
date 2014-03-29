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
        public Texture2D texture;
        public int health;
        public int damage;
        public int attackRange;
        public bool alive;
        public bool selected;
        

        // stuff that needs to moved from GameObject to Mobile
        //public int moveSpeed;
        //public Vector2 direction;
        //public Vector2 destination;
        //public bool isMoving;

        // stuff that can be removed b/c of Mobile
        //public bool canMove;

        public GameObject(int health, Vector2 position, int damage, int attackRange)
        {
            this.health = health;
            this.position = position;
            this.damage = damage;
            this.attackRange = attackRange;
            this.alive = true;
            this.selected = false;

            // stuff that needs to be moved from GO to Mobile
            //this.moveSpeed = 1;
            //this.isMoving = false;

            // stuff that can be removed b/c of Mobile
            //this.canMove = false;
        }

        //  A rectangle to represent the object
        public Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, this.texture.Width, this.texture.Height); }
        }

        //  Move this method to mobile
        //public void move()
        //{
        //    if (canMove)
        //    {
        //        this.isMoving = true;
        //        Vector2 end_point = Vector2.Add(this.destination, new Vector2(2, 2));
        //        Vector2 prev_point = Vector2.Subtract(this.destination, new Vector2(2, 2));
        //        if (this.position.X <= end_point.X && this.position.X >= prev_point.X
        //            && this.position.Y <= end_point.Y && this.position.Y >= prev_point.Y)
        //        {
        //            this.isMoving = false;
        //            return;
        //        }
        //        position += direction*moveSpeed;
        //    }
        //}

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

        // Move this to Mobile
        //public void setDestination(Vector2 theVector, Vector2 destination)
        //{
        //    this.direction = theVector;
        //    this.destination = destination;
        //}

        public Boolean isSelectable(MouseState mouse)
        {
            if ((mouse.X >= this.Bounds.Left && mouse.X <= this.Bounds.Right)
                && (mouse.Y >= this.Bounds.Top && mouse.Y <= this.Bounds.Bottom))
                return true;
            else
            {
                return false;
            }
        }
    }
}
