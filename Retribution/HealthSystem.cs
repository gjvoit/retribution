#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Audio;
#endregion

namespace Retribution
{
    class HealthSystem
    {
        public List<GameObject> player;
        public List<GameObject> artificial;
        public SoundEffect clink;

        public HealthSystem(List<GameObject> newPlayer, List<GameObject> newArtificial)
        {
            player = new List<GameObject>(newPlayer);
            artificial = new List<GameObject>(newArtificial);
           
        }

        public void Update(List<GameObject> newPlayer, List<GameObject> newArtificial)

        {
            player = newPlayer;
            artificial = newArtificial;
        }

        public void checkHealth(ContentManager content)
        {
            for (int i = 0; i < player.Count; i++)
            {
                if (player[i].isAlive() == false)
                {
                    player[i].kill(content);
                    player.Remove(player[i]);
                }
            }

            for (int i = 0; i < artificial.Count; i++)
            {
                if (artificial[i].isAlive() == false)
                {
                    MoraleBar.resourceAdd(resourceUtil(artificial[i].type));
                    clink.Play();
                    artificial.Remove(artificial[i]);
                }
            }
        }
        public int resourceUtil(String type)
        {
            switch (type)
            {
                case "ARCHER":
                    return Archer.cost / 2;
                case "APPRENTICE":
                    return Apprentice.cost / 2;
                case "PAWN":
                    return Pawn.cost / 2;
                case "COMMANDER":
                    return Commander.cost/2;
                case "CLERIC":
                    return Cleric.cost/2;
                case "CATAPULT":
                    return Catapult.cost / 2;
                case "TOWER":
                    return Tower.cost;
                case "ROGUE":
                    return Rogue.cost / 2;
                case "WARRIOR":
                    return Warrior.cost / 2;
            }
            return 1;



        }
    }
}
