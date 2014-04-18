using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;


namespace Retribution
{
    abstract class Mobile : GameObject
    {

        public Vector2 direction;
        public Vector2 destination;
        public int moveSpeed;
        public bool isMoving;
        public List<Tile> pathList;
        public bool isPaused;
        public List<Tile> collisionList;
        public int pauseTimer;

        public Mobile(int health, Vector2 position, int damage, int attackRange)
            : base(health, position, damage, attackRange)
        {
            this.isMoving = false;
            this.isPaused = false;
            this.pauseTimer = 0;
            pathList = new List<Tile>();
            collisionList = new List<Tile>();
            this.animateState = "move";
        }

       public void setDestination(Vector2 destination)
        {
            //this.direction = direction;
            this.destination = destination;
        }

        public void move()
        {
            //Animate();
            if (this.pathList == null || this.pathList.Count == 0)
            {
                this.isMoving = false;
                return;
            }

            else
            {

                Vector2 end_point = Vector2.Add(new Vector2(this.pathList[0].Bounds.Center.X, this.pathList[0].Bounds.Center.Y), new Vector2(3, 3));
                Vector2 prev_point = Vector2.Subtract(new Vector2(this.pathList[0].Bounds.Center.X, this.pathList[0].Bounds.Center.Y), new Vector2(3, 3));

                float xPos = this.Bounds.Center.X;
                float yPos = this.Bounds.Center.Y;

                this.direction = getNormalizedVector(new Vector2(xPos, yPos), new Vector2(this.pathList[0].Bounds.Center.X, this.pathList[0].Bounds.Center.Y));

                if (xPos <= end_point.X && xPos >= prev_point.X
                    && yPos <= end_point.Y && yPos>= prev_point.Y)
                {

                    this.pathList.RemoveAt(0);
                    if (pathList.Count == 0)
                    {

                        this.isMoving = false;
                        this.collisionList.Clear();
                    }
                    return;
                }

                position += direction * moveSpeed;
            }
        }

        public Vector2 getNormalizedVector(Vector2 startVector, Vector2 endVector)
        {
            Vector2 moveVector = Vector2.Subtract(endVector, startVector);
            Vector2 normalizedVector = Vector2.Normalize(moveVector);
            return normalizedVector;
        }

        public void Update(GameTime gameTime)
        {
            if (this.isAlive())
            {
                // TODO: put code here for when mobile is alive
            }
        }

        public override void Animate()
        {
            switch (this.animateState)
            {
                case "move":
                    //  Animate
                    if (animateTime >= 10)
                    {
                        this.ssX = 1;
                    }
                    if (animateTime >= 20)
                    {
                        this.ssX = 0;
                    }
                    if (animateTime >= 30)
                    {
                        this.ssX = 2;
                    }
                    if (animateTime >= 40)
                    {
                        this.ssX = 0;
                        this.animateTime = 0;
                    }
                    break;
                case "attack":
                    //  Animate
                    if (animateTime >= 5)
                    {
                        this.ssX = 0;
                    }
                    if (animateTime >= 10)
                    {
                        this.ssX = 1;
                    }
                    if (animateTime >= 15)
                    {
                        this.ssX = 2;
                    }
                    if (animateTime >= 20)
                    {
                        this.ssX = 0;
                        this.animateTime = 0;
                        this.animateState = "move";
                    }
                    break;
                default:
                    break;
            }

            Vector2 getDirection = getNormalizedVector(this.position, this.destination);
            if (getDirection.Y >= 0)
            {
                isUp = true;
            }
            else
            {
                getDirection.Y *= -1;
                isUp = false;
            }
            if (getDirection.X >= 0)
            {
                isRight = true;
            }
            else
            {
                getDirection.X *= -1;
                isRight = false;
            }
            //  Then, set the appropriate direction of the sprite
            if (getDirection.Y >= getDirection.X)
            {
                if (isUp)
                {
                    this.ssY = 1;       //  TyNote: Not sure why up and down values are reversed, but it works
                    if (this.animateState == "attack")
                        this.ssY += 4;
                }
                else
                {
                    this.ssY = 0;
                    if (this.animateState == "attack")
                        this.ssY += 4;
                }
            }
            else
            {
                if (isRight)
                {
                    this.ssY = 3;
                    if (this.animateState == "attack")
                        this.ssY += 4;
                }
                else
                {
                    this.ssY = 2;
                    if (this.animateState == "attack")
                        this.ssY += 4;
                }
            }

            this.animateTime++;
        }
    }
}
