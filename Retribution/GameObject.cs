using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Retribution
{
    abstract class GameObject
    {
        public int health;
        public Vector2 position;
        public int damage;
        public int attack_range;
        public int move_speed;
        public bool can_move;
        public List<Vector2> tiles_in_range;
        public bool alive;

        public GameObject(int health, Vector2 position, int damage, int attack_range)
        {
            this.health = health;
            this.position = position;
            this.damage = damage;
            this.attack_range = attack_range;
            this.move_speed = 1;
            this.can_move = false;
            this.tiles_in_range = this.SetRange();
            this.alive = true;
        }

        public void Move(Vector2 destination)
        {
            if (can_move)
            {
                // get the distance
                // get the normal vector
                // multiply the normal vector with speed and time
                // add the result to current position
            }
        }

        public List<Vector2> SetRange()
        {
            tiles_in_range = new List<Vector2>();
            if (attack_range > 0)
            {
                for (int i = 1; i <= this.attack_range; i++)
                {
                    Vector2 extendy = new Vector2(0, i);
                    Vector2 extendx = new Vector2(i, 0);
                    // covers basic range in cardinal directions
                    // need to handle diagonals somehow
                    Vector2 covered = Vector2.Add(this.position, extendy);
                    tiles_in_range.Add(covered);
                    covered = Vector2.Add(this.position, extendx);
                    tiles_in_range.Add(covered);
                    covered = Vector2.Subtract(this.position, extendy);
                    tiles_in_range.Add(covered);
                    covered = Vector2.Subtract(this.position, extendx);
                    tiles_in_range.Add(covered);
                }
            }
            return tiles_in_range;
        }

        public void Attack(GameObject target)
        {
            target.health -= this.damage;
        }

        public bool inRange(Vector2 position)
        {
            return this.tiles_in_range.IndexOf(position) != -1;
        }

        public abstract void Die();

    }
}
