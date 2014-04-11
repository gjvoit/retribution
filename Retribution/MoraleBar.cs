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
        ModelManager modMan;
        private MoraleBar()
        {
            aiScore = 0;
            playerScore = 0;
        }
        public MoraleBar (ref ModelManager mod)
        {
            modMan = mod;
             }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(createBar(spriteBatch.GraphicsDevice), new Vector2(1027,0), Color.White);

        }
        public Texture2D createBar(GraphicsDevice arg)
        {
            int total = aiScore + playerScore;
            Texture2D texture = new Texture2D(arg, 20, 704);
            Color[] data = new Color[20*704];
            for (int i = 0; i < data.Length; i++)
                data[i] = Color.Red;

            int scaled = playerScore*704;
            if(total!=0)
                scaled=scaled/total;
            for (int j = 0; j < scaled*20; j++)
                data[j] = Color.LawnGreen;
            texture.SetData(data);
            return texture;
        }
        public void calculateScore()
        {
            playerScore = 0;
            aiScore = 0;
            foreach (GameObject unit in modMan.player)
            {
                switch (unit.type)
                {
                    case "ARCHER":
                        playerScore+=unit.health;
                        break;
                    case "APPRENTICE":
                        playerScore +=unit.health;
                        break;
                    case "CATAPULT":
                        playerScore += unit.health;
                        break;
                    case "CLERIC":
                        playerScore += unit.health;
                        break;
                    case "COMMANDER":
                        playerScore += 30 + unit.health;
                        break;
                    case "PAWN":
                        playerScore += unit.health;
                        break;
                    case"ROGUE":
                        playerScore += unit.health;
                        break;
                    case"TOWER":
                        playerScore += unit.health;
                        break;
                    case"WARRIOR":
                        playerScore += unit.health;
                        break;
                }
            }
            foreach (GameObject unit in modMan.artificial)
            {
                switch (unit.type)
                {
                    case "ARCHER":
                        aiScore += unit.health;
                        break;
                    case "APPRENTICE":
                        aiScore += unit.health;
                        break;
                    case "CATAPULT":
                        aiScore += unit.health;
                        break;
                    case "CLERIC":
                        aiScore += unit.health;
                        break;
                    case "COMMANDER":
                        aiScore += 30 + unit.health;
                        break;
                    case "PAWN":
                        aiScore += unit.health;
                        break;
                    case "ROGUE":
                        aiScore += unit.health;
                        break;
                    case "TOWER":
                        aiScore += unit.health;
                        break;
                    case "WARRIOR":
                        aiScore += unit.health;
                        break;
                }
            }

        }
    }
}