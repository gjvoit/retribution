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
    class InputManager
    {
        MovementManager movementManager;
        Rectangle mouseRec;
        Vector2 mouseRecOrigin;

        public InputManager( MovementManager newMovementManager)
        {
            movementManager = newMovementManager;
            mouseRec = Rectangle.Empty;
            mouseRecOrigin = Vector2.Zero;
        }

        public void Update(MouseState current, MouseState previous, KeyboardState keyPress, ref List<Tower> towers, ref List<GameObject> units)
        {
            // Select with a single mouse click:
            if (current.LeftButton == ButtonState.Pressed
                && previous.LeftButton == ButtonState.Released
                )
            {

                mouseRec = new Rectangle((int) current.X, (int)current.Y, 0, 0);
                mouseRecOrigin = new Vector2(current.X, current.Y);

                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].selected == true)
                        units[i].selected = false;
                    if (units[i].isSelectable(current) == true)
                    {
                        units[i].selected = true;
                    }
                }
            }

            //  Update mouse rectangle:
            if (current.LeftButton == ButtonState.Pressed
                && previous.LeftButton == ButtonState.Pressed
                )
            {
                if (current.X > mouseRecOrigin.X)
                {
                    mouseRec.Width = current.X - mouseRec.X;
                }
                else
                {
                    mouseRec.Width = (int)mouseRecOrigin.X - current.X;
                    mouseRec.X = current.X;
                }

                if (current.Y > mouseRecOrigin.Y)
                {
                    mouseRec.Height= current.Y - mouseRec.Y;
                }
                else
                {
                    mouseRec.Height = (int)mouseRecOrigin.Y - current.Y;
                    mouseRec.Y = current.Y;
                }

            }

            // Select units aftere releasing mouse:
            if (current.LeftButton == ButtonState.Released
                && previous.LeftButton == ButtonState.Pressed
                )
            {
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].selected == true)
                        units[i].selected = false;
                    if (units[i].Bounds.Intersects(mouseRec))
                    {
                        units[i].selected = true;
                    }
                }
            }


            // Move selected units or attack:
            if (current.RightButton == ButtonState.Pressed
                && previous.RightButton == ButtonState.Pressed
                )
            {

                Vector2 testvec = new Vector2(current.X, current.Y);

                //  How to handle attacking.....?
                for (int i = 0; i < towers.Count; i++)
                {
                    if(towers[i].Bounds.Contains((int)testvec.X, (int)testvec.Y) ){
                        for (int j = 0; j < units.Count; j++)
                        {
                            if(units[j].selected == true && units[j].IsInRange(towers[i]))
                            {
                                //units[j].
                                units[j].Attack(towers[i]);
                            }

                        }
                    }
                }

                MovementManager.changeDestination(units, testvec);

            }

        }
    }
}
