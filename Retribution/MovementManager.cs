using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Retribution
{
    class MovementManager
    {

        public static Map myMap;

        static MovementManager instance;
        private MovementManager()
        {
            myMap = null;
        }
        public static MovementManager getInstance()
        {
            if (instance == null)
            {
                instance = new MovementManager();
                return instance;
            }
            else
                return instance;
        }
        public void setMap(Map newMap){
            myMap = newMap;
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
                if (mobiles[i].collisionList.Contains(myMap.GetTile(mobiles[i].destination)))
                {
                    System.Console.WriteLine("test");
                    mobiles[i].isMoving = false;
                }

                if (mobiles[i].isMoving == true)
                {
                    
                    for (int j = 0; j < mobiles.Count; j++)
                    {
                        if (mobiles[i].collidesWith(mobiles[j]) && i != j)
                        {
                            newClosedList.Add(myMap.GetContainingTile(mobiles[j]));
                        }
                    }


                    if (CompareLists(newClosedList, mobiles[i].collisionList) == false)
                    {
                        //System.Console.WriteLine("Test");
                        //mobiles[i].collisionList = newClosedList;
                        mobiles[i].collisionList.Clear();
                        mobiles[i].collisionList.AddRange(newClosedList);

                        mobiles[i].pathList.Clear();
                        mobiles[i].pathList.AddRange(myMap.GetPath(mobiles[i].position, mobiles[i].destination, newClosedList));
                    }

                }

                mobiles[i].move();
                newClosedList.Clear();
            }
        }
       
        public void moveObjects(List<GameObject> playerUnits, List<GameObject> aiUnits)
        {
            List<GameObject> mobiles = new List<GameObject>();
            mobiles.AddRange(playerUnits);
            mobiles.AddRange(aiUnits);

            /*
            for (int i = 0; i < listOfSelectedObjects.Count; i++)
            {
                if (listOfSelectedObjects[i].selected == true ||
                    (listOfSelectedObjects[i].GetType().BaseType == typeof(Mobile) && ((Mobile)(listOfSelectedObjects[i])).isMoving == true)
                    || listOfSelectedObjects[i].GetType().BaseType == typeof(Projectile) && ((Projectile)(listOfSelectedObjects[i])).isMoving == true)
                {
                    if (listOfSelectedObjects[i].GetType().BaseType == typeof(Mobile))
                    {
                        ((Mobile)(listOfSelectedObjects[i])).move();
                    }
                    if (listOfSelectedObjects[i].GetType().BaseType == typeof(Projectile))
                    {
                        ((Projectile)(listOfSelectedObjects[i])).move();
                    }
                }
            }*/

            List<Tile> newClosedList = new List<Tile>();

            for (int i = 0; i < mobiles.Count; i++)
            {
                if (mobiles[i].GetType().BaseType == typeof(Mobile))
                {
                    if (((Mobile)mobiles[i]).isMoving == true)
                    {

                        if (((Mobile)mobiles[i]).collisionList.Contains(myMap.GetTile(((Mobile)mobiles[i]).destination)))
                        {
                            ((Mobile)mobiles[i]).isMoving = false;
                        }

                        for (int j = 0; j < mobiles.Count; j++)
                        {
                            if (mobiles[i].collidesWith(mobiles[j]) && i != j)
                            {
                                newClosedList.Add(myMap.GetContainingTile(mobiles[j]));
                            }
                        }

                        if (CompareLists(newClosedList, ((Mobile)mobiles[i]).collisionList) == false)
                        {

                            ((Mobile)mobiles[i]).collisionList.Clear();
                            ((Mobile)mobiles[i]).collisionList.AddRange(newClosedList);

                            //if (((Mobile)mobiles[i]).collisionList.Contains(myMap.GetDestinationTile(((Mobile)mobiles[i]).destination)))
                            // {
                            //   ((Mobile)mobiles[i]).isMoving = false;
                            //}

                            ((Mobile)mobiles[i]).pathList.Clear();
                            ((Mobile)mobiles[i]).pathList.AddRange(myMap.GetPath(mobiles[i].position, ((Mobile)mobiles[i]).destination, newClosedList));
                        }

                        ((Mobile)mobiles[i]).move();
                    }


                    newClosedList.Clear();
                }
            }
        }

        public static void changeDestination(List<GameObject> listOfSelectedObjects, Vector2 destination)
        {


            for (int i = 0; i < listOfSelectedObjects.Count; i++)
            {
                if (listOfSelectedObjects[i].GetType().BaseType == typeof(Projectile))
                {
                    ((Projectile)(listOfSelectedObjects[i])).setDestination(
                        getNormalizedVector(listOfSelectedObjects[i].getPosition(),
                        ((Projectile)(listOfSelectedObjects[i])).target.getPosition()),
                        ((Projectile)(listOfSelectedObjects[i])).target.getPosition());
                }
                if (listOfSelectedObjects[i].selected == true && listOfSelectedObjects[i].GetType().BaseType == typeof(Mobile))
                {
                    ((Mobile)(listOfSelectedObjects[i])).isMoving = false;
                    ((Mobile)(listOfSelectedObjects[i])).setDestination(getNormalizedVector(listOfSelectedObjects[i].getPosition(), destination), destination);
                    List<Tile> newClosedList = new List<Tile>();
                    //System.Console.WriteLine(listOfSelectedObjects[i].destination.X + ", " + listOfSelectedObjects[i].destination.Y);
                    Vector2 startPoint = new Vector2(listOfSelectedObjects[i].Bounds.Center.X, listOfSelectedObjects[i].Bounds.Center.Y);

                    ((Mobile)(listOfSelectedObjects[i])).pathList = myMap.GetPath(startPoint, ((Mobile)(listOfSelectedObjects[i])).destination, newClosedList);
                    ((Mobile)(listOfSelectedObjects[i])).isMoving = true;
                }
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
