using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Retribution
{
    //  A tanky, high healthed, unit. Has armor that takes extra damage from magic
    class Warrior : Mobile
    {

        public Warrior(Vector2 position, int health = 4, int damage = 1, int attackRange = 1)
            : base(health, position, damage, attackRange)
        {
            this.health = 40;
            this.damage = 6;
            //this.attackSpeed = 2;
            this.attackRange = 1;
            this.moveSpeed = 5;
            this.position = position;
            //this.animationState        //  The actual animation the object is performing (moving left, moving right, attacking, etc.)
            //this.animationFrame   //  Keeps track of the animation frame the object is on
            //this.animationTime    //  Calculates how much time has passed since animation began
            //this.attackReady = false;
            //this.isArmored = true;
        }

        public override void LoadContent(ContentManager content)
        {
            this.texture = content.Load<Texture2D>("warrior.png");
        }


    }
}