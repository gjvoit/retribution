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
        public ProjectileManager projMan;
        public ContentManager content;


        public AttackSystem(ref List<GameObject> player, ref List<GameObject> artificial)
        {
            this.player = player;
            this.artificial = artificial;
        }
        public void linkSystem(ProjectileManager newMan)
        {
            projMan = newMan;
        }
        public void linkContent(ContentManager cont)
        {
            content = cont;
        }
        public void Update(ref List<GameObject> newPlayer, ref List<GameObject> newArtificial)
        {
            player = newPlayer;
            artificial = newArtificial;
        }
        public void autoAttacks() { 
            foreach(GameObject pobj in player){
                foreach (GameObject aobj in artificial)
                {
                    if (pobj.IsInRange(aobj)) //if ai is attackable by player
                    {
                        if (pobj.attackWait <= 0)
                        {
                            if (pobj.isAlive() && !pobj.attacked)
                            {
                                pobj.Attack(aobj);
                                pobj.resetAttack();
                            }
                            if (String.Compare(pobj.type,"ARCHER",true)==0)
                            {
                                projMan.proj.Add(((Archer)pobj).myArrow);
                                ((Archer)pobj).myArrow.LoadContent(content);
                            }
                        }

                    }
                    else if (aobj.IsInRange(pobj))
                    {
                        if (aobj.attackWait <= 0)
                        {
                            if (aobj.isAlive() && !aobj.attacked)
                            {
                                aobj.Attack(pobj);
                                aobj.resetAttack();
                            }
                            if (String.Compare(aobj.type,"ARCHER",true)==0)
                            {
                                projMan.proj.Add(((Archer)aobj).myArrow);
                                ((Archer)aobj).myArrow.LoadContent(content);
                                
                            }
                        }
                    }
                    
                }
            }
        }
        public void autoAttacks(ContentManager content, ref List<Projectile> proj)
        {
            for (int j = 0; j < player.Count; j++) 
            {
                for (int i = 0; i < artificial.Count; i++)
                {
                    if (artificial[i].IsInRange(player[j]))
                    {
                        if (artificial[i].GetType() == typeof(Archer))
                        {
                            if (player[j].isAlive())
                            {
                                artificial[i].Attack(player[j]);
                                ((Archer)artificial[i]).myArrow.LoadContent(content);
                                proj.Add(((Archer)artificial[i]).myArrow);
                                break;

                            }
                        }
                        else if (artificial.GetType().BaseType != typeof(Projectile))
                        {
                            artificial[i].Attack(player[j]);
                        }
                    }
                }
            }
            for (int j = 0; j < player.Count; j++) 
            {
                for (int i = 0; i < artificial.Count; i++)
                {
                    if(player[j].IsInRange(artificial[i])){
                        if (player[j].GetType() == typeof(Archer))
                        {
                            if (artificial[i].isAlive())
                            {

                                player[j].Attack(artificial[i]);
                                ((Archer)player[j]).myArrow.LoadContent(content);
                                proj.Add(((Archer)player[j]).myArrow);
                                break;

                            }
                        }
                        else if (player.GetType().BaseType != typeof(Projectile))
                        {
                            player[j].Attack(artificial[i]);
                        }
                    }
                }

            }


        }
    }
}
