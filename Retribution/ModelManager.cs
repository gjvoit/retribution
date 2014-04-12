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
        public static List<GameObject> player;
        public static List<GameObject> artificial;
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
        public static List<GameObject> getArtificial()
        {
            if (artificial != null)
                return artificial;
            else
                artificial = new List<GameObject>();
            return artificial;
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
                    case "APPRENTICE":
                        Apprentice app = new Apprentice(autoPlace);
                        autoPlace.X += 32;
                        player.Add(app);
                        break;
                    case "ARCHER":
                        Archer temp = new Archer(autoPlace);
                        autoPlace.X+=50;
                        player.Add(temp);
                        break;
                    case "CATAPULT":
                        Catapult cat = new Catapult(autoPlace);
                        autoPlace.X += 32;
                        player.Add(cat);
                        break;
                    case "COMMANDER":
                        Commander com = new Commander(autoPlace);
                        autoPlace.X += 32;
                        player.Add(com);
                        break;
                    case "ROGUE":
                        Rogue rog = new Rogue(autoPlace);
                        autoPlace.X += 32;
                        player.Add(rog);
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
                    case "PAWN":
                        Pawn paw = new Pawn(autoPlace);
                        autoPlace.X += 50;
                        player.Add(paw);
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
                    case "APPRENTICE":
                        Apprentice app = new Apprentice(autoPlace);
                        autoPlace.X += 32;
                        artificial.Add(app);
                        break;
                    case "ARCHER":
                        Archer temp = new Archer(autoPlace);
                        autoPlace.X+=32;
                        artificial.Add(temp);
                        break;
                    case "CATAPULT":
                        Catapult cat = new Catapult(autoPlace);
                        autoPlace.X += 32;
                        artificial.Add(cat);
                        break;
                    case "COMMANDER":
                        Commander com = new Commander(autoPlace);
                        autoPlace.X += 32;
                        artificial.Add(com);
                        break;

                    case "PAWN":
                        Pawn paw = new Pawn(autoPlace);
                        autoPlace.X += 50;
                        artificial.Add(paw);
                        break;
                    case "ROGUE":
                        Rogue rog = new Rogue(autoPlace);
                        autoPlace.X += 32;
                        artificial.Add(rog);
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
                    case "APPRENTICE":
                        Apprentice app = new Apprentice(position);
                        player.Add(app);
                        break;
                    case "ARCHER":
                        Archer temp = new Archer(position);
                        player.Add(temp);
                        break;
                    case "CATAPULT":
                        Catapult cat = new Catapult(position);
                        player.Add(cat);
                        break;
                    case "COMMANDER":
                        Commander com = new Commander(position);
                        player.Add(com);
                        break;
                    case "PAWN":
                        Pawn paw = new Pawn(position);
                        player.Add(paw);
                        break;
                    case "ROGUE":
                        Rogue rog = new Rogue(position);
                        player.Add(rog);
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
                    case "APPRENTICE":
                        Apprentice app = new Apprentice(position);
                        artificial.Add(app);
                        break;
                    case "ARCHER":
                        Archer temp = new Archer(position);
                        artificial.Add(temp);
                        break;
                    case "CATAPULT":
                        Catapult cat = new Catapult(position);
                        artificial.Add(cat);
                        break;
                    case "COMMANDER":
                        Commander com = new Commander(autoPlace);
                        artificial.Add(com);
                        break;
                    case "PAWN":
                        Pawn paw = new Pawn(position);
                        artificial.Add(paw);
                        break;
                    case "ROGUE":
                        Rogue rog = new Rogue(position);
                        artificial.Add(rog);
                        break;
                    case "TOWER":
                        Tower tower = new Tower(position);
                        artificial.Add(tower);
                        break;
                    case "WARRIOR":
                        Warrior war = new Warrior(position);
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
