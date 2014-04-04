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

        private AIManager(ref Map newMap)
        {
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
          
                            ((Mobile)unit).setDestination(new Vector2(0, 0), new Vector2(randomX, randomY));
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