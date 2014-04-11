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
        Vector2 mousepos;
        public bool buildPhase=true;
        MouseState current;
        MouseState previous;
        int spacing = 1;
        KeyboardState previousKeyboard;
        int default_player_y = 672;

        //public InputManager( MovementManager newMovementManager)
        public InputManager(ref ModelManager newmodelManager)
        {
            //movementManager = newMovementManager;
            modelManager = newmodelManager;
            mouseRec = Rectangle.Empty;
            mouseRecOrigin = Vector2.Zero;
        }

        public void Update(MouseState newcurrent, MouseState newprevious, KeyboardState keyPress, ref List<GameObject> units, ref LoadManager loadManager, ref ProjectileManager projMan, 
                            ContentManager theContent, ref int playerResources, bool dbuildPhase)

        {
            current = newcurrent;
            previous = newprevious;
            buildPhase = dbuildPhase;
            if (keyPress.IsKeyDown(Keys.S)&&current.LeftButton==ButtonState.Pressed&&previous.LeftButton==ButtonState.Released)
            {
                 int x = Convert.ToInt32(current.X);
                 int y = Convert.ToInt32(current.Y);
                Vector2 mousePosition = new Vector2(x,y);
                ClickBox temp = new ClickBox(mousePosition);
                //temp.LoadContent(theContent);
                foreach (GameObject unit in units)
                {
                    if (unit.selected)
                    {
                        unit.position = mousePosition;
                    }
                }
            }

            
            

            //  Purchase Archer
            if (!previousKeyboard.IsKeyDown(Keys.Z) && keyPress.IsKeyDown(Keys.Z))
            {
                if (playerResources >= 1)
                {
                    modelManager.addUnit("PLAYER", "ARCHER", placementUtil());
                    playerResources--;
                }
            }

            //  Purchase tower
            if (!previousKeyboard.IsKeyDown(Keys.X) && keyPress.IsKeyDown(Keys.X))
            {
                if (playerResources >= 2)
                {
                    modelManager.addUnit("PLAYER", "TOWER", placementUtil());
                    playerResources -= 2;
                }
            }

            //  Purchase warrior
            if (!previousKeyboard.IsKeyDown(Keys.C) && keyPress.IsKeyDown(Keys.C))
            {
                if (playerResources >= 5)
                {
                    modelManager.addUnit("PLAYER", "WARRIOR", placementUtil());
                    playerResources -= 5;
                }
            }

            //  Spawn Pawn For Free!!!! Yay
            if (!previousKeyboard.IsKeyDown(Keys.V) && keyPress.IsKeyDown(Keys.V))
            {
                if (playerResources >= 0)
                {
                    modelManager.addUnit("PLAYER", "PAWN", placementUtil());
                    playerResources -= 0;
                }
            }

            //  Spawn Apprentice For Free!!!! Yay
            if (!previousKeyboard.IsKeyDown(Keys.B) && keyPress.IsKeyDown(Keys.B))
            {
                if (playerResources >= 0)
                {
                    modelManager.addUnit("PLAYER", "APPRENTICE", placementUtil());
                    playerResources -= 0;
                }
            }

            //  Spawn Commander For Free!!!! Yay
            if (!previousKeyboard.IsKeyDown(Keys.N) && keyPress.IsKeyDown(Keys.N))
            {
                if (playerResources >= 0)
                {
                    modelManager.addUnit("PLAYER", "COMMANDER", placementUtil());
                    playerResources -= 0;
                }
            }

            //  Spawn Catapult For Free!!!! Yay
            if (!previousKeyboard.IsKeyDown(Keys.M) && keyPress.IsKeyDown(Keys.M))
            {
                if (playerResources >= 0)
                {
                    modelManager.addUnit("PLAYER", "CATAPULT", placementUtil());
                    playerResources -= 0;
                }
            }

            //  Spawn Catapult For Free!!!! Yay
            if (!previousKeyboard.IsKeyDown(Keys.J) && keyPress.IsKeyDown(Keys.J))
            {
                if (playerResources >= 0)
                {
                    modelManager.addUnit("PLAYER", "ROGUE", placementUtil());
                    playerResources -= 0;
                }
            }
            if (!previousKeyboard.IsKeyDown(Keys.K) && keyPress.IsKeyDown(Keys.K))
            {
                if (playerResources >= 0)
                {
                    modelManager.addUnit("PLAYER", "CLERIC", placementUtil());
                    playerResources -= 0;
                }
            }

            //  Skill - When F is pressed, units selected will perform skill
            foreach (GameObject unit in units)
            {
                //  Skill - Apprentice Fireball
                if (unit.selected && unit.GetType() == typeof(Apprentice) &&
                    !previousKeyboard.IsKeyDown(Keys.F) && keyPress.IsKeyDown(Keys.F))
                {
                    ((Apprentice)unit).fireball(projMan, theContent, unit, new Vector2(current.X, current.Y));
                }

                //  Skill - Rogue stealth

                //  SKill - etc.
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

                MovementManager.changeDestination(units, testvec);

            }

            previousKeyboard = keyPress;

        }

        public Vector2 placementUtil()
        {
            Vector2 posit;
            if (buildPhase)
                posit = new Vector2(current.X, current.Y);
            else posit = new Vector2(current.X, default_player_y);
            return posit;

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
