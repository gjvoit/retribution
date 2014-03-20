using System;
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

        public GameObject(int health, Vector2 position, int damage, int attack_range, int movement_range)
        {
            this.health = health;
            this.position = position;
            this.damage = damage;
            this.attack_range = attack_range;
            this.movement_range = movement_range;
            this.move_speed = 1;
        }

        public void Move()
        {
        }

        public void Attack(GameObject target)
        {
        }

        public void Die()
        {
            if (this.health <= 0)
            {
                //some animation?
            }
        }


    }
}
