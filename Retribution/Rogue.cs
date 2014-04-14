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
    //  stealthy assassination unit capable of killing key units or injured units undetected
    //  TyNote: Refer to Pawn.cs for more general notes on GameObject classes
    class Rogue : Mobile
    {
        public static int cost = 15;
        public double stealthCD = 15.0;      //  Time till next stealth can be executed
        public Timer stealthTimer = new Timer(8000);
        //public string state;
        //public Texture2D image;
     
        public override void attackSound(ContentManager content)
        {
            SoundEffect soundEffect = content.Load<SoundEffect>("blade.wav");
            soundEffect.Play();
        }

        public Rogue(Vector2 position, int health =20, int damage = 6, int attackRange = 40)
            : base(health, position, damage, attackRange)
        {
            type = "ROGUE";
            this.attackSpeed = 60;
            this.moveSpeed = 5;
            this.position = position;
            stealthTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            //this.animationState        //  The actual animation the object is performing (moving left, moving right, attacking, etc.)
            //this.animationFrame   //  Keeps track of the animation frame the object is on
            //this.animationTime    //  Calculates how much time has passed since animation began
            //this.attackReady = false;
        }
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            specialAttack = false;
            stealthTimer.Stop();
        }
        public override void kill(ContentManager content)
        {
            SoundEffect soundEffect = content.Load<SoundEffect>("death.wav");
            soundEffect.Play();
        }
        public override void Attack(GameObject target, ContentManager content, ProjectileManager projMan)
        {
            base.Attack(target, content, projMan);
            if (stealthTimer.Enabled)
            {
                //stealthTimer.Start();
                foreach (GameObject unit in ModelManager.artificial)
                {
                    if (unit.aiTarget == this)
                        unit.aiTarget = null;
                }
            }
            
        }
        //  On skill key press, hides the image of the unit
        //  TyNote: For now, we simply hide the rogue unit by changing its image. No indication of its location or transparency yet
        //  This means the target is also still targetable, if the user randomly clicks and knows where the unit is at
        public void stealth()
        {
            if (!stealthTimer.Enabled)
            {
                specialAttack = true;
                stealthTimer.Start();
                           
                    foreach (GameObject unit in ModelManager.artificial)
                    {
                        if (unit.aiTarget == this)
                            unit.aiTarget = null;
                    }
                            }
        
        }

        //  Increment cooldown timer
        public void updateCD(GameTime gameTime)
        {
            if (this.stealthCD < 15.0)
            {
                //  TyDO: If time elapsed is 1 second,
                this.stealthCD++;
            }
        }

        //  Load the image?
        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Rogue.png");
        }
    }
}