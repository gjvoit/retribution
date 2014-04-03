using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Retribution
{
    class MovementManager
    {
        static MovementManager instance;
        private MovementManager()
        {
            //  Do Nothing
        }
        public MovementManager getInstance()
        {
            if (instance == null)
                instance = new MovementManager();
            else
                return instance;
        }

        //  Helper method to calculate normalized vector
        public static Vector2 getNormalizedVector(Vector2 startVector, Vector2 endVector)
        {
            Vector2 moveVector = Vector2.Subtract(endVector, startVector);
            Vector2 normalizedVector = Vector2.Normalize(moveVector);
            return normalizedVector;
        }

        //  Call movement method of all selected objects
        public static void moveObjects(List<Mobile> listOfSelectedObjects)
        {
            for (int i = 0; i < listOfSelectedObjects.Count; i++)
            {
                if (listOfSelectedObjects[i].selected == true || listOfSelectedObjects[i].isMoving == true)
                {
                    listOfSelectedObjects[i].move();
                }
            }
        }

        public static void changeDestination(List<Mobile> listOfSelectedObjects, Vector2 destination)
        {
            for (int i = 0; i < listOfSelectedObjects.Count; i++)
            {
                if (listOfSelectedObjects[i].selected == true)
                {
                    listOfSelectedObjects[i].setDestination(getNormalizedVector(listOfSelectedObjects[i].getPosition(), destination), destination);
                    listOfSelectedObjects[i].isMoving = true;
                }
            }
        }
    }
}
