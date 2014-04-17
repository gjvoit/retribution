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
    
    class Archer : Mobile
    {
        public Projectile myArrow;
        private Timer rapidFireTimer = new Timer(2000);
        private Timer cdTimer = new Timer(10000);
//        public GameObject aiTarget;
        public static int cost = 2;
        SoundEffect soundEffect;
        public Archer(Vector2 position, int health = 20, int damage = 1, int attackRange = 130)
            : base(health, position, damage, attackRange)
        {
            this.type = "ARCHER";
            this.moveSpeed = 2;
            attackSpeed = 100;
            rapidFireTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            cdTimer.Elapsed += new ElapsedEventHandler(CDTimedEvent);
        }
        public override void attackSound(ContentManager content)
        {
            SoundEffect soundEffect = content.Load<SoundEffect>("bow.wav");
            soundEffect.Play();
        }
        public override void kill(ContentManager content)
        {
            SoundEffect soundEffect = content.Load<SoundEffect>("death.wav");
            soundEffect.Play();
        }
        
        //  The new attack code
        public override void Attack(GameObject target, ContentManager content, ProjectileManager projMan)
        {
            //if (!rapidFireTimer.Enabled)
            //{
            //    specialAttack = false;
            //    attackSpeed = 180;
            //}
            attackSound(content);
            Vector2 corrected = Vector2.Add(position, new Vector2(16, 16));
            Projectile projectile = new Arrow(corrected, 100, target, 100, 0);
            myArrow = projectile;
            Vector2 direction = MovementManager.getNormalizedVector(projectile.position, target.position);
            projectile.setDestination(direction, target.position);
            projectile.LoadContent(content);
            projMan.proj.Add(projectile);
            this.animateState = "attack";
            this.animateTime = 0;
        }
        public void rapidFire()
        {
            
            if (!rapidFireTimer.Enabled&&!cdTimer.Enabled)
            {
                specialAttack = true;
                rapidFireTimer.Start();
                attackWait = 0;
                attackSpeed = 20;    
            }
           
        }
        private void CDTimedEvent(object source, ElapsedEventArgs e)
        {
            cdTimer.Stop();
        }
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            attackWait += 180;
            specialAttack = false;
            attackSpeed = 180;
            rapidFireTimer.Stop();
            cdTimer.Start();
        }
        /*
        public override void Attack(GameObject target)
        {
            aiTarget = target;
            myArrow = makeArrow(target);
            Vector2 direction = MovementManager.getNormalizedVector(this.myArrow.position, target.position);
            myArrow.setDestination(direction, target.position);
        }
        public Arrow makeArrow(GameObject target)
        {
            Vector2 corrected = Vector2.Add(position, new Vector2(16, 16));
            return new Arrow(corrected, ref target);
            
        }
         * */

        public override void LoadContent(ContentManager content)
        {
           
            texture = content.Load<Texture2D>("archer_spritesheet.png");
        }

    }
}
