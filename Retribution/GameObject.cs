using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

namespace Retribution
{
    //  The overarching "Grandpa" class of all objects in the game (projectiles, towers, walls, boulders, etc.)
    public abstract class GameObject
    {
        public Vector2 position;
        public Texture2D texture;
        public GameObject aiTarget;
        public int health;
        public int damage;
        public int attackRange;
        public int basehealth;
        public bool alive;
        public bool selected;
        public string type;
        public int attackWait;
        public int attackSpeed;
        public bool attacked=false;
        public int ssX = 0;     //  Sprite Sheet x coordinate
        public int ssY = 0;     //  Sprite Sheet y coordinate
        public int imageSize = 32;  //  Sprite image size
        public String animateState = "";   //  Sprite animation state
        public int animateTime = 0;
        public bool isUp = true;   //  Direction variable used to help with animation
        public bool isRight = true;
        SoundEffect soundEffect;

        public GameObject(int health, Vector2 position, int damage, int attackRange)
        {
            this.health = health;
            this.basehealth = health;
            this.position = position;
            this.damage = damage;
            this.attackRange = attackRange;
            this.alive = true;
            this.selected = false;
            this.attackSpeed = 180;
            this.attackWait = 0;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (selected)
            {
                Vector2 temp = Vector2.Subtract(position, new Vector2(attackRange-16, attackRange-16));
                spriteBatch.Draw(createCircle(attackRange, spriteBatch.GraphicsDevice), temp, Color.Cyan);
                                        }
            spriteBatch.Draw(texture, new Rectangle((int)this.position.X, (int)this.position.Y, imageSize, imageSize), new Rectangle(ssX*32, ssY*32, imageSize, imageSize), Color.White);
            if(selected)
                spriteBatch.Draw(createHPBar(this.health, spriteBatch.GraphicsDevice), position, Color.White);
        }
        public virtual void Draw(SpriteBatch spriteBatch, Color color)
        {
            //Vector2 temp = Vector2.Subtract(position, new Vector2(attackRange - 16, attackRange - 16));
            //spriteBatch.Draw(createCircle(attackRange, spriteBatch.GraphicsDevice), temp, Color.Crimson);
            spriteBatch.Draw(texture, new Rectangle((int)this.position.X, (int)this.position.Y, imageSize, imageSize), new Rectangle(ssX * 32, ssY * 32, imageSize, imageSize), color);
            spriteBatch.Draw(createHPBar(this.health, spriteBatch.GraphicsDevice), position, color);
        }
        public void resetAttack()
        {
            attackWait = attackSpeed;
            attacked = false;

        }
        public virtual void kill(ContentManager content)
        {
            
        
        }
        //  A rectangle to represent the object
        public Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, this.texture.Width, this.texture.Height); }
        }
        public virtual void attackSound(ContentManager content)
        {
           
        }

        //  Issue attack. Alpha method that damages target. No other skills or actions are implemented in the Alpha Version
        public virtual void Attack(GameObject target, ContentManager content, ProjectileManager projMan)
        {
            attackSound(content);
            target.health -= this.damage;
        }

        public Boolean IsInRange(GameObject target)
        {
           double distance;
           distance = (int) Math.Sqrt(Math.Pow((this.position.X - target.position.X), 2) + Math.Pow((this.position.Y - target.position.Y), 2));
           if (distance <= this.attackRange)
           {
               //aiTarget = target;
               return true;
           }
           else return false;
        }

        //public abstract void Die();

        //  Return true if this object collides with target object
        public bool collidesWith(GameObject target)
        {
            if (this.Bounds.Intersects(target.Bounds))
                return true;

            else
                return false;
        }

        //  Returns true if object is alive
        public virtual bool isAlive()
        {
            if (health > 0)
                return true;
            else
            {
                return false;
            }
        }

        //  Getters and Setters
        public Vector2 getPosition()
        {
            return this.position;
        }

        public Boolean isSelectable(MouseState mouse)
        {
            if ((mouse.X >= this.Bounds.Left && mouse.X <= this.Bounds.Right)
                && (mouse.Y >= this.Bounds.Top && mouse.Y <= this.Bounds.Bottom))
                return true;
            else
            {
                return false;
            }
        }

        public abstract void LoadContent(ContentManager content);
        public void Animate()
        {
        }
        public Texture2D createHPBar(int health, GraphicsDevice arg)
        {
            Texture2D texture = new Texture2D(arg, 32, 2);
            Color[] data = new Color[32 * 2];
            for (int i = 0; i < data.Length; i++)
                data[i] = Color.Red;
            int scaled = health *64 / basehealth;
            for (int j = 0; j < scaled/2; j++)
                data[j] = Color.LawnGreen;
            for (int p = 32; p < 32+scaled/2; p++)
                data[p] = Color.LawnGreen;
            texture.SetData(data);
            return texture;

        }
        public Texture2D createCircle(int radius, GraphicsDevice arg)
        {
            int outerRadius = radius * 2 + 2; // So circle doesn't go out of bounds
            Texture2D texture = new Texture2D(arg, outerRadius, outerRadius);

            Color[] data = new Color[outerRadius * outerRadius];

            // Colour the entire texture transparent first.
            for (int i = 0; i < data.Length; i++)
                data[i] = Color.Transparent;

            // Work out the minimum step necessary using trigonometry + sine approximation.
            double angleStep = 1f / radius;

            for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
            {
                // Use the parametric definition of a circle: http://en.wikipedia.org/wiki/Circle#Cartesian_coordinates
                int x = (int)Math.Round(radius + radius * Math.Cos(angle));
                int y = (int)Math.Round(radius + radius * Math.Sin(angle));

                data[y * outerRadius + x + 1] = Color.White;
            }

            texture.SetData(data);
            return texture;
        }
    }
}
