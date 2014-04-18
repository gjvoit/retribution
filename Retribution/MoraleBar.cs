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
       
        public double playerScore;
        public double aiScore;
        public double prevScore;
        public double timeDrain;
        public double disAd;
        bool commander = false;
        int catapults = 0;
        int clerics = 0;
        public int waveNum;
        public int scaled;
        public SpriteFont txt;
        public static int resources = 0;
        public bool active;
        public bool bossSpawn = false;
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
            spriteBatch.DrawString(txt, resources.ToString(), new Vector2(1047, scaled), Color.White);
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

                   scaled = resources * 704/150; //150 max resource right now
                 
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
            
            int total = (int)(aiScore + playerScore);
            Texture2D texture = new Texture2D(arg, 20, 704);
            Color[] data = new Color[20*704];
            for (int i = 0; i < data.Length; i++)
                data[i] = Color.Black;

            int scaled = (int)(playerScore*704);
            
            if(total!=0)
                scaled=scaled/total;
            if (scaled >= 704)
                scaled = 704;
            for (int j = scaled*20-1; j > 0; j--)
                data[j] = Color.GhostWhite;
            texture.SetData(data);
            return texture;
        }
        public void waveMech()
        {
            
            if (playerScore <= 0 && aiScore > 0&&waveNum<=7)
            {
                int unitcount=ModelManager.player.Count-ModelManager.artificial.Count;
                if(unitcount>0)
                waveNum++;
                if (unitcount < 0)
                    unitcount = 0;
                if (waveNum >=7 &&!bossSpawn)
                {
                    waveNum = 7;
                    ModelManager.artificial.Add(new BossUnit(new Vector2(200, 25), (int)(playerScore * 10), 30, 100));
                    bossSpawn = true;
                }
                disAd = 0;
                catapults = 0;
                clerics = 0;
                commander = false;
                timeDrain = 0;
                horn.Play();
                for (int x = 0; x <unitcount+1; x++)
                {
                    modMan.addUnit("ARTIFICIAL", reinforce(), new Vector2(150 + x * 32, 25));
                }
            }
        }
        public String reinforce()
        {
            Random rand = new Random();
            int temp=rand.Next(1,12);
            if (waveNum <2)
            {
                switch (temp)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        return "ARCHER";
                    case 6:
                    case 7:
                    case 8:
                        return "WARRIOR";
                    case 9:
                    case 10:
                    case 11:
                    case 12:
                        return "PAWN";
                    // break;
                }
            }
            if (waveNum < 4)
            {
                switch (temp)
                {
                    case 1:
                    case 2:
                    case 3:
                        return "ARCHER";
                    case 4:
                        if (!commander)
                        {
                            commander = true;
                            return "COMMANDER";
                        }
                        else
                            return "WARRIOR";
                    case 5:
                        if (clerics < 2)
                        {
                            clerics++;
                            return "CLERIC";
                        }
                        else
                            return "APPRENTICE";
                    case 6:
                        return "ROGUE";
                    case 7:
                    case 8:    
                    case 9:
                        return "WARRIOR";
                    case 10:
                        return "APPRENTICE";
                    case 11:
                    case 12:
                        return "PAWN";
                    // break;
                }
            }
            if (waveNum < 5)
            {
                switch (temp)
                {
                    case 1:
                    case 2:
                    case 3:
                        return "ARCHER";
                    case 4:
                        if (!commander)
                        {
                            commander = true;
                            return "COMMANDER";
                        }
                        else
                            return "WARRIOR";
                    case 5:
                        if (catapults <= 2)
                        {
                            catapults++;
                            return "CATAPULT";
                        }
                        else
                            return "APPRENTICE";
                    case 6:
                        if (clerics < 2)
                        {
                            clerics++;
                            return "CLERIC";
                        }
                        else
                            return "APPRENTICE";
                    case 7:      
                    case 8:
                    case 9:
                        return "WARRIOR";
                    case 10:
                    case 11:
                        return "ROGUE";
                    case 12:
                        return "APPRENTICE";
                    // break;
                }
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
                        playerScore +=350+unit.health;
                        break;
                    case "CATAPULT":
                        playerScore +=2000+unit.health;
                        break;
                    case "CLERIC":
                        playerScore +=600+unit.health;
                        break;
                    case "COMMANDER":
                        playerScore += 3000 + unit.health;
                        break;
                    case "PAWN":
                        playerScore +=60+unit.health;
                        break;
                    case"ROGUE":
                        playerScore +=1500+unit.health;
                        break;
                    case"TOWER":
                        playerScore +=200+unit.health;
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
                        aiScore +=25+unit.health;
                        break;
                    case "APPRENTICE":
                        aiScore +=150+unit.health;
                        break;
                    case "CATAPULT":
                        aiScore +=400+unit.health;
                        break;
                    case "CLERIC":
                        aiScore +=250+unit.health;
                        break;
                    case "COMMANDER":
                        aiScore += 1500 + unit.health;
                        break;
                    case "PAWN":
                        aiScore +=17+unit.health;
                        break;
                    case "ROGUE":
                        aiScore +=300+unit.health;
                        break;
                    case "TOWER":
                        aiScore +=50+unit.health;
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
                timeDrain+=700/(aiScore);
            if (prevScore - playerScore > 1)
            {//if you lost more than 1
                disAd+= timeDrain;
                timeDrain = 0;
                active = true;
            }
        }
    }
}