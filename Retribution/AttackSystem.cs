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

        public AttackSystem(ref List<GameObject> player, ref List<GameObject> artificial)
        {
            this.player = player;
            this.artificial = artificial;
        }

        public void Update(ref List<GameObject> newPlayer, ref List<GameObject> newArtificial)
        {
            player = newPlayer;
            artificial = newArtificial;
        }

        public void autoAttacks(ContentManager content, ref List<Projectile> proj)
        {
            for (int i = 0; i < artificial.Count; i++)
            {
                for (int j = 0; j < player.Count; j++)
                {
                    if(artificial[i].IsInRange(player[j]))
                    {
                        if (artificial[i].GetType() == typeof(Archer))
                        {
                            if (player[j].isAlive())
                            {
                                Arrow new_arrow = ((Archer)(artificial[i])).makeArrow(player[j]);
                                new_arrow.LoadContent(content);
                                Vector2 direction = ModelManager.getNormalizedVector(new_arrow.position, player[j].position);
                                new_arrow.setDestination(direction, player[j].position);
                                proj.Add(new_arrow);
                            }
                        }
                        else if (artificial.GetType().BaseType != typeof(Projectile))
                        {
                            artificial[i].Attack(player[j]);
                        }
                       }
                    if(player[j].IsInRange(artificial[i])){
                        if (player[j].GetType() == typeof(Archer))
                        {
                            if (artificial[i].isAlive())
                            {
                                Arrow new_arrow = ((Archer)(player[j])).makeArrow(artificial[i]);
                                new_arrow.LoadContent(content);
                                Vector2 direction = ModelManager.getNormalizedVector(new_arrow.position, artificial[i].position);
                                new_arrow.setDestination(direction, artificial[i].position);
                                proj.Add(new_arrow);
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
