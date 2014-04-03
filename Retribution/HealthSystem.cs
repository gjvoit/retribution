#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace Retribution
{
    class HealthSystem
    {
        public List<GameObject> player;
        public List<GameObject> artificial;

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

        public void checkHealth()
        {
            for (int i = 0; i < player.Count; i++)
            {
                if (player[i].isAlive() == false)
                {
                    player.Remove(player[i]);
                }
            }

            for (int i = 0; i < artificial.Count; i++)
            {
                if (artificial[i].isAlive() == false)
                {
                    artificial.Remove(artificial[i]);
                }
            }
        }
    }
}
