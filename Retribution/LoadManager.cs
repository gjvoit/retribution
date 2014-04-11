using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace Retribution
{

    class LoadManager
    {
        public static LoadManager instance;
        private LoadManager()
        {
        }
        public static LoadManager getInstance()
        {
            if (instance == null)
            {
                instance = new LoadManager();
                return instance;
            }
            else
                return instance;
        }
        //public void loadContent(ContentManager content)
        //{
        //    Archer.LoadContent(content);
        //    Apprentice.LoadContent(content);
        //    Arrow.LoadContent(content);
        //    Catapult.LoadContent(content);
        //    Cleric.LoadContent(content);
        //    ClickBox.LoadContent(content);
        //    Commander.LoadContent(content);
        //    Digits.LoadContent(content);
        //    Fireball.LoadContent(content);
        //    Iceball.LoadContent(content);
        //    Rogue.LoadContent(content);
        //    titleShell.LoadContent(content);
        //    Tower.LoadContent(content);
        //    Warrior.LoadContent(content);
        //    Pawn.LoadContent(content);
        //}
        public void load(ContentManager content, List<GameObject> toLoad)
        {
            

            for (int x = 0; x < toLoad.Count; x++)
            {

                toLoad[x].LoadContent(content);
                //switch (toLoad[x].type)
                //{
                //    case "ARCHER":
                //        ((Archer)toLoad[x]).LoadContent(content);
                //        break;
                //    case "TOWER":
                //        ((Tower)toLoad[x]).LoadContent(content);
                //        break;
                //}
            }


        }
    }
}