#region Using Statements
using System;
using System.IO;
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
    class UnitGroup
    {
        Vector2 destination;
        public List<Mobile> units;
        public Mobile leader;
        int urCounter, brCounter, blCounter, ulCounter;

        public UnitGroup(List<Mobile> unitList, Vector2 newDest)
        {
            units = unitList;
            destination = newDest;
            urCounter = 0;
            brCounter = 0;
            blCounter = 0;
            ulCounter = 0;

            // Leader of group is the unit closest to the destination:
            float distance = 999999;
            foreach(Mobile unit in units)
            {
                float testDistance = Vector2.Distance(new Vector2(unit.Bounds.Center.X, unit.Bounds.Center.Y), new Vector2(destination.X, destination.Y));
                if (testDistance < distance)
                {
                    leader = unit;
                }
            }
        }

        public void SetPaths(Map myMap)
        {
            List<Tile> newCollisionList = new List<Tile>();

            leader.setDestination(destination);
            foreach(Mobile unit in units)
            {
                unit.isMoving = false;
                if(unit != leader)
                {
                    Vector2 newDestination = GetOffset(unit, myMap) + destination;
                    if(myMap.GetTile(newDestination) != null && myMap.GetTile(newDestination).isWalkable == true)
                    {
                        unit.setDestination(GetOffset(unit, myMap) + destination);
                    }
                    else
                        unit.setDestination(destination);
                }
                //unit.setDestination(destination);
                Vector2 startPoint = new Vector2(unit.Bounds.Center.X, unit.Bounds.Center.Y);
                unit.pathList.Clear();
                unit.pathList.AddRange(myMap.GetPath(startPoint, unit.destination, newCollisionList));
                unit.isMoving = true;
            }

        }

        public Boolean Contains(Mobile unit)
        {
            if (units.Contains(unit))
                return true;
            else
                return false;
        }

        public Vector2 GetOffset(Mobile unit, Map myMap)
        {
            //  Save tiles for leader and unit position:
            Tile leaderTile = myMap.GetContainingTile(leader);
            Tile unitTile = myMap.GetContainingTile(unit);

            if (leaderTile == null || unitTile == null)
                return new Vector2(0, -0);

            // Upper left
            if (unitTile.xPosition < leaderTile.xPosition && unitTile.yPosition <= leaderTile.yPosition)
            {
                ulCounter++;
                if(ulCounter == 1)
                    return new Vector2(-32, 0);
                else
                    return new Vector2(-32, -32);
            }

            //  Upper right
            if (unitTile.xPosition >= leaderTile.xPosition && unitTile.yPosition < leaderTile.yPosition)
            {
                urCounter++;
                if (urCounter == 1)
                    return new Vector2(0, -32);
                else
                    return new Vector2(32, -32);
            }

            //  Bottom right
            if (unitTile.xPosition > leaderTile.xPosition && unitTile.yPosition >= leaderTile.yPosition)
            {
                brCounter++;
                if(brCounter == 1)
                    return new Vector2(32, 0);
                else
                    return new Vector2(32, 32);
            }

            // Bottom left
            if (unitTile.xPosition <= leaderTile.xPosition && unitTile.yPosition > leaderTile.yPosition)
            {
                blCounter++;
                if (blCounter == 1)
                    return new Vector2(32, 0);
                else
                    return new Vector2(-32, 32);
            }

            else
                return new Vector2(0, 0);
        }

    }
}
