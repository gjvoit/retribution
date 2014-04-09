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
        public void moveObjects(List<GameObject> playerUnits, List<GameObject> aiUnits)
        {
            List<GameObject> mobiles = new List<GameObject>();
            mobiles.AddRange(playerUnits);
            mobiles.AddRange(aiUnits);

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
                                //if (mobiles[i].GetType().BaseType == typeof(Mobile) && ((Mobile)mobiles[i]).isMoving == true)
                                //{
                                //}
                                //else
                                    newClosedList.Add(myMap.GetContainingTile(mobiles[j]));
                            }
                        }

                        if (CompareLists(newClosedList, ((Mobile)mobiles[i]).collisionList) == false)
                        {

                            ((Mobile)mobiles[i]).collisionList.Clear();
                            ((Mobile)mobiles[i]).collisionList.AddRange(newClosedList);

                            //System.Console.WriteLine("test");

                            ((Mobile)mobiles[i]).pathList.Clear();
                            Vector2 startPoint = new Vector2(mobiles[i].Bounds.Center.X, mobiles[i].Bounds.Center.Y);
                            ((Mobile)mobiles[i]).pathList.AddRange(myMap.GetPath(startPoint, ((Mobile)mobiles[i]).destination, newClosedList));
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
                    ((Mobile)(listOfSelectedObjects[i])).setDestination(destination);
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
    }
}
