using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace Retribution
{

    public class ProjectileManager
    {
        public List<Projectile> proj;
        public static ProjectileManager instance;
        private ProjectileManager()
        {
            proj = new List<Projectile>();
        }
        public static ProjectileManager getInstance()
        {
            if (instance == null)
            {
                instance = new ProjectileManager();
                return instance;
            }
            else
                return instance;
        }
        public void updateProjectiles(List<Projectile> newProjectiles)
        {
            proj = newProjectiles;
        }
        public void fireProjectiles()
        {
            foreach (Projectile item in proj)
            {
                item.move();
             }
            refreshProjectiles();
            
        }

        public void refreshProjectiles()
        {
            foreach (Projectile item in proj.ToArray())
            {
                if (!item.isAlive()||item.collided)
                    proj.Remove(item);
                //else
                //   item.attackWait--;
            }

        }

    }
}