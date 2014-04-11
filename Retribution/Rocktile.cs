using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Retribution
{
    //  A rock projectile launched at an arc
    //  TyNote: Refer to Pawn.cs for more general notes on GameObject classes
    class Rocktile : Projectile
    {
        public string state;
        public Texture2D image;

        public Rocktile(Vector2 position, int damage, GameObject target, int health = 1, int attackRange = 0)
            : base(position, damage, target, health, attackRange)
        {
            this.position = position;
            this.moveSpeed = 6;
            this.damage = 100;
            this.attackRange = 50;
            this.collisionType = "arc";
            
            //this.health = 1000;
            //this.damage = 8;
            //this.attackSpeed = 0;
            //this.attackRange = 1;
            //this.moveSpeed = 20;
            //this.canMove = true;
            //this.position = position;
            //this.animationState        //  The actual animation the object is performing (moving left, moving right, attacking, etc.)
            //this.animationFrame   //  Keeps track of the animation frame the object is on
            //this.animationTime    //  Calculates how much time has passed since animation began
            //this.attackReady = true;
            //this.collisionType = "arc'7";
        }

        //  Load the image?
        public override void LoadContent(ContentManager content)
        {
            this.texture = content.Load<Texture2D>("rocktile.png");
        }
    }
}