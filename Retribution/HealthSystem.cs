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
        public List<Tower> towers;
        public List<Archer> archers;

        public HealthSystem(List<Tower> newTowers, List<Archer> newArchers)
        {
            towers = new List<Tower>(newTowers);
            archers = new List<Archer>(newArchers);
        }

        public void Update(List<Tower> newTowers, List<Archer> newArchers)
        {
            towers = newTowers;
            archers = newArchers;
        }

        public void checkHealth()
        {
            for (int i = 0; i < towers.Count; i++)
            {
                if (towers[i].isAlive() == false)
                {
                    towers.Remove(towers[i]);
                }
            }

            for (int i = 0; i < archers.Count; i++)
            {
                if (archers[i].isAlive() == false)
                {
                    archers.Remove(archers[i]);
                }
            }
        }
    }
}
