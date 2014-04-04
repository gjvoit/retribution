using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Retribution
{
    class MovementManager
    {

        Map myMap;

        static MovementManager instance;
        private MovementManager(Map newMap)
        {
            myMap = newMap;
        }
        public MovementManager getInstance(Map newMap)
        {
            if (instance == null)
            {
                instance = new MovementManager(newMap);
                return instance;
            }
            else
                return instance;
        }

        //  Helper method to calculate normalized vector
        public static Vector2 getNormalizedVector(Vector2 startVector, Vector2 endVector)
        {
            Vector2 moveVector = Vector2.Subtract(endVector, startVector);
            Vector2 normalizedVector = Vector2.Normalize(moveVector);
            return normalizedVector;
        }

        //  Call movement method of all selected objects
        public void moveObjects(List<Mobile> mobiles, List<Tower> towers)
        {
            Boolean collision = false;
            List<Tile> newClosedList = new List<Tile>();

            for (int i = 0; i < mobiles.Count; i++)
            {
                if (mobiles[i].isMoving == true)
                {
                    
                    for (int j = 0; j < mobiles.Count; j++)
                    {
                        if (mobiles[i].collidesWith(mobiles[j]) && i != j)
                        {
                            newClosedList.Add(myMap.GetContainingTile(mobiles[j]));
                        }
                    }

                    //System.Console.WriteLine(mobiles[i].collisionList.Count);
                    //System.Console.WriteLine(newClosedList.Count);

                    if (CompareLists(newClosedList, mobiles[i].collisionList) == false)
                    {
                        //System.Console.WriteLine("Test");
                        //mobiles[i].collisionList = newClosedList;
                        mobiles[i].collisionList.Clear();
                        mobiles[i].collisionList.AddRange(newClosedList);
                        //System.Console.WriteLine(mobiles[i].collisionList.Count);
                        //System.Console.WriteLine(newClosedList.Count);
                        if (mobiles[i].collisionList.Contains(myMap.GetTile(mobiles[i].destination)))
                        {
                            mobiles[i].isMoving = false;
                        }

                        mobiles[i].pathList.Clear();
                        mobiles[i].pathList.AddRange(myMap.GetPath(mobiles[i].position, mobiles[i].destination, newClosedList));
                    }

                }

                mobiles[i].move();
                newClosedList.Clear();
            }
        }

        public Boolean CompareLists(List<Tile> newList, List<Tile> oldList)
        {
           
            if (newList.Count == oldList.Count)
            {
                for (int i = 0; i < newList.Count; i++)
                {
                    if (newList[i] != oldList[i])
                    {

                        return false;
                    }
                }
                
                return true;
            }
            else
            {
                return false;
            }
        }

        /* Might not need this method...
        public void CheckPauses(List<Mobile> mobiles)
        {
            Boolean check = false;
            for (int i = 0; i < mobiles.Count; i++)
            {
                if (mobiles[i].isPaused == true)
                {
                    for (int j = 0; j < mobiles.Count; j++)
                    {
                        if (i != j)
                        {
                            if (mobiles[i].collidesWith(mobiles[j]) && mobiles[j].isMoving == true && mobiles[j].isPaused == false)
                            {
                                check = true;
                            }
                        }
                    }

                    if (check == false)
                    {
                        mobiles[i].isPaused = false;
                    }
                }
            }
        }*/

        public void changeDestination(List<Mobile> listOfSelectedObjects, Vector2 destination)
        {
            
            for (int i = 0; i < listOfSelectedObjects.Count; i++)
            {
                if (listOfSelectedObjects[i].selected == true)
                {
                    listOfSelectedObjects[i].isMoving = false;
                    listOfSelectedObjects[i].setDestination(getNormalizedVector(listOfSelectedObjects[i].getPosition(), destination), destination);
                    List<Tile> newClosedList = new List<Tile>();
                    //System.Console.WriteLine(listOfSelectedObjects[i].destination.X + ", " + listOfSelectedObjects[i].destination.Y);
                    Vector2 startPoint = new Vector2(listOfSelectedObjects[i].Bounds.Center.X, listOfSelectedObjects[i].Bounds.Center.Y);

                    listOfSelectedObjects[i].pathList = myMap.GetPath(startPoint, listOfSelectedObjects[i].destination, newClosedList);
                    listOfSelectedObjects[i].isMoving = true;
                }
            }
        }
    }
}
