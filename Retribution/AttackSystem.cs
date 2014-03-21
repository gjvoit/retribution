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
        public List<Tower> towers;
        public List<Archer> archers;

        public AttackSystem(List<Tower> newTowers, List<Archer> newArchers)
        {
            towers = new List<Tower>(newTowers);
            archers = new List<Archer>(newArchers);
        }

        public void Update(List<Tower> newTowers, List<Archer> newArchers)
        {
            towers = newTowers;
            archers = newArchers;
        }

        public void autoAttacks()
        {
            for (int i = 0; i < towers.Count; i++)
            {
                for (int j = 0; j < archers.Count; j++)
                {
                    if(towers[i].IsInRange(archers[j]))
                    {
                        towers[i].Attack(archers[j]);
                        break;
                    }
                }

            }


        }
    }
}
