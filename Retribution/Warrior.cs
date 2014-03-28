using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Retribution
{
    class Warrior : Mobile
    {

        public Warrior(Vector2 position, int health = 4, int damage = 1, int attackRange = 1)
        {
            this.moveSpeed = 2;
            this.position = position;
        }

    }
}
