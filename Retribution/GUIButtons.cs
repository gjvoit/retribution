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
    class GUIButtons
    {

        public int playerScore;
        public int aiScore;
        public int prevScore;
        public int timeDrain;
        public int disAd;
        public int waveNum;
        public Texture2D archerButton;
        public Texture2D apprenticeButton;
        public Texture2D clericButton;
        public Texture2D catapultButton;
        public Texture2D rogueButton;
        public Texture2D pawnButton;
        public Texture2D commanderButton;
        public Texture2D towerButton;
        public Texture2D warriorButton;
        public Dictionary<String,Rectangle> buttonCols;
        public List<String> labs;
        public static int resources = 0;
        public bool active;
        ModelManager modMan;
        private GUIButtons()
        {
            waveNum = 0;
            aiScore = 0;
            playerScore = 0;
            timeDrain = 0;
            disAd = 0;
            active = false;
            labs = new List<String>(new string[]{"ARCHER", "PAWN", "TOWER", "WARRIOR", "COMMANDER", "CLERIC", "ROGUE", "APPRENTICE", "CATAPULT" });
            buttonCols = new Dictionary<String,Rectangle>();
        }
        public GUIButtons(ref ModelManager mod)
        {
            modMan = mod;
            waveNum = 0;
            aiScore = 0;
            playerScore = 0;
            timeDrain = 0;
            disAd = 0;
            active = false;
            buttonCols = new Dictionary<String,Rectangle>();
            labs = new List<String>(new string[] { "ARCHER", "PAWN", "TOWER", "WARRIOR", "COMMANDER", "CLERIC", "ROGUE", "APPRENTICE", "CATAPULT" });
            init();
        }
        public void init()
        {
            int j = 0;
            for (int row = 0; row <= 132; row += 66)
             {
                for (int col = 1070; col <= 1202; col += 66)   
                {
                    Rectangle button = new Rectangle(col, row, 64, 64);       
                    buttonCols.Add(labs[j],button);
                    j++;
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(archerButton, new Vector2(1070, 0), colorUtil(Archer.cost));
            spriteBatch.Draw(pawnButton, new Vector2(1136, 0), colorUtil(Pawn.cost));
            spriteBatch.Draw(towerButton, new Vector2(1202, 0), colorUtil(Tower.cost));
            spriteBatch.Draw(warriorButton, new Vector2(1070, 66), colorUtil(Warrior.cost));
            spriteBatch.Draw(commanderButton, new Vector2(1136,66), colorUtil(Commander.cost));
            spriteBatch.Draw(clericButton, new Vector2(1202,66), colorUtil(Cleric.cost));
            spriteBatch.Draw(rogueButton, new Vector2(1070, 132), colorUtil(Rogue.cost));
            spriteBatch.Draw(apprenticeButton, new Vector2(1136, 132), colorUtil(Apprentice.cost));
            spriteBatch.Draw(catapultButton, new Vector2(1202, 132), colorUtil(Catapult.cost));

            //spriteBatch.Draw(createRBar(spriteBatch.GraphicsDevice), new Vector2(1048, 0), Color.White);

        }
        public Color colorUtil(int cost)
        {
            if (cost <= MoraleBar.resources)
                return Color.White;
            else
                return Color.Gray;
        }
        public Texture2D createButton(GraphicsDevice arg)
        {

            Texture2D resBar = new Texture2D(arg, 64, 64);
            Color[] data = new Color[64*64];
            for (int i = 0; i < data.Length; i++)
                data[i] = Color.Gray;

            //int scaled = resources * 704 / 150; //150 max resource right now

            //for (int j = 0; j < scaled * 20; j++)
            //    data[j] = Color.Gold;
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
            if (aiScore < 0)
                aiScore = 0;
            if (playerScore < 0)
                playerScore = 0;

            int total = aiScore + playerScore;
            Texture2D texture = new Texture2D(arg, 20, 704);
            Color[] data = new Color[20 * 704];
            for (int i = 0; i < data.Length; i++)
                data[i] = Color.Red;

            int scaled = playerScore * 704;
            if (total != 0)
                scaled = scaled / total;
            for (int j = scaled * 20 - 1; j > 0; j--)
                data[j] = Color.LawnGreen;
            texture.SetData(data);
            return texture;
        }
        public void waveMech()
        {
            if (playerScore <= 0 && aiScore > 0)
            {
                waveNum++;
                if (waveNum > 5)
                    waveNum = 5;
                disAd = 0;
                timeDrain = 0;
                for (int x = 0; x < waveNum; x++)
                    modMan.addUnit("ARTIFICIAL", reinforce(), new Vector2(0 + x * 32, 25));
            }
        }
        public void LoadContent(ContentManager content)
        {
            archerButton = content.Load<Texture2D>("archerButton.png");
            apprenticeButton = content.Load<Texture2D>("apprenticeButton.png");
            clericButton = content.Load<Texture2D>("clericButton.png");
            catapultButton = content.Load<Texture2D>("catapultButton.png");
            commanderButton = content.Load<Texture2D>("commanderButton.png");
            pawnButton = content.Load<Texture2D>("pawnButton.png");
            rogueButton = content.Load<Texture2D>("rogueButton.png");
            towerButton = content.Load<Texture2D>("towerButton.png");
            warriorButton = content.Load<Texture2D>("warriorButton.png");
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
            prevScore = playerScore;
            playerScore = 0 - timeDrain - disAd;
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
                        playerScore += 200 + unit.health;
                        break;
                    case "APPRENTICE":
                        playerScore += 300 + unit.health;
                        break;
                    case "CATAPULT":
                        playerScore += 800 + unit.health;
                        break;
                    case "CLERIC":
                        playerScore += 500 + unit.health;
                        break;
                    case "COMMANDER":
                        playerScore += 3000 + unit.health;
                        break;
                    case "PAWN":
                        playerScore += 100 + unit.health;
                        break;
                    case "ROGUE":
                        playerScore += 1500 + unit.health;
                        break;
                    case "TOWER":
                        playerScore += 300 + unit.health;
                        break;
                    case "WARRIOR":
                        playerScore += 400 + unit.health;
                        break;
                }
            }
            foreach (GameObject unit in ModelManager.artificial)
            {
                switch (unit.type)
                {
                    case "ARCHER":
                        aiScore += 200 + unit.health;
                        break;
                    case "APPRENTICE":
                        aiScore += 300 + unit.health;
                        break;
                    case "CATAPULT":
                        aiScore += 800 + unit.health;
                        break;
                    case "CLERIC":
                        aiScore += 500 + unit.health;
                        break;
                    case "COMMANDER":
                        aiScore += 3000 + unit.health;
                        break;
                    case "PAWN":
                        aiScore += 100 + unit.health;
                        break;
                    case "ROGUE":
                        aiScore += 1500 + unit.health;
                        break;
                    case "TOWER":
                        aiScore += 300 + unit.health;
                        break;
                    case "WARRIOR":
                        aiScore += 400 + unit.health;
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
                disAd += timeDrain;
                timeDrain = 0;
                active = true;
            }
        }
    }
}