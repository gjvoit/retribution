using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
namespace Retribution
{
    class InfoCard
    {
        public SpriteFont txt;
        public static String name="";
        public static String descrip="";
        public static String range="";
        public static String damage="";
        public static String health="";
        public static String movespeed="";
        public static String attackspeed="";
        public static String special="";
        public static String cost = "";
        public static Texture2D texture;
        public static int resources = 0;
        public bool active;
        ModelManager modMan;
        public SoundEffect horn;
        private InfoCard()
        {
            active = false;
        }
        public InfoCard(ref ModelManager mod)
        {
            modMan = mod;

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            
            spriteBatch.DrawString(txt, name+"   COST:"+cost, new Vector2(1070, 198), Color.White);
            spriteBatch.Draw(texture, new Rectangle(1079,244,32,32), Color.White);
            spriteBatch.DrawString(txt, health + " HP", new Vector2(1120, 220), Color.White);
            spriteBatch.DrawString(txt, damage + " DAMAGE", new Vector2(1120, 240), Color.White);
            spriteBatch.DrawString(txt, range + " RANGE", new Vector2(1120, 260), Color.White);
            spriteBatch.DrawString(txt, attackspeed+ " ATTKSPEED", new Vector2(1120, 280), Color.White);
            spriteBatch.DrawString(txt, movespeed + " MOVESPEED", new Vector2(1120, 300), Color.White);
            spriteBatch.DrawString(txt, descrip, new Vector2(1070, 320), Color.White);
            spriteBatch.DrawString(txt, special, new Vector2(1070, 360), Color.White);
            spriteBatch.DrawString(txt, "Left Click(&Drag) to Select",new Vector2(1070,460), Color.White);
            spriteBatch.DrawString(txt, "Right Click to Command", new Vector2(1070, 480), Color.White);
            spriteBatch.DrawString(txt, "F for special attack\nLeft click +S to place Towers", new Vector2(1070, 500), Color.White);
            //spriteBatch.Draw(createBar(spriteBatch.GraphicsDevice), new Vector2(1025, 0), Color.White);
            //spriteBatch.Draw(createRBar(spriteBatch.GraphicsDevice), new Vector2(1046, 0), Color.White);

        }
        public static void info(GameObject unit)
        {
            cost = getCost(unit.type);
            name = unit.type;
            texture = unit.texture;
            descrip = pullText(unit.type);
            range = unit.attackRange.ToString();
            if (unit.GetType().BaseType == typeof(Mobile))
                movespeed = ((Mobile)unit).moveSpeed.ToString();
            else
                movespeed = "0";
            damage=unit.damage.ToString();
            health=unit.basehealth.ToString();
            attackspeed=unit.attackSpeed.ToString();
            special=specialText(unit.type);
            
        }
        public static String getCost(String Type)
        {
            switch (Type)
            {
                case "ARCHER":
                    return Archer.cost.ToString();
                case "APPRENTICE":
                    return Apprentice.cost.ToString();
                case "CATAPULT":
                    return Catapult.cost.ToString();
                case "CLERIC":
                    return Cleric.cost.ToString();
                case "COMMANDER":
                    return Commander.cost.ToString();
                case "PAWN":
                    return Pawn.cost.ToString();
                case "ROGUE":
                    return Rogue.cost.ToString();
                case "TOWER":
                    return Tower.cost.ToString();
                case "WARRIOR":
                    return Warrior.cost.ToString();
            }
            return "0";
        }
        public static String pullText(String type)
        {
            switch (type)
            {
                case "ARCHER":
                    return "An elite ranged unit with moderate \ndamage--fast and inexpensive";
                case "APPRENTICE":
                    return "A vulnerable mystic wielding a\ncrippling range-reducing magic attack";
                case "CATAPULT":
                    return "A terror on the battlefield--inaccurate\nbut destructive to clumped units";
                case "CLERIC":
                    return "A holy healer dealing no damage but\nhealing all same-side units in range";
                case "COMMANDER":
                    return "A formidable warrior with morale and\nrallying bonus";
                case "PAWN":
                    return "The backbone of the army; devastating\n in numbers";
                case "ROGUE":
                    return "A quick, shadowy, assassin deadly\n with the hit and run";
                case "TOWER":
                    return "A stalwart defender with impressive \nrange";
                case "WARRIOR":
                    return "A chivalrous tank, slow but powerful\nunless enraged";
            }
            return "This is a description of the unit";
        }
        public static String specialText(String type)
        {
            switch (type)
            {
                case "ARCHER":
                    return "Rapidfire: A powerful volley of arrows\n useful for finishing an enemy\nCooldown:10 sec";
                case "APPRENTICE":
                    return "Fireball: Spout flames directed towards\n the mouse--damages the first hit!\nCooldown:4 sec";
                case "CATAPULT":
                    return "No Special";
                case "CLERIC":
                    return "No Special";
                case "COMMANDER":
                    return "Rally: Summons all non-engaged\n units to him and heals a small amount\nCooldown:15 sec";
                case "PAWN":
                    return "No Special";
                case "ROGUE":
                    return "Stealth:Engages a high chance that he\nwill lose his enemy's aggression\nCooldown:8 sec";
                case "TOWER":
                    return "Entrench:Health and damage boost\nin exchange for range and attackspeed\nCooldown:Toggleable";
                case "WARRIOR":
                    return "Juggernaut:Increased damage and speed\nat the cost of 20% health and range\nCooldown:4 sec";
            }
            return "This is a description of the unit special";
        }
        public Texture2D createRBar(GraphicsDevice arg)
        {
            if (resources > 150)
                resources = 150;
            if (resources < 0)
                resources = 0;
            Texture2D resBar = new Texture2D(arg, 20, 704);
            Color[] data = new Color[20 * 704];
            for (int i = 0; i < data.Length; i++)
                data[i] = Color.Gray;

            int scaled = resources * 704 / 150; //150 max resource right now

            for (int j = 0; j < scaled * 20; j++)
                data[j] = Color.Gold;
            resBar.SetData(data);
            return resBar;
        }
        public static void resourceVal(int res)
        {
            resources = res;
        }
        public static void resourceAdd(int res)
        {
            resources += res;
        }
        public Texture2D createBar(GraphicsDevice arg)
        {
            //playerScore--;
            Texture2D texture = new Texture2D(arg, 20, 704);
            Color[] data = new Color[20 * 704];
            for (int i = 0; i < data.Length; i++)
                data[i] = Color.Black;

//            int scaled = playerScore * 704;
  //          if (total != 0)
    //            scaled = scaled / total;
            //for (int j = scaled * 20 - 1; j > 0; j--)
                //data[j] = Color.GhostWhite;
            texture.SetData(data);
            return texture;
        }
        public void waveMech()
        {
            //if (playerScore <= 0 && aiScore > 0 && waveNum < 5)
            //{
            //    waveNum++;
            //    if (waveNum > 5)
            //        waveNum = 5;
            //    disAd = 0;
            //    timeDrain = 0;
            //    horn.Play();
            //    for (int x = 0; x < waveNum; x++)
            //    {
                  //  modMan.addUnit("ARTIFICIAL", reinforce(), new Vector2(150 + x * 32, 25));
                //}
            //}
        }
        public String reinforce()
        {
            Random rand = new Random();
            int temp = rand.Next(1, 12);
            switch (temp)
            {
                case 1:
                case 2:
                case 3:
                    return "ARCHER";
                case 4:
                    return "APPRENTICE";
                case 5:
                    return "CLERIC";
                case 6:
                    return "WARRIOR";
                case 7:
                    return "COMMANDER";
                case 8:
                    return "ROGUE";
                case 9:
                    return "CATAPULT";
                case 10:
                case 11:
                case 12:
                    return "PAWN";
                // break;
            }
            return "PAWN";
        }
        public void calculateScore()
        {
            waveMech();
            //prevScore = playerScore;
            //playerScore = 0 - timeDrain - disAd;
            //aiScore = 0;
            //foreach (GameObject unit in ModelManager.player)
            //{
            //    if (unit.GetType().BaseType == typeof(Mobile))
            //    {
            //        if (((Mobile)unit).isMoving)
            //            active = true;
            //    }
            //    switch (unit.type)
            //    {
            //        case "ARCHER":
            //            playerScore += 200 + unit.health;
            //            break;
            //        case "APPRENTICE":
            //            playerScore += 300 + unit.health;
            //            break;
            //        case "CATAPULT":
            //            playerScore += 800 + unit.health;
            //            break;
            //        case "CLERIC":
            //            playerScore += 500 + unit.health;
            //            break;
            //        case "COMMANDER":
            //            playerScore += 3000 + unit.health;
            //            break;
            //        case "PAWN":
            //            playerScore += 100 + unit.health;
            //            break;
            //        case "ROGUE":
            //            playerScore += 1500 + unit.health;
            //            break;
            //        case "TOWER":
            //            playerScore += 300 + unit.health;
            //            break;
            //        case "WARRIOR":
            //            playerScore += 400 + unit.health;
            //            break;
                //}
            //}
            //foreach (GameObject unit in ModelManager.artificial)
            //{
            //    switch (unit.type)
            //    {
            //        case "ARCHER":
            //            aiScore += 200 + unit.health;
            //            break;
            //        case "APPRENTICE":
            //            aiScore += 300 + unit.health;
            //            break;
            //        case "CATAPULT":
            //            aiScore += 800 + unit.health;
            //            break;
            //        case "CLERIC":
            //            aiScore += 500 + unit.health;
            //            break;
            //        case "COMMANDER":
            //            aiScore += 3000 + unit.health;
            //            break;
            //        case "PAWN":
            //            aiScore += 100 + unit.health;
            //            break;
            //        case "ROGUE":
            //            aiScore += 1500 + unit.health;
            //            break;
            //        case "TOWER":
            //            aiScore += 300 + unit.health;
            //            break;
            //        case "WARRIOR":
            //            aiScore += 400 + unit.health;
            //            break;
            //    }
            //}
            //if (playerScore == prevScore)//if recalculated is the same as what it was
            //{
            //    active = false;
            //}
            //if (!active)
            //    timeDrain++;
            //if (prevScore - playerScore > 1)
            //{//if you lost more than 1
            //    disAd += timeDrain;
            //    timeDrain = 0;
            //    active = true;
            //}
        }
    }
}