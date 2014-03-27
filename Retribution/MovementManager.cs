using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Retribution
{
    class MovementManager
    {
        public MovementManager()
        {
            //  Do Nothing
        }

        //  Helper method to calculate normalized vector
        public static Vector2 getNormalizedVector(Vector2 startVector, Vector2 endVector)
        {
            Vector2 moveVector = Vector2.Subtract(endVector, startVector);
            Vector2 normalizedVector = Vector2.Normalize(moveVector);
            return normalizedVector;
        }

        //  Call movement method of all selected objects
        public static void moveObjects(List<GameObject> listOfSelectedObjects)
        {
            for (int i = 0; i < listOfSelectedObjects.Count; i++)
            {
                if (listOfSelectedObjects[i].selected == true)
                {
                    listOfSelectedObjects[i].move();
                }
            }
        }

        public static void changeDestination(List<GameObject> listOfSelectedObjects, Vector2 destination)
        {
            for (int i = 0; i < listOfSelectedObjects.Count; i++)
            {
                if (listOfSelectedObjects[i].selected == true)
                {
                    listOfSelectedObjects[i].setDestination(getNormalizedVector(listOfSelectedObjects[i].getPosition(), destination), destination);
                }
            }
        }
    }
}
