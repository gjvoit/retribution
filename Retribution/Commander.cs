using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
namespace Retribution
{

    //  uber unit that can dash
    //  TyNote: Refer to Pawn.cs for more general notes on GameObject classes
    class Commander : Mobile
    {
        public double shieldCD = 15.0;
        public double dashCD = 2.0;
        public string state;
        public Texture2D image;
        public override void attackSound(ContentManager content)
        {
            SoundEffect soundEffect = content.Load<SoundEffect>("blade.wav");
            soundEffect.Play();
        }
        public Commander(Vector2 position, int health = 30, int damage = 6, int attackRange = 100)
            : base(health, position, damage, attackRange)
        {
            this.attackSpeed = 180;
            this.moveSpeed = 6;
            //this.animationState        //  The actual animation the object is performing (moving left, moving right, attacking, etc.)
            //this.animationFrame   //  Keeps track of the animation frame the object is on
            //this.animationTime    //  Calculates how much time has passed since animation began
            //this.attackReady = false;
        }

        //  When receiving damage, negate the initial attack and give a 15 point shield lasting for 5 seconds
        //  TyNote: For now, every 15 seconds that passes the commander regains half its hp back.

        //  Increment cooldown timer
        public void updateCD(GameTime gameTime)
        {
            if (this.shieldCD < 15.0)
            {
                //  TyDO: If time elapsed is 1 second,
                this.shieldCD++;
            }
            else if (this.shieldCD >= 15.0)
            {
                if (this.health <= 15)
                {
                    this.health += 15;
                }
                else
                {
                    this.health = 30;
                }
                this.shieldCD = 0.0;
            }

            if (this.dashCD < 2.0)
            {
                this.dashCD++;
            }
        }

        //  When skill key is pressed, dash to target location very quickly
        //  TyNote: For now, increase movement speed temporarily
        public void dash()
        {
            if (this.dashCD >= 2.0)
            {
                this.dashCD = 0.0;
                this.moveSpeed *= 2;
                //this.animate = "dash";
            }
        }

        //  Load the image?
        public override void LoadContent(ContentManager content)
        {
            this.texture = content.Load<Texture2D>("commander.png");
        }
    }
}