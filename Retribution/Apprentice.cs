﻿using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Timers;

namespace Retribution
{
    //  mage unit that can conjur a fireball
    class Apprentice : Mobile
    {
        public Fireball myArrow;
        public GameObject aiTarget;
        public double fireballCD = 4.0;
        private static Timer fireballTimer = new Timer(4000);
        private static Timer animateTimer = new Timer(250);
        public string state;
        public Texture2D image;

        public Apprentice(Vector2 position, int health = 2, int damage = 3, int attack_range = 2)
            : base(health, position, damage, attack_range)
        {
            this.health = 12;
            this.damage = 1;
            this.attackSpeed = 2;
            this.attackRange = 6;
            this.moveSpeed = 6;
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
        public void fireball()
        {
            if (!fireballTimer.Enabled)
            {
                fireballTimer.Start();
                //  TyDo: Create a new fireball projectile
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

        //  TyDo: Create a new projectile that deals less damage than an arrow, but for now just make it the fireball
        //  Attack the target (not homing). Create a new object projectile sent in the direction of target
        public override void Attack(GameObject target)
        {
            aiTarget = target;
            myArrow = makeArrow(target);
            Vector2 direction = MovementManager.getNormalizedVector(this.myArrow.position, target.position);
            myArrow.setDestination(direction, target.position);
        }
        public Fireball makeArrow(GameObject target)
        {
            Vector2 corrected = Vector2.Add(position, new Vector2(16, 16));
            return new Fireball(corrected, ref target);

        }

        //  Load the image?
        public override void LoadContent(ContentManager content)
        {
            this.texture = content.Load<Texture2D>("apprentice.png");
        }
    }
}