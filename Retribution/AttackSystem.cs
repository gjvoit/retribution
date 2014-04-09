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
        public void autoAttacks()
        {
            foreach (GameObject pobj in player)
            {

                if (pobj.aiTarget != null && pobj.aiTarget.alive && pobj.attackWait <= 0 && pobj.IsInRange(pobj.aiTarget))//Have target?
                {
                    pobj.Attack(pobj.aiTarget, content, projMan);
                    pobj.resetAttack();
                    break;
                }
                else
                {
                    if (pobj.attackWait <= 0)//ready to attack
                    {
                        foreach (GameObject aobj in artificial)
                        {
                            if (pobj.IsInRange(aobj)) //if ai is attackable by player
                            {
                                pobj.aiTarget = aobj;

                                //  If the player object is alive and is not currently being attacked
                                if (pobj.isAlive() && !pobj.attacked)
                                {
                                    //  Call player object attack

                                    pobj.Attack(aobj, content, projMan);
                                    pobj.resetAttack();
                                    break;
                                }
                            }//rangecheck
                        }//uses loop
                        
                    }//waitcheck
                }//has or search for target
                //if (String.Compare(pobj.type, "CLERIC", true) == 0&&pobj.attackWait<=0)
                //{
                //    foreach (GameObject hobj in player)
                //    {
                //        if (pobj.IsInRange(hobj))
                //        {
                //            if (hobj.health <= hobj.basehealth - 1)
                //                hobj.health += 1;
                //        }
                //    }
                //    pobj.resetAttack();
                //}
                pobj.attackWait--;
            }
            foreach (GameObject aobj in artificial)
            {
                ///AI ATTACKING
                if (aobj.aiTarget != null && aobj.aiTarget.alive && aobj.attackWait <= 0 && aobj.IsInRange(aobj.aiTarget))//Have target?
                {
                    aobj.Attack(aobj.aiTarget, content, projMan);
                    aobj.resetAttack();
                }
                else//find target
                {
                    if (aobj.attackWait <= 0)
                    {
                        foreach (GameObject pobj in player)
                        {
                            if (aobj.IsInRange(pobj))
                            {


                                if (aobj.isAlive() && !aobj.attacked)
                                {

                                    aobj.aiTarget = pobj;
                                    aobj.Attack(pobj, content, projMan);
                                    aobj.resetAttack();
                                }
                            }
                        }//uses loop
                        
                    }//waitcheck

                }//has target/search for target check
                if (String.Compare(aobj.type, "CLERIC", true) == 0&&aobj.attackWait<=0)
                {
                    foreach (GameObject hobj in artificial)
                    {
                        if (aobj.IsInRange(hobj))
                        {
                            if (hobj.health < hobj.basehealth - 1)
                                hobj.health += 1;
                        }
                    }
                    aobj.resetAttack();
                }
                aobj.attackWait--;
            }
        }
    }
}
    
