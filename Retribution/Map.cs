#region Using Statements
using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace Retribution
{

    public class Map
    {
        public Tile[,] mapTiles;
        public int height;
        public int width;
        public Boolean isDrawn;
        // public int xoffset = 0; For use with FullScreen
        // public int yoffset = 0; For use with FullScreen

        //public List<Tile> pathList;
        public List<Tile> openList;
        public List<Tile> closedList;
        public List<Tile> collisionList;
        public Tile startTile;
        public Tile endTile;
        public Tile lowestScoreTile;
        public Tile currentTile;

        public Map(String fileName)
        {
            openList = new List<Tile>();
            closedList = new List<Tile>();
            isDrawn = false;
            string[] tiles = File.ReadAllLines(fileName);
            width = tiles[0].Length;
            height = tiles.Length;
            mapTiles = new Tile[height, width];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    mapTiles[y, x] = new Tile(tiles[y][x]);
                    mapTiles[y, x].xPosition = x;
                    mapTiles[y, x].yPosition = y;
                    mapTiles[y, x].fScore = 0;
                }
            }

            //pathList = new List<Tile>();
            
        }

        public void DrawMap(SpriteBatch spriteBatch)
        {
            isDrawn = true;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Texture2D myTexture = mapTiles[y, x].texture;
                    Vector2 origin = new Vector2(x * myTexture.Width, y * myTexture.Height);
                    mapTiles[y, x].SetOrigin(origin);
                    spriteBatch.Draw(myTexture, origin, Color.White);
                }
            }
        }

        //////////////////////////////////////////////////
        // PATHFINDING://////////////////////////////////
        /////////////////////////////////////////////////

        public List<Tile> GetPath(Vector2 startPoint, Vector2 endPoint, List<Tile> newCollisionList)
        {
            // Set lists:
            openList.Clear();
            closedList.Clear();
            collisionList = newCollisionList;
            List<Tile> pathList = new List<Tile>();

            // Get starting and ending nodes:
            startTile = GetTile(startPoint);
            endTile = GetTile(endPoint);

            if (endTile == null|| endTile.isWalkable == false || closedList.Contains(endTile) == true)
            {
                return pathList;
            }

            openList.Add(startTile);
            int i = 0;
            while(closedList.Contains(endTile)==false && openList.Count != 0)
            {
                //System.Console.WriteLine(i++);

                currentTile = FindLowestScore();
                if (currentTile == endTile)
                {
                    break;
                }

                openList.Remove(currentTile);
                closedList.Add(currentTile);

                int x = currentTile.xPosition;
                int y = currentTile.yPosition;

                // Check all adjacent tiles to see if open:
                CheckNode(x - 1, y, x, y);
                CheckNode(x, y - 1, x, y);
                CheckNode(x + 1, y, x, y);
                CheckNode(x, y + 1, x, y);
                CheckNode(x + 1, y + 1, x, y);
                CheckNode(x + 1, y - 1, x, y);
                CheckNode(x - 1, y + 1, x, y);
                CheckNode(x - 1, y - 1, x, y);

            }

            while (currentTile.parentTile != null)
            {
                //System.Console.WriteLine(currentTile.xPosition + ", " + currentTile.yPosition);
                pathList.Add(currentTile);
                Tile temp = currentTile.parentTile;
                currentTile.parentTile = null;
                currentTile = temp;

                if(pathList.Count > 50)
                {
                    System.Console.WriteLine("break");
                    break;
                }
            }

            pathList.Reverse();
            return pathList;
        }


        public Tile FindLowestScore()
        {
            int lowestScore = 9999999; //  Magic number yayyy
            int index = 0;
            if(openList !=null)
           // lowestScore = openList[0].fScore;
            // Get tile in open list with the lowest fscore:
            foreach (Tile myTile in openList)
            {
                if (myTile.fScore < lowestScore && myTile.fScore > 0)
                {
                    lowestScore = myTile.fScore;
                    index = openList.IndexOf(myTile);
                }
            }
            return openList[index];
        }


        public void CheckNode(int newX, int newY, int parentX, int parentY)
        {
            //  If node is out of bounds, return:
            if (newX < 0 || newY < 0 || newX >= width || newY >= height)
            {
                return;
            }
            
            else
            {
                // Else if tile is walkable and not on closed or collision list:
                if (mapTiles[newY, newX].isWalkable == true 
                    && closedList.Contains(mapTiles[newY,newX]) == false
                    && collisionList.Contains(mapTiles[newY, newX]) == false
                    )
                {
                    int movementCost = 10;
                    if((Math.Abs(newX - parentX) + Math.Abs(newY - parentY)) >= 2)
                    {
                        movementCost += 4;
                    }

                    if (openList.Contains(mapTiles[newY, newX]) == false)
                    {
                        openList.Add(mapTiles[newY, newX]);
                        mapTiles[newY, newX].parentTile = currentTile;
                        mapTiles[newY, newX].gScore = currentTile.gScore + movementCost;
                        int heuristic = (int)Vector2.Distance(new Vector2(mapTiles[newY, newX].Bounds.Center.X, mapTiles[newY, newX].Bounds.Center.Y), new Vector2(endTile.Bounds.Center.X, endTile.Bounds.Center.Y));
                        mapTiles[newY, newX].hScore = heuristic;
                        mapTiles[newY, newX].fScore = mapTiles[newY, newX].gScore + mapTiles[newY, newX].hScore;
                    }
                    else if((currentTile.gScore + movementCost) < mapTiles[newY, newX].gScore)
                    {
                        mapTiles[newY, newX].parentTile = currentTile;
                        mapTiles[newY, newX].gScore = currentTile.gScore + movementCost;
                        mapTiles[newY, newX].fScore = mapTiles[newY, newX].gScore + mapTiles[newY, newX].hScore;
                    }

                }
            }
        }


        public Tile GetTile(Vector2 endPoint)
        {
            for (int y = 0; y < height; y++)//for each y tile
            {
                for (int x = 0; x < width; x++)//for each x tile
                {
                    if (endPoint.X >= mapTiles[y, x].origin.X //left side check
                        && endPoint.X <= (mapTiles[y, x].origin.X + mapTiles[y, x].width)//right side check
                        && endPoint.Y >= mapTiles[y, x].origin.Y && //inside top check
                        endPoint.Y <= (mapTiles[y, x].origin.Y + mapTiles[y, x].height)//inside bottom
                        )
                    {
                        //System.Console.WriteLine(mapTiles[x, y].xPosition + ", " + mapTiles[x, y].yPosition);
                        return mapTiles[y, x];
                    }
                }
            }

            return null;
        }



        public Tile GetContainingTile(GameObject myObject)
        {
            float xPosition = myObject.Bounds.Center.X;
            float yPosition = myObject.Bounds.Center.Y;
               
            //List<Tile> intersectingTiles = new List<Tile>();     

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {

                    if(xPosition >= mapTiles[y,x].origin.X && xPosition <= mapTiles[y,x].origin.X + mapTiles[y,x].width
                       && yPosition >= mapTiles[y,x].origin.Y && yPosition <= mapTiles[y,x].origin.Y + mapTiles[y,x].width
                       )
                    {
                        return mapTiles[y, x];
                    }

                }
            }

            return null;
            
        }
    }

}
