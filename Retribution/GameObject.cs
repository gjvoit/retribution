using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

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
        public int moveSpeed;
        public bool canMove;
        public bool alive;
        //public List<Vector2> tilesInRange;

        public GameObject(int health, Vector2 position, int damage, int attackRange)
        {
            this.health = health;
            this.position = position;
            this.damage = damage;
            this.attackRange = attackRange;
            this.moveSpeed = 1;
            this.canMove = false;
            //this.tilesInRange = this.SetRange();
            this.alive = true;
        }

        //  A rectangle to represent the object
        public Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height); }
        }

        //  Get a vector and move towards the destination
        public void Move(Vector2 destination)
        {
            if (canMove)
            {
                // get the distance
                position += destination;
                // get the slope
            }
        }

        /*
        //  Set the attack range of object
        public List<Vector2> SetRange()
        {
            tilesInRange = new List<Vector2>();
            if (attackRange > 0)
            {
                for (int i = 1; i <= this.attackRange; i++)
                {
                    Vector2 extendy = new Vector2(0, i);
                    Vector2 extendx = new Vector2(i, 0);
                    // covers basic range in cardinal directions
                    // need to handle diagonals somehow
                    Vector2 covered = Vector2.Add(this.position, extendy);
                    tilesInRange.Add(covered);
                    covered = Vector2.Add(this.position, extendx);
                    tilesInRange.Add(covered);
                    covered = Vector2.Subtract(this.position, extendy);
                    tilesInRange.Add(covered);
                    covered = Vector2.Subtract(this.position, extendx);
                    tilesInRange.Add(covered);
                }
            }
            return tilesInRange;
        }
        */

        //  Issue attack. Alpha method that damages target. No other skills or actions are implemented in the Alpha Version
        public void Attack(GameObject target)
        {
            target.health -= this.damage;
        }

        //  Return true if passed in vector is within attack range
        /*
        public bool inRange(Vector2 position)
        {
            return this.tilesInRange.IndexOf(position) != -1;
        }
        */

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

    }
}
