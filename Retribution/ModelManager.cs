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
        private ModelManager()
        {
            player = new List<GameObject>();
            artificial = new List<GameObject>();
            inMotion = new List<GameObject>();
            autoPlace = new Vector2(0, 0);
            //  Do Nothing
        }

        public static ModelManager getInstance(){
            if (instance == null)
            {
                instance = new ModelManager();
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
                 switch(type){
                    case "ARCHER":
                        Archer temp = new Archer(autoPlace);
                        autoPlace.X+=32;
                        player.Add(temp);
                        break;
                    case "TOWER":
                        Tower tower = new Tower(autoPlace);
                        autoPlace.X+=32;
                        player.Add(tower);
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
                        player.Add(temp);
                        break;
                    case "TOWER":
                        Tower tower = new Tower(position);
                        autoPlace.X += 32;
                        player.Add(tower);
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

        //  Call movement method of all selected objects
        public static void moveObjects(List<GameObject> listOfSelectedObjects)
        {
            for (int i = 0; i < listOfSelectedObjects.Count; i++)
            {
                if (listOfSelectedObjects[i].selected == true)
                {
                    listOfSelectedObjects[i].move();
                }
            }
        }

        public static void changeDestination(List<GameObject> listOfSelectedObjects, Vector2 destination)
        {
            for (int i = 0; i < listOfSelectedObjects.Count; i++)
            {
                if (listOfSelectedObjects[i].selected == true)
                {
                    listOfSelectedObjects[i].setDestination(getNormalizedVector(listOfSelectedObjects[i].getPosition(), destination), destination);
                }
            }
        }
    }
}
