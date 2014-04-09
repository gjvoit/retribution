using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Retribution
{
    //  Arillery unit capable of taking down structures effectively. Its attack is also an arced AOE projectile
    //  TyNote: Refer to Pawn.cs for more general notes on GameObject classes
    class Catapult : Mobile
    {
        public string state;
        public Texture2D image;

        public Catapult(Vector2 position, int health = 20, int damage = 3, int attack_range = 2)
            : base(health, position, damage, attack_range)
        {
            this.health = 20;
            this.damage = 80;
            this.attackSpeed = 8;
            this.attackRange = 8;
            this.moveSpeed = 4;
            this.position = position;
            this.imageSize = 64;
            //this.animationState        //  The actual animation the object is performing (moving left, moving right, attacking, etc.)
            //this.animationFrame   //  Keeps track of the animation frame the object is on
            //this.animationTime    //  Calculates how much time has passed since animation began
            //this.attackReady = false;
        }

        //  Issue a command to launch a projectile at target location
        //  TyDo: Arced projectile. Collision only matters at the very end of its impact. Damages all enemy units that collide

        public void Attack()
        {
            //  TyDo: Create new projectile at catapults's position of type arced, set destination to target location
        }

        //  Load the image?
        public override void LoadContent(ContentManager content)
        {
            this.texture = content.Load<Texture2D>("catapult.png");
        }
    }
}