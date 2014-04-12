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
        //public SoundEffect soundEffect;

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
                    MoraleBar.resourceAdd(1);
                    artificial.Remove(artificial[i]);
                }
            }
        }
    }
}
