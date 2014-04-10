using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Retribution
{
    class ScreenManager
    {
        public List<Map> allMaps = new List<Map>();
        public List<Selector> allSelectors = new List<Selector>();
        // Keep track of 3 maps because of the 3 possible states (win/lose/playing)
        public Map prevMap;
        public Map currentMap;
        public Map nextMap;
        // victory can be "victory", "defeat" or "undef"; undef is used while a game is being played or
        // while we are still in a non-playable level (mainScreen, levelSelect, etc.)
        public String victory;
        // progressIndex is used to help confirm selector booleans
        public int progressIndex;
        static ScreenManager instance;
        
        // Need constructor, should it emulate the other managers?
        // startPrev will be defeatScreen; startCurr will be mainScreen; startNext will be levelSelect;
       
        private ScreenManager()
        {
            prevMap = null;
            currentMap = null;
            nextMap = null;
            victory = "undef";
            // progressIndex is pointing to mainScreen currently
            progressIndex = 0;
        }

        public void chooseSelector(Mobile levelChooser)
        {
            switch (currentMap.name)
            {
                case "Content/MainScreen.txt":
                    allSelectors[1].isColliding(levelChooser);
                    break;
                case "Content/levelSelect.txt":
                    if (allSelectors[2].getInteraction() == true)
                    {
                        allSelectors[2].isColliding(levelChooser);
                    }
                    else if (allSelectors[3].getInteraction() == true)
                    {
                        allSelectors[3].isColliding(levelChooser);
                    }
                    else if (allSelectors[4].getInteraction() == true)
                    {
                        allSelectors[4].isColliding(levelChooser);
                    }
                    break;
                case "Content/defeatScreen.txt":
                    if (allSelectors[0].getInteraction() == true)
                    {
                        //Console.WriteLine("We're checking for collision with defeatScreen!");
                        //Console.WriteLine("Who are we checking for collision with?!?!?  " + levelChooser);
                        allSelectors[0].isColliding(levelChooser);
                    }
                    break;
                case "Content/victoryScreen.txt":
                    if (allSelectors[5].getInteraction() == true)
                    {
                        allSelectors[5].isColliding(levelChooser);
                    }
                    break;
            }
        }

        public static ScreenManager getInstance()
        {
            if (instance == null)
            {
                instance = new ScreenManager();
                return instance;
            }
            else
                return instance;
        }

        /* Screen manager needs to do the following:
         * Create a list of maps:
         *      screenList[0] = defeatScreen (graveyard) screenList[1] = mainScreen; screenList[2] = levelSelect;
         *      screenList[3] = castleDefense; screenList[4] = riverDefense; screenList[5] = castleSiege;
         * If current map is levelSelect, then all selectors will be pointing to game levels.
         * If current map is a game level, there are two conditions: 
         *      if victory, advance to next screen
         *      if loss, decrement screen
         * 
         * Turn the selectors off
         * 
         */

        public void updateSelectors(String victoryCondition)
        {
            if (victoryCondition.Equals("victory"))
            {
                // The only time you can achieve "victory" is on playable maps
                switch (currentMap.name)
                {
                    case "Content/castleDefense.txt":
                        //Console.WriteLine("We won castleDefense!");
                        currentMap = allMaps[2];
                        //Console.WriteLine("allMaps[2] is: " + allMaps[2].name);
                        allSelectors[3].setInteraction(true);
                        allSelectors[3].setUnlocked(true);
                        allSelectors[2].setUnlocked(true);
                        break;
                    case "Content/riverDefense.txt":
                        currentMap = allMaps[2];
                        allSelectors[3].setUnlocked(true);
                        allSelectors[2].setUnlocked(true);
                        allSelectors[4].setUnlocked(true);
                        allSelectors[4].setInteraction(true);
                        break;
                    case "Content/castleSiege.txt":
                        currentMap = allMaps[6];
                        allSelectors[5].setUnlocked(true);
                        allSelectors[5].setInteraction(true);
                        break;
                }
                victory = "undef";
            }
            else if (victoryCondition.Equals("defeat"))
            {
                switch (currentMap.name)
                {
                    case "Content/castleDefense.txt":
                        currentMap = allMaps[0];
                        nextMap = allMaps[1];
                        allSelectors[0].setUnlocked(true);
                        allSelectors[0].setInteraction(true);
                        //Console.WriteLine("interaction for defeatScreen is: " + allSelectors[0].getInteraction());
                        //allSelectors
                        break;
                    case "Content/riverDefense.txt":
                        currentMap = allMaps[2];
                        allSelectors[3].setUnlocked(false);
                        allSelectors[2].setUnlocked(true);
                        allSelectors[3].setInteraction(false);
                        break;
                    case "Content/castleSiege.txt":
                        currentMap = allMaps[2];
                        allSelectors[2].setUnlocked(true);
                        allSelectors[3].setUnlocked(true);
                        allSelectors[4].setUnlocked(false);
                        allSelectors[4].setInteraction(false);
                        break;
                }
                victory = "undef";
            }
            else
            {
                //Console.WriteLine("Victory condition in updateSelectors is " + victoryCondition);
                //Console.WriteLine(currentMap.name);
                switch (currentMap.name)
                {
                    case "Content/defeatScreen.txt":
                        //Console.WriteLine("Hello from updateSelectors in defeatScreen switch case");
                        allSelectors[0].setInteraction(true);
                        allSelectors[0].setUnlocked(true);
                        //allSelectors[0].setOccupied(false);
                        break;
                    case "Content/MainScreen.txt":
                        // First update the selector attributes for the given level
                        //Console.WriteLine("Hello from updateSelectors in mainScreen switch case");
                        allSelectors[0].setUnlocked(false);
                        allSelectors[1].setInteraction(true);
                        allSelectors[1].setUnlocked(true);
                        if (allSelectors[1].getOccupied() == true)
                        {
                            //Console.WriteLine("You've reached the mainScreen selector!");
                            currentMap = allMaps[2];
                            nextMap = allMaps[3];
                            allSelectors[2].setInteraction(true);
                            allSelectors[2].setUnlocked(true);
                        }
                        break;
                    case "Content/levelSelect.txt":
                        if ((allSelectors[2].getOccupied() == true) && (allSelectors[2].getInteraction() == true)
                            && (allSelectors[2].getUnlocked() == true))
                        {
                            currentMap = allMaps[3];
                            foreach (Selector select in allSelectors)
                            {
                                select.setUnlocked(false);
                            }
                            progressIndex = 0;
                        }
                        if ((allSelectors[3].getOccupied() == true) && (allSelectors[3].getInteraction() == true)
                            && (allSelectors[3].getUnlocked() == true))
                        {
                            currentMap = allMaps[4];
                            foreach (Selector select in allSelectors)
                            {
                                select.setUnlocked(false);
                            }
                            progressIndex = 1;
                        }
                        if ((allSelectors[4].getOccupied() == true) && (allSelectors[4].getInteraction() == true)
                            && (allSelectors[4].getUnlocked() == true))
                        {
                            currentMap = allMaps[5];
                            foreach (Selector select in allSelectors)
                            {
                                select.setUnlocked(false);
                            }
                            progressIndex = 3;
                        }
                        break;
                    case "Content/victoryScreen.txt":
                        allSelectors[5].setInteraction(true);
                        allSelectors[5].setUnlocked(true);
                        break;
                }
            }
        }
    }
}
