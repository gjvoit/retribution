using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Timers;
namespace Retribution
{
    class AIManager
    {
        private static AIManager instance;
        public Map myMap;
        public Random random;
        public List<GameObject> aiUnits;
        public static Timer moveTimer = new Timer(100);
        public static Timer imTimer = new Timer(10000);
        private AIManager(ref Map newMap)
        {
            random = new Random();
            myMap = newMap;
            moveTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            imTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent2);
        }
        public void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            moveTimer.Stop();
            //if (!imTimer.Enabled)
            //    moveTimer.Start();
        }
        public void OnTimedEvent2(object source, ElapsedEventArgs e)//after 3 seconds
        {
            imTimer.Stop();
            moveTimer.Start();//open the window to explore
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
        public void pursue(GameObject unit)
        {
            //Unit is the AI unit set to pursue
            List<GameObject> attackParty = new List<GameObject>();
            unit.selected = true;
            attackParty.Add(unit);
            int size = random.Next(1, aiUnits.Count);
            if (String.Compare(unit.type, "COMMANDER",true) == 0)
                size = aiUnits.Count;
            special(unit);
            for (int x = 0; x < size; x++) // "Count/3" was breaking the game once a user got the enemy units to less than 3
            {
                GameObject gunit = aiUnits[x];
                if (gunit.aiTarget == null && gunit.GetType().BaseType == typeof(Mobile))//if doesn't have target already and can move
                {
                    special(gunit);
                    gunit.aiTarget = unit.aiTarget;
                    gunit.selected = true;
                    attackParty.Add(gunit);//band with this guy
                }
            }
            MovementManager.changeDestination(attackParty, unit.aiTarget.position);
            //foreach (GameObject cunit in attackParty)
            //{
            //    if (cunit.GetType().BaseType == typeof(Mobile))
            //    {
            //        ((Mobile)cunit).setDestination( unit.aiTarget.position);
            //        List<Tile> newList = new List<Tile>();

            //        ((Mobile)cunit).pathList.Clear();
            //        ((Mobile)cunit).pathList.AddRange(myMap.GetPath(unit.position, ((Mobile)cunit).destination, newList));
            //        ((Mobile)cunit).isMoving = true;
            //        ((Mobile)cunit).isPaused=false;
            //    }
            //}
        }
        public void special(GameObject unit)
        {
            switch (unit.type)
            {
                case "ARCHER":
                    ((Archer)unit).rapidFire();
                    break;
                case "COMMANDER":
                    ((Commander)unit).rally();
                    break;
                case "ROGUE":
                    ((Rogue)unit).stealth();
                    break;
                case "WARRIOR":
                    ((Warrior)unit).juggernaut();
                    break;
                case "BOSS":
                    ((BossUnit)unit).pound();
                    break;
            }
        }
        public void explore(GameObject unit)
        {
 
            //if (random.NextDouble()*100<=4)
            //{
                List<GameObject> searchParty = new List<GameObject>();
                unit.selected = true;
                searchParty.Add(unit);
                int randpull = random.Next(0, aiUnits.Count);
                for (int x = 0; x < randpull; x++)
                {
                    GameObject gunit = aiUnits[x];

                    if (gunit.aiTarget == null && gunit.GetType().BaseType == typeof(Mobile))
                    {
                        searchParty.Add(gunit);
                        gunit.selected = true;
                    }
                }
                Vector2 explore = new Vector2(random.Next(0, 704), random.Next(600, 1024));
                MovementManager.changeDestination(searchParty, explore);
                //foreach (GameObject cunit in searchParty)
                //{
                   
                    //((Mobile)cunit).setDestination(MovementManager.getNormalizedVector(cunit.position,explore), explore);
                    //((Mobile)cunit).setDestination(explore);
                    //List<Tile> newList = new List<Tile>();

                    //((Mobile)cunit).pathList.Clear();
                    //((Mobile)cunit).pathList.AddRange(myMap.GetPath(cunit.position, ((Mobile)cunit).destination, newList));
                    //((Mobile)cunit).isMoving = true;

               // }
            //}
        }

        public void SetAIDestinations2(List<GameObject> aiunits)
        {
            
            this.aiUnits = aiunits;
            if (myMap.isDrawn == true)
            {
                foreach (GameObject unit in aiUnits)
                {
                    
                    if (unit.GetType().BaseType == typeof(Mobile))
                    {
                        if (unit.aiTarget != null)//if it has a target
                        {

                            pursue(unit);
                            if (!unit.aiTarget.alive)
                                unit.aiTarget = null;
                            //break;
                        }
                        else
                        if (!((Mobile)unit).isMoving||(((Mobile)unit).isMoving&&unit.aiTarget==null&&moveTimer.Enabled))
                        { //
                            explore(unit);
                              break;
                        }
                        
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
                            MovementManager.changeDestination(aiUnits,new Vector2(randomX,randomY));
                            //((Mobile)unit).setDestination(new Vector2(randomX, randomY));
                            //((Mobile)unit).pathList.Clear();
                            //((Mobile)unit).pathList.AddRange(myMap.GetPath(unit.position, ((Mobile)unit).destination, newList));
                            //((Mobile)unit).isMoving = true;
                        }
                    }
   
            }
            else
                return;
        }

    }
}