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
        ModelManager modMan;
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
            foreach (Projectile projectile in proj)       //  pobj stands for projectile object
            {
                foreach (GameObject aobj in ModelManager.getArtificial())
                {
                    if (projectile.isAlive())
                    {
                        if (projectile.collisionType == "homing")
                        {
                            if (projectile.position.X <= projectile.end_point.X && projectile.position.X >= projectile.prev_point.X
                                && projectile.position.Y <= projectile.end_point.Y && projectile.position.Y >= projectile.prev_point.Y) //equivalent to IsInRange
                            {
                                projectile.health = -1;
                                projectile.collided = true;
                                if (projectile.target.isAlive())
                                {
                                    projectile.target.health -= projectile.damage;
                                }

                                if (String.Compare(projectile.type, "ICEBALL", true) == 0 && projectile.target.isAlive())
                                {
                                    if (projectile.target.attackRange > 5)
                                        projectile.target.attackRange -= 5;
                                    projectile.target.attackSpeed += 10;
                                }

                            }
                        }
                        else if (projectile.collisionType == "straight")
                        {
                            if (projectile.IsInRange(aobj))
                            {
                                projectile.health = -1;
                                projectile.collided = true;
                                if (aobj.isAlive())
                                {
                                    aobj.health -= projectile.damage;
                                }
                            }
                        }
                        else if (projectile.collisionType == "arc")
                        {
                            if (projectile.position.X <= projectile.end_point.X && projectile.position.X >= projectile.prev_point.X
                                && projectile.position.Y <= projectile.end_point.Y && projectile.position.Y >= projectile.prev_point.Y)
                            {
                                projectile.collided = true;
                                if (aobj.isAlive() && projectile.IsInRange(aobj))
                                {
                                    aobj.health -= projectile.damage;
                                }
                            }
                        }
                    }
                }
                if (projectile.collisionType == "arc")  //  Kills the arc projectile at the end, after checking collision with all objects
                    if (projectile.position.X <= projectile.end_point.X && projectile.position.X >= projectile.prev_point.X
                        && projectile.position.Y <= projectile.end_point.Y && projectile.position.Y >= projectile.prev_point.Y)
                        projectile.health = -1;

                

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