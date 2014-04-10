using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Retribution
{
    //  The ModelManager manages the player units and ai units. It can add units to the list
    class ModelManager
    {
        public static ModelManager instance;
        public List<GameObject> player;
        public List<GameObject> artificial;
        public List<GameObject> inMotion;
        private Vector2 autoPlace;
        public Map myMap;

        private ModelManager(ref Map newMap)
        {
            player = new List<GameObject>();
            artificial = new List<GameObject>();
            inMotion = new List<GameObject>();
            autoPlace = new Vector2(0, 0);
            myMap = newMap;
        }

        public static ModelManager getInstance(ref Map newMap){
            if (instance == null)
            {
                instance = new ModelManager(ref newMap);
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
                    case "WARRIOR":

                        Warrior war = new Warrior(autoPlace);
                        autoPlace.X += 50;
                        player.Add(war);
                        break;
                    case "CLERIC":
                        Cleric cler = new Cleric(autoPlace);
                        autoPlace.X += 50;
                        player.Add(cler);
                        break;
                }
            }
            else
            {
                //Console.WriteLine("creating artificial stuff");
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
                    case "WARRIOR":
                        Warrior war = new Warrior(autoPlace);
                        autoPlace.X += 50;
                        artificial.Add(war);
                        break;
                    case "CLERIC":
                        Cleric cler = new Cleric(autoPlace);
                        autoPlace.X += 50;
                        artificial.Add(cler);
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
                        player.Add(temp);
                        break;
                    case "TOWER":
                        Tower tower = new Tower(position);
                        player.Add(tower);
                        break;
                    case "WARRIOR":
                        Warrior war = new Warrior(position);
                        player.Add(war);
                        break;
                    case "DIGIT":
                        Digits digit = new Digits(position);
                        player.Add(digit);
                        break;
                    case "CLERIC":
                        Cleric cler = new Cleric(position);
                        player.Add(cler);
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
                    case "WARRIOR":
                        Warrior war = new Warrior(position);
                        autoPlace.X += 50;
                        artificial.Add(war);
                        break;
                    case "CLERIC":
                        Cleric cler = new Cleric(position);
                        artificial.Add(cler);
                        break;

                }
            }

        }
        
        //  Helper method to calculate normalized vector
        
        /*
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
         * */

        //  Return true if the list of tiles are the same
        /*
        public Boolean CompareLists(List<Tile> newList, List<Tile> oldList)
        {
            if (newList.Count == oldList.Count)
            {
                for (int i = 0; i < newList.Count; i++)
                {
                    if (newList[i] != oldList[i])
                        return false;
                }
                return true;
            }
            else
                return false;
        }
         * */
    }
}
