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
        public int movement_range;
        public int move_speed;
        public bool can_move;
        public List<Vector2> tiles_in_range;

        public GameObject(int health, Vector2 position, int damage, int attack_range, int movement_range)
        {
            this.health = health;
            this.position = position;
            this.damage = damage;
            this.attack_range = attack_range;
            this.movement_range = movement_range;
            this.move_speed = 1;
            this.can_move = false;
            this.tiles_in_range = this.SetRange();
        }

        public abstract void Move();

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
            // for right now turnbased is fine; later on though should put in while loop
            target.health -= this.damage;
        }

        public bool inRange(Vector2 position)
        {
            return this.tiles_in_range.IndexOf(position) != -1;
        }

        public abstract void Die();

    }
}
