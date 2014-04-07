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
        public bool collision;
        public List<Tile> collisionList;

        public Mobile(int health, Vector2 position, int damage, int attackRange)
            : base(health, position, damage, attackRange)
        {
            this.isMoving = false;
            this.isPaused = false;
            pathList = new List<Tile>();
            collisionList = new List<Tile>();
        }
       public void setDestination(Vector2 destination)
        {
            //this.direction = direction;
            this.destination = destination;
        }

        public void move()
        {

            if (this.pathList == null || this.pathList.Count == 0)
            {
                this.isMoving = false;
                return;
            }

            else
            {
                //if (this.collisionList.Contains(this.pathList[0]))
                //{
                //    this.isMoving = false;
                //    return;
                //}


                Vector2 end_point = Vector2.Add(new Vector2(this.pathList[0].Bounds.Center.X, this.pathList[0].Bounds.Center.Y), new Vector2(2, 2));
                Vector2 prev_point = Vector2.Subtract(new Vector2(this.pathList[0].Bounds.Center.X, this.pathList[0].Bounds.Center.Y), new Vector2(2, 2));

                float xPos = this.Bounds.Center.X;
                float yPos = this.Bounds.Center.Y;

                this.direction = getNormalizedVector(new Vector2(xPos, yPos), new Vector2(this.pathList[0].Bounds.Center.X, this.pathList[0].Bounds.Center.Y));
                //this.direction = getNormalizedVector(this.position, this.destination);

                if (xPos <= end_point.X && xPos >= prev_point.X
                    && yPos <= end_point.Y && yPos>= prev_point.Y)
                {
                    this.pathList.RemoveAt(0);
                    if (pathList.Count == 0)
                    {
                        //System.Console.WriteLine("test");

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

    }
}
