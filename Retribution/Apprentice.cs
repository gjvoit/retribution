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
    //  mage unit that can conjur a fireball
    class Apprentice : Mobile
    {
        public Fireball myArrow;
        public GameObject aiTarget;
        private static Timer fireballTimer = new Timer(1);  //  1 for testing purposes, should be 4000 possibly depending on balance issues
        private static Timer animateTimer = new Timer(250);
        public string state;
        public Texture2D image;
        SoundEffect soundEffect;
        

        public Apprentice(Vector2 position, int health = 12, int damage = 12, int attackRange = 150)
            : base(health, position, damage, attackRange)
        {
            this.type = "APPRENTICE";
            this.attackSpeed = 180;
            this.moveSpeed = 1;
            this.position = position;
            //this.animationState        //  The actual animation the object is performing (moving left, moving right, attacking, etc.)
            //this.animationFrame   //  Keeps track of the animation frame the object is on
            //this.animationTime    //  Calculates how much time has passed since animation began
            //this.attackReady = false;
            fireballTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            animateTimer.Elapsed += new ElapsedEventHandler(OnTimedEventAnimation);
        }

        //  On skill key press, conjur a fireball and send it to target location
        //  TyNote: Create a fireball unit and set destination to target location (not homing). Dies on first collision or at end of path.
        //  TyDo: Fireball has a life timer
        //  TyDo: Make this a spell. For now, it's implemented as the autoattack
        public void fireball(ProjectileManager projMan, ContentManager content, GameObject target, Vector2 destination)
        {
            Fireball fireballTest = null;
            if (!fireballTimer.Enabled)
            {
                fireballTimer.Start();
                fireballTest = new Fireball(this.position, 100, target, 100, 0);
                Vector2 direction = MovementManager.getNormalizedVector(this.position, destination);
                fireballTest.setDestination(direction, destination);
                fireballTest.LoadContent(content);
                projMan.proj.Add(fireballTest);
            }
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            fireballTimer.Stop();
        }
        private static void OnTimedEventAnimation(object source, ElapsedEventArgs e)
        {
            //  TyDo: If frame is last, stop timer. Else, increment frame counter
            //  TyDo: May not need animate method perhaps?
        }
        public override void kill(ContentManager content)
        {
            SoundEffect soundEffect = content.Load<SoundEffect>("death.wav");
            soundEffect.Play();
        }

        //  TyDo: Create a new projectile that deals less damage than an arrow, but for now just make it the fireball
        //  Attack the target (not homing). Create a new object projectile sent in the direction of target
        //  The new attack code
        public override void Attack(GameObject target, ContentManager content, ProjectileManager projMan)
        {
            this.soundEffect = content.Load<SoundEffect>("fireball.wav");
            soundEffect.Play();
            Vector2 corrected = Vector2.Add(position, new Vector2(16, 16));
            Projectile projectile = new Iceball(corrected, 100, target, 100, 0);
            Vector2 direction = MovementManager.getNormalizedVector(projectile.position, target.position);
            projectile.setDestination(direction, target.position);
            projectile.LoadContent(content);
            projMan.proj.Add(projectile);
        }

        //  Load the image?
        public override void LoadContent(ContentManager content)
        {
            this.texture = content.Load<Texture2D>("apprentice.png");
        }
    }
}