﻿using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Retribution
{
    //  stealthy assassination unit capable of killing key units or injured units undetected
    //  TyNote: Refer to Pawn.cs for more general notes on GameObject classes
    class Rogue : Mobile
    {
        public double stealthCD = 15.0;      //  Time till next stealth can be executed
        public string state;
        public Texture2D image;

        public Rogue(Vector2 position, int health = 2, int damage = 3, int attack_range = 2)
            : base(health, position, damage, attack_range)
        {
            this.health = 4;
            this.damage = 12;
            this.attackSpeed = 1;
            this.attackRange = 1;
            this.moveSpeed = 12;
            this.position = position;
            //this.animationState        //  The actual animation the object is performing (moving left, moving right, attacking, etc.)
            //this.animationFrame   //  Keeps track of the animation frame the object is on
            //this.animationTime    //  Calculates how much time has passed since animation began
            //this.attackReady = false;
        }

        //  On skill key press, hides the image of the unit
        //  TyNote: For now, we simply hide the rogue unit by changing its image. No indication of its location or transparency yet
        //  This means the target is also still targetable, if the user randomly clicks and knows where the unit is at
        public void stealth()
        {
            if (this.stealthCD >= 15.0)
            {
                stealthCD = 0.0;
                //this.image = "none";
                //  Later on, there will actually be a transparency animation, but animation also acts as a good way to set a timer,
                //  which is exactly what stealth needs.
                //this.animation = "stealth";
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
            this.texture = content.Load<Texture2D>("Rogue.png");
        }
    }
}