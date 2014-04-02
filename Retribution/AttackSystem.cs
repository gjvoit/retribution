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
    class AttackSystem
    {
        public List<GameObject> player;
        public List<GameObject> artificial;

        public AttackSystem(List<GameObject> player, List<GameObject> artificial)
        {
            this.player = player;
            this.artificial = artificial;
        }

        public void Update(List<GameObject> newPlayer, List<GameObject> newArtificial)
        {
            player = newPlayer;
            artificial = newArtificial;
        }

        public void autoAttacks()
        {
            for (int i = 0; i < artificial.Count; i++)
            {
                for (int j = 0; j < player.Count; j++)
                {
                    if(artificial[i].IsInRange(player[j]))
                    {
                        artificial[i].Attack(player[j]);
                       }
                    if(player[j].IsInRange(artificial[i])){
                        player[j].Attack(artificial[i]);

                    }
                }

            }


        }
    }
}
