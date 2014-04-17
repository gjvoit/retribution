using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.Timers;
namespace Retribution
{
    //  A tanky, high healthed, unit. Has armor that takes extra damage from magic
    class Warrior : Mobile
    {
        public static int cost = 5;
        private Timer juggernautTimer = new Timer(4000);
        public Warrior(Vector2 position, int health = 40, int damage = 15, int attackRange = 50)
            : base(health, position, damage, attackRange)
        {
            //this.health = 40;
            //this.damage = 6;
            //this.attackSpeed = 2;
            this.moveSpeed = 1;
            attackSpeed = 320;
            type = "WARRIOR";
            juggernautTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            //this.animationState        //  The actual animation the object is performing (moving left, moving right, attacking, etc.)
            //this.animationFrame   //  Keeps track of the animation frame the object is on
            //this.animationTime    //  Calculates how much time has passed since animation began
            //this.attackReady = false;
            //this.isArmored = true;
        }
        public override void Attack(GameObject target, ContentManager content, ProjectileManager projMan)
        {
            //if (!juggernautTimer.Enabled)
            //{
            //    damage = 15;
            //    attackSpeed = 320;
            //    moveSpeed = 1;
            //    specialAttack = false;
            //    attackRange = 50;
            //}

            attackSound(content);
            target.health -= this.damage;
            this.animateState = "attack";
            this.animateTime = 0;
        }
        public void juggernaut()
        {
            if (!juggernautTimer.Enabled)
            {
                attackWait = 0;
                specialAttack = true;
                juggernautTimer.Start();
                attackSpeed = 150;
                health -= (int)(health * .2);
                damage = 17;
                moveSpeed = 3;
                attackRange = 33;
            }
        }
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            damage = 15;
            attackSpeed = 320;
            moveSpeed = 1;
            specialAttack = false;
            attackRange = 50;
            juggernautTimer.Stop();
        }
        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("warrior_spritesheet.png");
        }
        public override void attackSound(ContentManager content)
        {
            SoundEffect soundEffect = content.Load<SoundEffect>("clang.wav");
            soundEffect.Play();
        }

    }
}