using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Retribution
{
    class ModelManager
    {
        public static ModelManager instance;
        public List<GameObject> player;
        public List<GameObject> artificial;
        public List<GameObject> inMotion;
        private Vector2 autoPlace;
        public Map myMap;

        private ModelManager(Map newMap)
        {
            player = new List<GameObject>();
            artificial = new List<GameObject>();
            inMotion = new List<GameObject>();
            autoPlace = new Vector2(0, 0);
            myMap = newMap;
        }

        public static ModelManager getInstance(Map newMap){
            if (instance == null)
            {
                instance = new ModelManager(newMap);
                return instance;
            }
            else
                return instance;
        }
        public void addUnit(String team, String type)
        {
            if (team == "PLAYER")
            {
                switch(type){
                    case "ARCHER":
                        Archer temp = new Archer(autoPlace);
                        autoPlace.X+=50;
                        player.Add(temp);
                        break;
                    case "TOWER":
                        Tower tower = new Tower(autoPlace);
                        autoPlace.X+=50;
                        player.Add(tower);
                        break;

                }
            }
            else
            {
                Console.WriteLine("creating artificial stuff");
                 switch(type){
                    case "ARCHER":
                        Archer temp = new Archer(autoPlace);
                        autoPlace.X+=32;
                        artificial.Add(temp);
                        break;
                    case "TOWER":
                        Tower tower = new Tower(autoPlace);
                        autoPlace.X+=32;
                        artificial.Add(tower);
                        break;

                }
            }

        }
        public void addUnit(String team, String type, Vector2 position)
        {
            if (team == "PLAYER")
            {
                switch (type)
                {
                    case "ARCHER":
                        Archer temp = new Archer(position);
                        autoPlace.X += 50;
                        player.Add(temp);
                        break;
                    case "TOWER":
                        Tower tower = new Tower(position);
                        autoPlace.X += 50;
                        player.Add(tower);
                        break;

                }
            }
            else
            {
                switch (type)
                {
                    case "ARCHER":
                        Archer temp = new Archer(position);
                        autoPlace.X += 32;
                        artificial.Add(temp);
                        break;
                    case "TOWER":
                        Tower tower = new Tower(position);
                        autoPlace.X += 32;
                        artificial.Add(tower);
                        break;

                }
            }

        }
        
        //  Helper method to calculate normalized vector
        public static Vector2 getNormalizedVector(Vector2 startVector, Vector2 endVector)
        {
            Vector2 moveVector = Vector2.Subtract(endVector, startVector);
            Vector2 normalizedVector = Vector2.Normalize(moveVector);
            return normalizedVector;
        }

        public void moveProj(ref List<Projectile> proj)
        {
            for (int i = 0; i < proj.Count; i++)
            {
                if (!proj[i].isAlive())
                    proj.Remove(proj[i]);
                else
                {
                    proj[i].move();
                }
            }
        }

        //  Call movement method of all selected objects
        public void moveObjects(List<GameObject> mobiles)
        {
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

                        for (int j = 0; j < mobiles.Count; j++)
                        {
                            if (mobiles[i].collidesWith(mobiles[j]) && i != j)
                            {
                                newClosedList.Add(myMap.GetContainingTile(mobiles[j]));
                            }
                        }

                        //System.Console.WriteLine(mobiles[i].collisionList.Count);
                        //System.Console.WriteLine(newClosedList.Count);

                        if (CompareLists(newClosedList, ((Mobile)mobiles[i]).collisionList) == false)
                        {
                            //System.Console.WriteLine("Test");
                            //mobiles[i].collisionList = newClosedList;
                            ((Mobile)mobiles[i]).collisionList.Clear();
                            ((Mobile)mobiles[i]).collisionList.AddRange(newClosedList);
                            //System.Console.WriteLine(mobiles[i].collisionList.Count);
                            //System.Console.WriteLine(newClosedList.Count);
                            if (((Mobile)mobiles[i]).collisionList.Contains(myMap.GetDestinationTile(((Mobile)mobiles[i]).destination)))
                            {
                                ((Mobile)mobiles[i]).isMoving = false;
                            }

                            ((Mobile)mobiles[i]).pathList.Clear();
                            ((Mobile)mobiles[i]).pathList.AddRange(myMap.GetPath(mobiles[i].position, ((Mobile)mobiles[i]).destination, newClosedList));
                        }

                    }

                    ((Mobile)mobiles[i]).move();
                    newClosedList.Clear();
                }
            }
        }

        public void changeDestination(List<GameObject> listOfSelectedObjects, Vector2 destination)
        {
            /*
            for (int i = 0; i < listOfSelectedObjects.Count; i++)
            {
                if (listOfSelectedObjects[i].GetType().BaseType == typeof(Projectile))
                {
                    ((Projectile)(listOfSelectedObjects[i])).setDestination(
                        getNormalizedVector(listOfSelectedObjects[i].getPosition(),
                        ((Projectile)(listOfSelectedObjects[i])).target.getPosition()), 
                        ((Projectile)(listOfSelectedObjects[i])).target.getPosition());
                }
                if (listOfSelectedObjects[i].selected == true)
                {
                    if (listOfSelectedObjects[i].GetType().BaseType == typeof(Mobile))
                    {
                        ((Mobile)(listOfSelectedObjects[i])).setDestination(
                            getNormalizedVector(listOfSelectedObjects[i].getPosition(), destination), destination);
                        ((Mobile)(listOfSelectedObjects[i])).isMoving = true;
                    }
                }
            }*/

            for (int i = 0; i < listOfSelectedObjects.Count; i++)
            {
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
    }
}
