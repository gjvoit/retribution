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
        public Tile startTile;
        public Tile endTile;
        public Tile lowestScoreNode;
        public Tile currentNode;

        public Map(String fileName)
        {
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
                }
            }

            //pathList = new List<Tile>();
            openList = new List<Tile>();
            closedList = new List<Tile>();
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

        public List<Tile> GetPath(Vector2 startPoint, Vector2 endPoint, List<Tile> newClosedList)
        {
            openList.Clear();
            //pathList.Clear();
            closedList = newClosedList;

            //System.Console.WriteLine(closedList.Count);

            List<Tile> pathList = new List<Tile>();

            // Get starting and ending nodes:
            startTile = GetTile(startPoint);
            endTile = GetTile(endPoint);

            //System.Console.WriteLine("Start position: " + startTile.xPosition + ", " + startTile.yPosition);
           // System.Console.WriteLine("End position: " + endTile.xPosition + ", " + endTile.yPosition);

            if (endTile.isWalkable == false || closedList.Contains(endTile) == true)
            {
                return pathList;
            }

            currentNode = startTile;

            //openList.Add(currentNode);

            while(currentNode.origin != endTile.origin)
            {
                if (currentNode == endTile)
                {
                    openList.Clear();
                    closedList.Clear();

                    break;
                }

                int x = currentNode.xPosition;
                int y = currentNode.yPosition;

                //openList.Add(currentNode);

                // Check all adjacent tiles to see if open:
                CheckNode(x - 1, y, x, y);
                CheckNode(x, y - 1, x, y);
                CheckNode(x + 1, y, x, y);
                CheckNode(x, y + 1, x, y);
                CheckNode(x + 1, y + 1, x, y);
                CheckNode(x + 1, y - 1, x, y);
                CheckNode(x - 1, y + 1, x, y);
                CheckNode(x - 1, y - 1, x, y);

                openList.Remove(currentNode);

                if (openList.Count == 0)
                {
                    break;
                }

                // Find lowest score from openList:
                FindLowestScore();

                currentNode = lowestScoreNode;
                pathList.Add(currentNode);

                if (pathList.Count > 50)
                {
                    break;
                }


            }
            //System.Console.WriteLine(pathList.Count);
            //System.Console.WriteLine("Lowest node:" + pathList[0].xPosition + ", " + pathList[0].yPosition);
            return pathList;
        }


        public void FindLowestScore()
        {
            int movementCost = 0;  //  Magic number yayyy

            float lowestScore = 999999999;

           // System.Console.WriteLine(openList.Count);

            // For each open Tile, calculate heuristic and store minimum:
            foreach(Tile myTile in openList)
            {
                if (Math.Abs(myTile.xPosition - currentNode.xPosition) + Math.Abs(myTile.yPosition - currentNode.yPosition) >= 2)
                    movementCost = 14;

                else
                    movementCost = 10;

                Vector2 currentPos = new Vector2(myTile.Bounds.Center.X, myTile.Bounds.Center.Y);
                Vector2 destination = new Vector2(endTile.Bounds.Center.X, endTile.Bounds.Center.Y);

                float heuristic = movementCost + (Vector2.Distance(currentPos, destination));

                if (heuristic < lowestScore)
                {
                    lowestScore = heuristic;
                    lowestScoreNode = myTile;
                }

            }

            //System.Console.WriteLine(lowestScoreNode.xPosition + ", " + lowestScoreNode.yPosition + ", " + lowestScore);

        }


        public void CheckNode(int newX, int newY, int parentX, int parentY)
        {
            //  If node is out of bounds, return:
            if (newX < 0 || newY < 0 || newX >= width || newY >= height)
            {
                return;
            }
            //  Else, add Tile to openList if it is walkable and not closed:
            else
            {
                if (mapTiles[newY, newX].isWalkable == true && closedList.Contains(mapTiles[newY,newX]) == false)
                {
                    mapTiles[newY, newX].parentTile = mapTiles[parentY, parentX];
                    openList.Add(mapTiles[newY, newX]);
                }
            }
        }


        public Tile GetTile(Vector2 endPoint)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (endPoint.X >= mapTiles[y, x].origin.X && endPoint.X <= (mapTiles[y, x].origin.X + mapTiles[y, x].width)
                        && endPoint.Y >= mapTiles[y, x].origin.Y && endPoint.Y <= (mapTiles[y, x].origin.Y + mapTiles[y, x].height)
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
