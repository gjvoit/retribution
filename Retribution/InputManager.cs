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
        //MovementManager movementManager;

        public ModelManager modelManager;

        Rectangle mouseRec;
        Vector2 mouseRecOrigin;
        Texture2D myTexture;
        GUIButtons gui;
        Vector2 mousepos;
        public bool buildPhase=true;

        MouseState current;
        MouseState previous;
        int spacing = 1;
        KeyboardState previousKeyboard;
        int default_player_y = 672;
        const double TimerDelay = 500;
        bool selectSimilar = false;
        bool prevclick = true;

        //public InputManager( MovementManager newMovementManager)
        public InputManager(ref ModelManager newmodelManager)
        {
            //movementManager = newMovementManager;
            this.modelManager = newmodelManager;
            mouseRec = Rectangle.Empty;
            mouseRecOrigin = Vector2.Zero;
        }
        public void linkGUI(GUIButtons gwee)
        {
            gui = gwee;
        }
        public void Update(MouseState newcurrent, MouseState newprevious, ref double ClickTimer, KeyboardState keyPress, ref List<GameObject> units, ref List<GameObject> aunits, ref LoadManager loadManager, ref ProjectileManager projMan, 
                            ContentManager theContent, ref int playerResources, bool dbuildPhase)

        {
            bool singleClick = false;
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
            // Select units after releasing mouse and clear rectangle:
            if (current.LeftButton == ButtonState.Released
                && previous.LeftButton == ButtonState.Pressed
                )
            {

                for (int i = 0; i < units.Count; i++)
                {
                    if (!selectSimilar)
                        units[i].selected = false;
                    if (units[i].Bounds.Intersects(mouseRec))
                    {
                        units[i].selected = true;
                    }

                }

                mouseRec = Rectangle.Empty;
            }

            if (current.LeftButton == ButtonState.Pressed && previous.LeftButton == ButtonState.Released && prevclick)
            {
                if (ClickTimer < TimerDelay)
                {
                    selectSimilar = true;
                    Console.WriteLine("double clicked here");
                }
                else
                {
                    ClickTimer = 0;
                    selectSimilar = false;
                }
            }

            // Select with a single mouse click:
            if (current.LeftButton == ButtonState.Pressed
                && previous.LeftButton == ButtonState.Released
                )
            {
                mouseRec = new Rectangle((int)current.X, (int)current.Y, 0, 0);
                mouseRecOrigin = new Vector2(current.X, current.Y);
                GameObject selectedUnit = null;

                for (int i = 0; i < units.Count; i++)
                {
                    units[i].selected = false;
                    if (units[i].isSelectable(current) == true)
                    {
                        units[i].selected = true;
                        selectedUnit = units[i];
                        break;
                    }
                }

                if (selectSimilar && selectedUnit != null)
                {
                    for (int i = 0; i < units.Count; i++)
                    {
                        if (units[i].GetType() == selectedUnit.GetType())
                        {
                            units[i].selected = true;
                        }
                    }
                }

                singleClick = true;
                prevclick = true;
            }


            // Move selected units or attack:
            if (current.RightButton == ButtonState.Pressed
                && previous.RightButton == ButtonState.Released
                )
            {

                Vector2 testvec = new Vector2(current.X, current.Y);
                GameObject selectedTarget = null;

                for (int i = 0; i < aunits.Count; i++)
                {
                    if (aunits[i].isLoaded) {
                        if (aunits[i].isSelectable(current))
                            {
                                selectedTarget = aunits[i];
                                break;
                            }
                    }
                    
                }

                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].selected == true)
                    {
                        units[i].aiTarget = selectedTarget;
                    }
                }
                MovementManager.changeDestination(units, testvec);

            }
            
            Rectangle rect;
            //  Purchase Archer
            gui.buttonCols.TryGetValue("ARCHER", out rect);
            if (!previousKeyboard.IsKeyDown(Keys.Z) && keyPress.IsKeyDown(Keys.Z)||(buttonClick(rect)&&singleClick))
            {
                if (playerResources >= Archer.cost)
                {
                    modelManager.addUnit("PLAYER", "ARCHER", placementUtil());
                    playerResources-=Archer.cost;
                }
            }

            //  Purchase tower
            gui.buttonCols.TryGetValue("TOWER", out rect);
            if (!previousKeyboard.IsKeyDown(Keys.X) && keyPress.IsKeyDown(Keys.X) || (buttonClick(rect)&&singleClick))
            {
                if (playerResources >= Tower.cost)
                {
                    modelManager.addUnit("PLAYER", "TOWER", placementUtil());
                    playerResources -=Tower.cost;
                }
            }

            //  Purchase warrior
            gui.buttonCols.TryGetValue("WARRIOR", out rect);
            if (!previousKeyboard.IsKeyDown(Keys.C) && keyPress.IsKeyDown(Keys.C) || (buttonClick(rect)&&singleClick))
            {
                if (playerResources >= Warrior.cost)
                {
                    modelManager.addUnit("PLAYER", "WARRIOR", placementUtil());
                    playerResources -=Warrior.cost;
                }
            }

            //  Spawn Pawn For Free!!!! Yay
            gui.buttonCols.TryGetValue("PAWN", out rect);
            if (!previousKeyboard.IsKeyDown(Keys.V) && keyPress.IsKeyDown(Keys.V) || (buttonClick(rect)&&singleClick))
            {
                if (playerResources >= Pawn.cost)
                {
                    modelManager.addUnit("PLAYER", "PAWN", placementUtil());
                    playerResources -=Pawn.cost;
                }
            }

            //  Spawn Apprentice For Free!!!! Yay
            gui.buttonCols.TryGetValue("APPRENTICE", out rect);
            if (!previousKeyboard.IsKeyDown(Keys.B) && keyPress.IsKeyDown(Keys.B) || (buttonClick(rect)&&singleClick))
            {
                if (playerResources >= Apprentice.cost)
                {
                    modelManager.addUnit("PLAYER", "APPRENTICE", placementUtil());
                    playerResources -=Apprentice.cost;
                }
            }

            //  Spawn Commander For Free!!!! Yay
            gui.buttonCols.TryGetValue("COMMANDER", out rect);
            if (!previousKeyboard.IsKeyDown(Keys.N) && keyPress.IsKeyDown(Keys.N) || (buttonClick(rect)&&singleClick))
            {
                if (playerResources >= Commander.cost)
                {
                    modelManager.addUnit("PLAYER", "COMMANDER", placementUtil());
                    playerResources -= Commander.cost;
                }
            }

            //  Spawn Catapult For Free!!!! Yay
            gui.buttonCols.TryGetValue("CATAPULT", out rect);
            if (!previousKeyboard.IsKeyDown(Keys.M) && keyPress.IsKeyDown(Keys.M) || (buttonClick(rect)&&singleClick))
            {
                if (playerResources >= Catapult.cost)
                {
                    modelManager.addUnit("PLAYER", "CATAPULT", placementUtil());
                    playerResources -=Catapult.cost;
                }
            }

            //  Spawn Catapult For Free!!!! Yay
            gui.buttonCols.TryGetValue("ROGUE", out rect);
            if (!previousKeyboard.IsKeyDown(Keys.J) && keyPress.IsKeyDown(Keys.J) || (buttonClick(rect)&&singleClick))
            {
                if (playerResources >= Rogue.cost)
                {
                    modelManager.addUnit("PLAYER", "ROGUE", placementUtil());
                    playerResources -= Rogue.cost;
                }
            }
            gui.buttonCols.TryGetValue("CLERIC", out rect);
            if (!previousKeyboard.IsKeyDown(Keys.K) && keyPress.IsKeyDown(Keys.K) || (buttonClick(rect)&&singleClick))
            {
                if (playerResources >= Cleric.cost)
                {
                    modelManager.addUnit("PLAYER", "CLERIC", placementUtil());
                    playerResources -= Cleric.cost;
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
                if (unit.selected && unit.GetType() == typeof(Archer) &&
                    !previousKeyboard.IsKeyDown(Keys.F) && keyPress.IsKeyDown(Keys.F))
                    ((Archer)unit).rapidFire();
                if (unit.selected && unit.GetType() == typeof(Warrior) &&
                    !previousKeyboard.IsKeyDown(Keys.F) && keyPress.IsKeyDown(Keys.F))
                    ((Warrior)unit).juggernaut();
                if (unit.selected && unit.GetType() == typeof(Rogue) &&
                    !previousKeyboard.IsKeyDown(Keys.F) && keyPress.IsKeyDown(Keys.F))
                    ((Rogue)unit).stealth();
                if (unit.selected && unit.GetType() == typeof(Commander) &&
                    !previousKeyboard.IsKeyDown(Keys.F) && keyPress.IsKeyDown(Keys.F))
                    ((Commander)unit).rally();
                //  Skill - Rogue stealth

                //  SKill - etc.
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

         
            previousKeyboard = keyPress;
            //MoraleBar.resourceVal(playerResources);
        }

        public Vector2 placementUtil()
        {
            Vector2 posit;
            if (buildPhase)
                posit = new Vector2(current.X, current.Y);
            else posit = new Vector2(current.X, default_player_y);
            if (posit.X > 1024)
            {
                posit.X = 496;
                posit.Y = default_player_y;
            }
            return posit;

        }
        public bool buttonClick(Rectangle arg)
        {
            return arg.Intersects(mouseRec);
                
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
