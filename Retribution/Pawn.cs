using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

//  TyDo: Add the following to GameObject.cs
//  TyNote: To anyone reading this entire file, these are old comments that are no longer needed. I just want to keep them here as a reference for myself.
//  - private for all states
//  - double attackSpeed calculated as attack cooldown ( like in warcraft3)
//  - GameObject objTarget the targeted object, needed to be saved due to animation and lapse of time (it is no longer instantaneous)
//  - String animationState the state of the animation (legend provided in comments)
//  - int animateFrame the frame of animation
//  - int animateTime the time passed since animation begain
//  - bool attackReady a flag that allows the actual damage to be inflicted to target
//  - bool isArmored=false an armor type, not sure if we will implement more. For now, this just indicates unit takes extra damage from magic.
//  - bool isAnimate=false a flag to indicate whether or not to animate the object
//  - Texture2D image
//  - String collisionType=object by default, objects act as normal, as compared to arced and homing
//  TyDo: Change the following to Game1.cs
//  - LoadContent should just preload all images and spritesheets. We do not need to load them constantly.
//  TyDo: Potentially get rid of Update()
//  TyDo: Managers to handle the Act class
//  - InRangeManager is in charge of checking "isinrange" for all objects. If so, set animate=attack and isAnimate=true
//  - AnimateManager is in charge of just running animate. Will not run if isAnimate=false
//  - AttackManager is in charge of calling attack(). Will not run if attackReady=false
//  TyDo: Collision Manager
//  - Include a loop for projectiles
//  - If homing, ignore furthur actions if collided object is not its intended target. Then call its attack on target on collision
//  - If Arced, ignore collisions until it reaches its destination. Call attack on all collided objects at that instance
//  - Destroy projectile after resolving actions
//  TyDo: Cooldown Manager
//  - One Job: simply call updateCD on all units.

namespace Retribution
{
    //  The most basic melee unit. It is a pawn, think of these units as disposable, a filler for extra resources.
    class Pawn : Mobile
    {
        public Pawn(Vector2 position, int health = 18, int damage = 3, int attackRange = 90)
            : base(health, position, damage, attackRange)
        {
            //  Set stats
            //  TyNote: I did not want to mess with the git hub/version conflictions, so I did not modify parameters in GameObject.
            //  Instead, I just defined the stats down below
            this.attackSpeed = 200;
            this.moveSpeed = 3;
            //this.animationState        //  The actual animation the object is performing (moving left, moving right, attacking, etc.)
            //this.animationFrame   //  Keeps track of the animation frame the object is on
            //this.animationTime    //  Calculates how much time has passed since animation began
            //this.attackReady = false;
        }

        //  Load the image
        //  TyDo: We could potentially put this in the constructor and not even worry about this method
        //  TyDo: Another note. in Game1.cs, loadcontent is needed to load the individual images. I do not think we need to load each
        //  image for each object. I believe LoadContent just "readys" the image resource file to be drawn.
        //  TyDo: The LoadContent method loads every single image we will be using in our game. It's not many, so I feel this is appropriate
        public override void LoadContent(ContentManager content)
        {
            this.texture = content.Load<Texture2D>("pawn.png");
        }
        //  TyDo: What is our game loop exactly? Is it:
        //  - Before game loop: initialize and load content
        //  - Retrieve player input
        //  - Act on all objects
        //      - move object towards target (if move)
        //      - attack (if in range of target)
        //          - permits movement
        //          - do animation on frame
        //          - inflict damage, summon projectile, allow movement, etc., at the end of animation
        //  - Resolve on all objects
        //      - Check collision
        //      - Inflict damage, decrease health, etc.
        //      - Remove dead objects from list
        //  TyNote: This implies projectiles damage upon collision (and die afterwards), melee attacks that start its animation will always hit
        //  target, even if target is out of range after animation
    }
}