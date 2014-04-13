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
    class MoraleBar
    {
       
        public int playerScore;
        public int aiScore;
        public int prevScore;
        public int timeDrain;
        public int disAd;
        public int waveNum;
        public static int resources = 0;
        public bool active;
        ModelManager modMan;
        public SoundEffect horn;
        private MoraleBar()
        {
            waveNum = 0;
            aiScore = 0;
            playerScore = 0;
            timeDrain = 0;
            disAd = 0;
            active = false;
        }
        public MoraleBar (ref ModelManager mod)
        {
            modMan = mod;
 
             }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(createBar(spriteBatch.GraphicsDevice), new Vector2(1025,0), Color.White);
            spriteBatch.Draw(createRBar(spriteBatch.GraphicsDevice), new Vector2(1046,0), Color.White);

        }
        public Texture2D createRBar(GraphicsDevice arg){
            if (resources > 150)
                resources = 150;
            if (resources < 0)
                resources = 0;
                   Texture2D resBar= new Texture2D(arg, 20,704);
                   Color[] data = new Color[20 * 704];
                   for (int i = 0; i < data.Length; i++)
                       data[i] = Color.Gray;

                   int scaled = resources * 704/150; //150 max resource right now
                 
                   for (int j = 0; j < scaled*20; j++)
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
            resources+= res;
        }
        public Texture2D createBar(GraphicsDevice arg)
        {
            //playerScore--;
            if (aiScore < 0)
                aiScore = 0;
            if (playerScore < 0)
                playerScore = 0;
            
            int total = aiScore + playerScore;
            Texture2D texture = new Texture2D(arg, 20, 704);
            Color[] data = new Color[20*704];
            for (int i = 0; i < data.Length; i++)
                data[i] = Color.Black;

            int scaled = playerScore*704;
            if(total!=0)
                scaled=scaled/total;
            for (int j = scaled*20-1; j > 0; j--)
                data[j] = Color.GhostWhite;
            texture.SetData(data);
            return texture;
        }
        public void waveMech()
        {
            if (playerScore <= 0 && aiScore > 0&&waveNum<5)
            {
                waveNum++;
                if (waveNum > 5)
                    waveNum = 5;
                disAd = 0;
                timeDrain = 0;
                horn.Play();
                for (int x = 0; x < waveNum; x++)
                {
                    modMan.addUnit("ARTIFICIAL", reinforce(), new Vector2(150 + x * 32, 25));
                }
            }
        }
        public String reinforce()
        {
            Random rand = new Random();
            int temp=rand.Next(1,12);
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
            prevScore = playerScore;
            playerScore = 0-timeDrain-disAd;
            aiScore = 0;
            foreach (GameObject unit in ModelManager.player)
            {
                if (unit.GetType().BaseType == typeof(Mobile))
                {
                    if (((Mobile)unit).isMoving)
                        active = true;
                }
                switch (unit.type)
                {
                    case "ARCHER":
                        playerScore+=200+unit.health;
                        break;
                    case "APPRENTICE":
                        playerScore +=300+unit.health;
                        break;
                    case "CATAPULT":
                        playerScore +=800+unit.health;
                        break;
                    case "CLERIC":
                        playerScore +=500+unit.health;
                        break;
                    case "COMMANDER":
                        playerScore += 3000 + unit.health;
                        break;
                    case "PAWN":
                        playerScore +=100+unit.health;
                        break;
                    case"ROGUE":
                        playerScore +=1500+unit.health;
                        break;
                    case"TOWER":
                        playerScore +=300+unit.health;
                        break;
                    case"WARRIOR":
                        playerScore +=400+unit.health;
                        break;
                }
            }
            foreach (GameObject unit in ModelManager.artificial)
            {
                switch (unit.type)
                {
                    case "ARCHER":
                        aiScore +=200+unit.health;
                        break;
                    case "APPRENTICE":
                        aiScore +=300+unit.health;
                        break;
                    case "CATAPULT":
                        aiScore +=800+unit.health;
                        break;
                    case "CLERIC":
                        aiScore +=500+unit.health;
                        break;
                    case "COMMANDER":
                        aiScore += 3000 + unit.health;
                        break;
                    case "PAWN":
                        aiScore +=100+unit.health;
                        break;
                    case "ROGUE":
                        aiScore +=1500+unit.health;
                        break;
                    case "TOWER":
                        aiScore +=300+unit.health;
                        break;
                    case "WARRIOR":
                        aiScore +=400+unit.health;
                        break;
                }
            }
            if (playerScore == prevScore)//if recalculated is the same as what it was
            {
                active = false;
                            }
            if (!active)
                timeDrain++;
            if (prevScore - playerScore > 1)
            {//if you lost more than 1
                disAd+= timeDrain;
                timeDrain = 0;
                active = true;
            }
        }
    }
}