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

    //  uber unit that can dash
    //  TyNote: Refer to Pawn.cs for more general notes on GameObject classes
    class BossUnit : Mobile
    {
        public double shieldCD = 15.0;
        public double dashCD = 2.0;
        public static int cost = 20;
        public Timer poundTimer = new Timer(3000);
        public Timer poundCD = new Timer(7000);
        //public string state;
        //public Texture2D image;
        public override void kill(ContentManager content)
        {
            SoundEffect soundEffect = content.Load<SoundEffect>("death.wav");
            soundEffect.Play();
        }
        //public override void attackSound(ContentManager content)
        //{
        //    SoundEffect soundEffect = content.Load<SoundEffect>("blade.wav");
        //    soundEffect.Play();
        //}
        public BossUnit(Vector2 position, int health = 40, int damage = 10, int attackRange = 100)
            : base(health, position, damage, attackRange)
        {
            type = "BOSS";
            this.attackSpeed = 200;
            this.moveSpeed = 2;
            //this.animationState        //  The actual animation the object is performing (moving left, moving right, attacking, etc.)
            //this.animationFrame   //  Keeps track of the animation frame the object is on
            //this.animationTime    //  Calculates how much time has passed since animation began
            //this.attackReady = false;
            poundTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            poundCD.Elapsed += new ElapsedEventHandler(OnTimedEvent2);
        }
        public void pound()
        {
            if (!poundTimer.Enabled&&!poundCD.Enabled)
            {
                poundTimer.Start();
                specialAttack = true;
                    foreach (GameObject unit in ModelManager.player)
                    {
                        if (unit.GetType().BaseType == typeof(Mobile))
                        {
                            if (this.IsInRange(unit)&&unit!=this)
                            {
                                List<GameObject> filler = new List<GameObject>();
                                filler.Add(unit);
                                Vector2 debug = Vector2.Normalize(Vector2.Subtract(unit.position, this.position));
                                Vector2 debug2=Vector2.Multiply(debug,this.attackRange);
                                Vector2 pvec =Vector2.Add(this.position, debug2);
                                int temp = ((Mobile)unit).moveSpeed;
                                ((Mobile)unit).moveSpeed = 900;
                                bool mov = ((Mobile)unit).isMoving;
                                bool paus = ((Mobile)unit).isPaused;
                                ((Mobile)unit).isMoving = false;
                                ((Mobile)unit).isPaused = false;
                                bool sel = unit.selected;
                                unit.selected = true;
                                MovementManager.changeDestination(filler, pvec);
                                unit.selected = sel;
                                ((Mobile)unit).isMoving = mov;
                                ((Mobile)unit).moveSpeed = temp;
                                ((Mobile)unit).isPaused = paus;
                                unit.health--;
                            }
                        }
                    }
                    
            }

        }
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            specialAttack = false;
            poundTimer.Stop();
            poundCD.Start();
        }
        private void OnTimedEvent2(object source, ElapsedEventArgs e)
        {
            poundCD.Stop();
        }

        public Vector2 circleFire(int x)
        {
            if (x == 0)
                return new Vector2(this.position.X, this.position.Y + 32);
            if (x == 1)
                return new Vector2(this.position.X + 32, this.position.Y);
            if (x == 2)
                return new Vector2(this.position.X - 32, this.position.Y);
            else
                return new Vector2(this.position.X, this.position.Y - 32);

        }
        public override void Attack(GameObject target, ContentManager content, ProjectileManager projMan)
        {
            Vector2 corrected = Vector2.Add(position, new Vector2(16, 16));
            for (int x = 0; x < 4; x++)
            {
                Fireball projectile = new Fireball(corrected, 0, this, 1, 0);
                Vector2 direction = MovementManager.getNormalizedVector(projectile.position, circleFire(x));
                projectile.setDestination(direction, circleFire(x));
                projectile.LoadContent(content);
                projMan.proj.Add(projectile);
                            
                
                
            }
            
        }
        //  Load the image?
        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("boss.png");
        }
        public override void Animate()
        {
            
        }
    }
}