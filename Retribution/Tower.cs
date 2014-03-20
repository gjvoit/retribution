using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Retribution
{
    class Tower : GameObject
    {
        public Tower(int health, Vector2 position, int damage, int attack_range, int movement_range)
            : base (health, position, damage, attack_range, movement_range)
        {
            this.health = 2;
            this.position = position;
            this.damage = 0;
            this.attack_range = 0;
            this.movement_range = 0;
        }
    }
}
