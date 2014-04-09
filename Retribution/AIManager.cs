using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Retribution
{
    class AIManager
    {
        private static AIManager instance;
        public Map myMap;
        public Random random;

        private AIManager(ref Map newMap)
        {
            random = new Random();
            myMap = newMap;
        }
        public static AIManager getInstance(ref Map newMap){
            if (instance == null)
            {
                instance = new AIManager(ref newMap);
                return instance;
            }
            else
                return instance;
        }
        public void pursue(List<GameObject> aiUnits, GameObject unit)
        {
            List<GameObject> attackParty = new List<GameObject>();
            attackParty.Add(unit);
            for (int x = 0; x < random.Next(1, aiUnits.Count); x++)
            {
                GameObject gunit = aiUnits[x];
                if (gunit.aiTarget == null && gunit.GetType().BaseType == typeof(Mobile))
                attackParty.Add(gunit);
            }
            foreach (GameObject cunit in attackParty)
            {
                if (cunit.GetType().BaseType == typeof(Mobile) && unit.aiTarget != null)
                {
                    ((Mobile)cunit).setDestination( unit.aiTarget.position);
                    List<Tile> newList = new List<Tile>();

                    ((Mobile)cunit).pathList.Clear();
                    ((Mobile)cunit).pathList.AddRange(myMap.GetPath(unit.position, ((Mobile)cunit).destination, newList));
                    ((Mobile)cunit).isMoving = true;
                }
            }
        }

        public void explore(List<GameObject> aiUnits, GameObject unit)
        {
 
            if (random.NextDouble()*100==24)
            {
                List<GameObject> searchParty = new List<GameObject>();
                searchParty.Add(unit);
                int randpull = random.Next(0, aiUnits.Count - 2);
                for (int x = randpull; x < randpull+2; x++)
                {
                GameObject gunit = aiUnits[x];
                if (gunit.aiTarget == null && gunit.GetType().BaseType == typeof(Mobile))
                     searchParty.Add(gunit);

                }
                foreach (GameObject cunit in searchParty)
                {
                    Vector2 explore = new Vector2(random.Next(180,248),random.Next(400,672));
                    //((Mobile)cunit).setDestination(MovementManager.getNormalizedVector(cunit.position,explore), explore);
                    ((Mobile)cunit).setDestination(explore);
                    List<Tile> newList = new List<Tile>();

                    ((Mobile)cunit).pathList.Clear();
                    ((Mobile)cunit).pathList.AddRange(myMap.GetPath(cunit.position, ((Mobile)cunit).destination, newList));
                    ((Mobile)cunit).isMoving = true;

                }
            }
        }

        public void SetAIDestinations2(List<GameObject> aiUnits)
        {
            if (myMap.isDrawn == true)
            {
                foreach (GameObject unit in aiUnits)
                {
      
                    if(String.Compare(unit.type, "ARCHER", true) == 0)
                    {
                        if (unit.aiTarget != null)
                            pursue(aiUnits, unit);
                        else
                            explore(aiUnits, unit);
                    }

                    if (String.Compare(unit.type, "WARRIOR", true) == 0&& unit.aiTarget!=null)
                    {
                        if (unit.aiTarget != null)
                        {
                            if (String.Compare(unit.aiTarget.type, "ARCHER", true) == 0)
                                pursue(aiUnits, unit);
                            else
                                if (random.Next(0, 15) == 7)
                                    pursue(aiUnits, unit);
                        }
                        else
                            explore(aiUnits, unit);
                    }                
                }

            }
            else
                return;
        }
        public void SetAIDestinations(List<GameObject> aiUnits)
        {
            if (myMap.isDrawn == true)
            {
                    foreach (GameObject unit in aiUnits)
                    {
                        List<Tile> newList = new List<Tile>();
                        if (unit.GetType().BaseType == typeof(Mobile) && ((Mobile)unit).isMoving == false)
                        {
                            //System.Console.WriteLine("Test");
                            Random random = new Random();
                            int randomX = random.Next(10, 700);
                            int randomY = random.Next(10, 350);
          
                            ((Mobile)unit).setDestination(new Vector2(randomX, randomY));
                            ((Mobile)unit).pathList.Clear();
                            ((Mobile)unit).pathList.AddRange(myMap.GetPath(unit.position, ((Mobile)unit).destination, newList));
                            ((Mobile)unit).isMoving = true;
                        }
                    }
   
            }
            else
                return;
        }

    }
}