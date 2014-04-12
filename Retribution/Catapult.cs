using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
namespace Retribution
{
    //  Arillery unit capable of taking down structures effectively. Its attack is also an arced AOE projectile
    //  TyNote: Refer to Pawn.cs for more general notes on GameObject classes
    class Catapult : Mobile
    {
        public static int cost = 45;
        //public string state;
        //public Texture2D image;
        public Catapult(Vector2 position, int health = 20, int damage = 8, int attackRange = 375)
            : base(health, position, damage, attackRange)
        {
            type = "CATAPULT";
            this.attackSpeed = 800;
            this.moveSpeed = 1;
            this.imageSize = 64;
            //this.animationState        //  The actual animation the object is performing (moving left, moving right, attacking, etc.)
            //this.animationFrame   //  Keeps track of the animation frame the object is on
            //this.animationTime    //  Calculates how much time has passed since animation began
            //this.attackReady = false;
        }

        //  Issue a command to launch a projectile at target location
        //  TyDo: Arced projectile. Collision only matters at the very end of its impact. Damages all enemy units that collide
        public override void attackSound(ContentManager content)
        {
            SoundEffect soundEffect = content.Load<SoundEffect>("catapult.wav");
            soundEffect.Play();
        }
        public override void kill(ContentManager content)
        {
            SoundEffect soundEffect = content.Load<SoundEffect>("death.wav");
            soundEffect.Play();
        }
        public override void Attack(GameObject target, ContentManager content, ProjectileManager projMan)
        {
            //attackSound(content);
            Vector2 corrected = Vector2.Add(position, new Vector2(16, 16));
            Projectile projectile = new Rocktile(corrected, 100, target, 100, 0);
            Vector2 direction = MovementManager.getNormalizedVector(projectile.position, target.position);
            projectile.setDestination(direction, target.position);
            projectile.LoadContent(content);
            projMan.proj.Add(projectile);
        }

        //  Load the image?
        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("catapult.png");
        }
    }
}