﻿#region Using Statements
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
        //MovementManager movementManager;

        ModelManager modelManager;

        Rectangle mouseRec;
        Vector2 mouseRecOrigin;
        Texture2D myTexture;
        int spacing = 1;
        KeyboardState previousKeyboard;

        //public InputManager( MovementManager newMovementManager)
        public InputManager(ref ModelManager newmodelManager)
        {
            //movementManager = newMovementManager;
            modelManager = newmodelManager;
            mouseRec = Rectangle.Empty;
            mouseRecOrigin = Vector2.Zero;
        }

        public void Update(MouseState current, MouseState previous, KeyboardState keyPress, ref List<GameObject> units, ref LoadManager loadManager, ContentManager theContent,
                            ref int playerResources)

        {

            if (keyPress.IsKeyDown(Keys.S)&&current.LeftButton==ButtonState.Pressed&&previous.LeftButton==ButtonState.Released)
            {
                 int x = Convert.ToInt32(current.X);
                 int y = Convert.ToInt32(current.Y);
                Vector2 mousePosition = new Vector2(x,y);
                ClickBox temp = new ClickBox(mousePosition);
                temp.LoadContent(theContent);
                foreach (GameObject unit in units)
                {
                    if (unit.selected)
                    {
                        unit.position = mousePosition;                       
                        //unit.position = mousePosition;
                    }
                }
            }

            //  Purchase Archer
            if (!previousKeyboard.IsKeyDown(Keys.Z) && keyPress.IsKeyDown(Keys.Z))
            {
                if (playerResources >= 1)
                {
                    Archer temp = new Archer(new Vector2(current.X, 672));
                    units.Add(temp);
                    loadManager.load(theContent, units);
                    playerResources--;
                }
            }

            //  Purchase tower
            if (!previousKeyboard.IsKeyDown(Keys.X) && keyPress.IsKeyDown(Keys.X))
            {
                if (playerResources >= 2)
                {
                    Tower temp = new Tower(new Vector2(current.X, 672));
                    units.Add(temp);
                    loadManager.load(theContent, units);
                    playerResources -= 2;
                }
            }

            //  Purchase warrior
            if (!previousKeyboard.IsKeyDown(Keys.C) && keyPress.IsKeyDown(Keys.C))
            {
                if (playerResources >= 5)
                {
                    Warrior temp = new Warrior(new Vector2(current.X, 672));
                    units.Add(temp);
                    loadManager.load(theContent, units);
                    playerResources -= 5;
                }
            }

            // Select with a single mouse click:
            if (current.LeftButton == ButtonState.Pressed
                && previous.LeftButton == ButtonState.Released
                )
            {

                mouseRec = new Rectangle((int) current.X, (int)current.Y, 0, 0);
                mouseRecOrigin = new Vector2(current.X, current.Y);

                for (int i = 0; i < units.Count; i++)
                {

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

            // Select units after releasing mouse and clear rectangle:
            if (current.LeftButton == ButtonState.Released
                && previous.LeftButton == ButtonState.Pressed
                )
            {
                for (int i = 0; i < units.Count; i++)
                {
                    units[i].selected = false;
                    if (units[i].Bounds.Intersects(mouseRec))
                    {
                        units[i].selected = true;
                    }
                }

                mouseRec = Rectangle.Empty;
            }


            // Move selected units or attack:
            if (current.RightButton == ButtonState.Pressed
                && previous.RightButton == ButtonState.Released
                )
            {

                Vector2 testvec = new Vector2(current.X, current.Y);

                //  How to handle attacking.....?

                /*for (int i = 0; i < towers.Count; i++)
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
                }*/

                //movementManager.changeDestination(units, testvec);
                //for (int i = 0; i < towers.Count; i++)
                //{
                //    if(towers[i].Bounds.Contains((int)testvec.X, (int)testvec.Y) ){
                //        for (int j = 0; j < units.Count; j++)
                //        {
                //            if(units[j].selected == true && units[j].IsInRange(towers[i]))
                //            {
                //                //units[j].
                //                units[j].Attack(towers[i]);
                //            }

                //        }
                //    }
                //}

                //MovementManager.changeDestination(units, testvec);
                MovementManager.changeDestination(units, testvec);

            }

            previousKeyboard = keyPress;

        }
        

        public void DrawMouseRectangle(SpriteBatch spriteBatch, ContentManager content)
        {
            if (myTexture == null)
            {
                myTexture = content.Load<Texture2D>("FFFFFF-0.8.png");
            }

           // spriteBatch.Begin();

            Rectangle dstRec = new Rectangle(mouseRec.X, (int)mouseRec.Y, 1, 1);
            Color color = Color.WhiteSmoke;

            if (mouseRec.Width > 0)
            {
                int x = mouseRec.Width / spacing;
                for (int i = 0; i <= x; i++)
                {
                    //draw horizontal top
                    spriteBatch.Draw(myTexture, dstRec, new Rectangle(0, 0, myTexture.Width, myTexture.Height), color);

                    if (mouseRec.Height > 0)
                    {
                        //draw horizontal bottom
                        dstRec.Y += mouseRec.Height;
                        spriteBatch.Draw(myTexture, dstRec, new Rectangle(0, 0, myTexture.Width, myTexture.Height), color);
                        dstRec.Y -= mouseRec.Height;
                    }

                    //advance
                    dstRec.X += spacing;
                }
            }

            if (mouseRec.Height > 0)
            {
                dstRec.X = mouseRec.X;
                int Y = mouseRec.Height / spacing;
                for (int i = 0; i <= Y; i++)
                {
                    //draw vertical right
                    spriteBatch.Draw(myTexture, dstRec, new Rectangle(0, 0, myTexture.Width, myTexture.Height), color);

                    if(mouseRec.Width > 0)
                    {
                        //draw vertical left
                        dstRec.X += mouseRec.Width;
                        spriteBatch.Draw(myTexture, dstRec, new Rectangle(0, 0, myTexture.Width, myTexture.Height), color);
                        dstRec.X -= mouseRec.Width;
                    }

                    dstRec.Y += spacing;
                }
            }

           // spriteBatch.End();

        }
    }
}
