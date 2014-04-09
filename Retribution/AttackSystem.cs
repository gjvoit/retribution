#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using System.Diagnostics;
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
                    //if (pobj.aiTarget != null&&pobj.aiTarget.alive)
                    //{
                    //    if (pobj.attackWait <= 0)
                    //    {
                    //        if (pobj.isAlive() && !pobj.attacked)
                    //        {
                    //            pobj.Attack(pobj.aiTarget);
                    //            pobj.resetAttack();
                    //        }
                    //        if (String.Compare(pobj.type, "ARCHER", true) == 0)
                    //        {
                    //            projMan.proj.Add(((Archer)pobj).myArrow);
                    //            ((Archer)pobj).myArrow.LoadContent(content);
                    //        }
                    //        if (String.Compare(pobj.type, "APPRENTICE", true) == 0)
                    //        {
                    //            projMan.proj.Add(((Apprentice)pobj).myArrow);
                    //            ((Apprentice)pobj).myArrow.LoadContent(content);
                    //        }
                    //        if (String.Compare(pobj.type, "TOWER", true) == 0)
                    //        {
                    //            projMan.proj.Add(((Tower)pobj).myArrow);
                    //            ((Tower)pobj).myArrow.LoadContent(content);
                    //        }
                    //    }
                    //    else
                    //        pobj.attackWait--;
                    //    break;
                    //}
                    //else  
                    if (pobj.IsInRange(aobj)) //if ai is attackable by player
                    {
                        pobj.aiTarget=aobj;
                        if (pobj.attackWait <= 0)
                        {
                            //  If the player object is alive and is not currently being attacked
                            if (pobj.isAlive() && !pobj.attacked)
                            {
                                //  Call player object attack
                                pobj.Attack(aobj, content, projMan);
                                pobj.resetAttack();
                            }
                            /*
                            if (String.Compare(pobj.type, "APPRENTICE", true) == 0)
                            if (pobj.isAlive() && !pobj.attacked)
                            {
                                pobj.Attack(aobj);
                                pobj.resetAttack();
                            }
                            if (String.Compare(pobj.type, "ARCHER", true) == 0)
                            {
                                projMan.proj.Add(((Archer)pobj).myArrow);
                                ((Archer)pobj).myArrow.LoadContent(content);
                            }
                            if (String.Compare(pobj.type, "APPRENTICE", true) == 0)
                            {
                                projMan.proj.Add(((Apprentice)pobj).myArrow);
                                ((Apprentice)pobj).myArrow.LoadContent(content);
                            }
                            if (String.Compare(pobj.type, "TOWER", true) == 0)
                            {
                                projMan.proj.Add(((Tower)pobj).myArrow);
                                ((Tower)pobj).myArrow.LoadContent(content);
                            }
                             * */
                        }
                        else
                            pobj.attackWait--;

                    }
                    if (aobj.IsInRange(pobj))
                    {
                        if (aobj.attackWait <= 0)
                        {
                            if (aobj.isAlive() && !aobj.attacked)
                            {
                                //  Call player object attack
                                aobj.Attack(pobj, content, projMan);
                                aobj.resetAttack();
                            }
                            /*
                            if (aobj.isAlive() && !aobj.attacked)
                            {
                                aobj.Attack(pobj);
                                aobj.resetAttack();
                            }
                            if (String.Compare(aobj.type, "ARCHER", true) == 0)
                            {
                                projMan.proj.Add(((Archer)aobj).myArrow);
                                ((Archer)aobj).myArrow.LoadContent(content);

                            }
                            if (String.Compare(aobj.type, "TOWER", true) == 0)
                            {
                                projMan.proj.Add(((Tower)aobj).myArrow);
                                ((Tower)aobj).myArrow.LoadContent(content);
                            }
                             * */
                        }
                        else
                            aobj.attackWait--;
                    }
                    
                }
            }
        }
        /*
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
         * */
    }
}
