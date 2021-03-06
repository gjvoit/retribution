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

        public ModelManager modelManager;
        static Vector2 dummy = new Vector2(0,0);
        Archer arc=new Archer(dummy);
        Apprentice app= new Apprentice(dummy);
        Catapult cat=new Catapult(dummy);
        Cleric cle=new Cleric(dummy);
        Commander com= new Commander(dummy);
        Pawn paw= new Pawn(dummy);
        Rogue rog= new Rogue(dummy);
        Tower tow= new Tower(dummy);
        Warrior war= new Warrior(dummy);
        bool loaded = false;
        Rectangle mouseRec;
        Vector2 mouseRecOrigin;
        Texture2D myTexture;
        GUIButtons gui;
        Vector2 mousepos;
        public bool buildPhase=true;
        ContentManager content;
        MouseState current;
        MouseState previous;
        int spacing = 1;
        KeyboardState previousKeyboard;
        int default_player_y = 640;
        const double TimerDelay = 500;
        bool selectSimilar = false;
        bool prevclick = true;

        int offset = 0;


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
        public void loadMouseOvers(ContentManager cont)
        {

            arc.LoadContent(cont);
            app.LoadContent(cont);
            cle.LoadContent(cont);
            paw.LoadContent(cont);
            cat.LoadContent(cont);
            com.LoadContent(cont);
            tow.LoadContent(cont);
            war.LoadContent(cont);
            rog.LoadContent(cont);
        }
        public void Update(MouseState newcurrent, MouseState newprevious, ref double ClickTimer, KeyboardState keyPress,
            ref Dictionary<Keys, List<GameObject>> groupedUnits, ref List<GameObject> units, ref List<GameObject> aunits,
            ref LoadManager loadManager, ref ProjectileManager projMan, ContentManager theContent, ref int playerResources, bool dbuildPhase)

        {
           
            Rectangle holder = mouseRec;
            mouseRec = new Rectangle(newcurrent.X, newcurrent.Y, 2, 2);
            if (!loaded)
            {
                loadMouseOvers(theContent);
                loaded = true;
            }
            mouseOver();
            mouseRec = holder;
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
                    if (unit.selected&&unit.GetType()==typeof(Tower))
                    {
                        if (!((Tower)unit).placed)
                        {
                            ((Tower)unit).placed = true;
                            unit.position = mousePosition;
                        }
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
                        InfoCard.info(units[i]);
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

            if (keyPress.GetPressedKeys().Length > 0 && IsKeyADigit(keyPress.GetPressedKeys()[0]))
            {
                Keys pressedKey = keyPress.GetPressedKeys()[0];
                if (previousKeyboard.IsKeyDown(Keys.LeftControl) && keyPress.IsKeyDown(pressedKey))
                {
                    List<GameObject> groupList = new List<GameObject>();

                    foreach (GameObject gobj in units)
                    {
                        if (gobj.selected)
                        {
                            groupList.Add(gobj);
                        }
                    }

                    if (groupList.Count > 0)
                    {
                        if (!groupedUnits.ContainsKey(pressedKey))
                            groupedUnits.Add(pressedKey, groupList);
                        else
                            groupedUnits[pressedKey] = groupList;
                    }
                }

            
                if (!previousKeyboard.IsKeyDown(pressedKey) && keyPress.IsKeyDown(pressedKey))
                {
                    if (groupedUnits.ContainsKey(pressedKey))
                    {
                        deselect(ref groupedUnits, pressedKey);
                        List<GameObject> groupList = groupedUnits[pressedKey];
                        foreach (GameObject gobj in groupList)
                            gobj.selected = true;
                    }

                }
            }

            if (!previousKeyboard.IsKeyDown(Keys.Delete) && keyPress.IsKeyDown(Keys.Delete) && units.Count > 0)
            {
                if (dbuildPhase) // complete refund
                {
                    int refund = getCost(units[units.Count - 1]);
                    playerResources += refund;
                }
                if (units.Count == 1)
                {
                    if (units[0].GetType() == typeof(Warrior))
                    {
                        if (((Warrior)(units[0])).moveSpeed == 7)
                        {
                            return;
                        }
                    }
                }
                units.RemoveAt(units.Count - 1);
                offset -= 50;
                if (offset < 0 && default_player_y > 608)
                {
                    offset = 300;
                    default_player_y -= 32;
                }

                // what to do when not in build phase? partial refund? currently can't undo build in play phase
                
            }

            if (!previousKeyboard.IsKeyDown(Keys.Back) && keyPress.IsKeyDown(Keys.Back))
            {
                foreach (GameObject gobj in units)
                {
                    if (gobj.selected)
                    {
                        if (gobj.GetType() == typeof(Warrior))
                        {
                            if (((Warrior)(gobj)).moveSpeed == 7)
                                return;
                        }
                        int refund = getCost(gobj);
                        playerResources += refund;
                    }
                }
                units.RemoveAll(gobj => gobj.selected);
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
                if (unit.selected && unit.GetType() == typeof(Tower) &&
                    !previousKeyboard.IsKeyDown(Keys.F) && keyPress.IsKeyDown(Keys.F))
                    ((Tower)unit).entrench();
                if (unit.selected && unit.GetType() == typeof(BossUnit) &&
                    !previousKeyboard.IsKeyDown(Keys.F) && keyPress.IsKeyDown(Keys.F))
                    ((BossUnit)unit).pound();
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
            int currentx = current.X;
            int currenty = current.Y;
            if (currenty < 450)
                currenty = 450;
            if (currenty > 650)
                currenty = 650;
            if (currentx < 50)
                currentx = 50;
            if (currentx > 950)
                currentx = 950;
            if (currentx >= 950 && currenty <= 450)
            {
                currentx = offset;
                currenty = default_player_y;
            }

            if (buildPhase)
            {
                posit = new Vector2(currentx, currenty);
                if (offset >= 300)
                {
                    offset = 0;
                    //default_player_y += 32;
                }
                else offset += 50;
            }
            else
            {
                posit = new Vector2(currentx, default_player_y);
                if (offset >= 600)
                {
                    offset = 0;
                    //default_player_y += 32;
                }
                else offset += 50;
            }
            //if (posit.X > 450)
            //{
            //    posit.X = 450;
            //    posit.Y = default_player_y;
            //}
            return posit;

        }
        public void mouseOver()
        {
            
            Rectangle rect;
            gui.buttonCols.TryGetValue("ARCHER", out rect);
            if (buttonClick(rect))
                InfoCard.info(arc);
            gui.buttonCols.TryGetValue("APPRENTICE", out rect);
            if (buttonClick(rect))
                InfoCard.info(app);
            gui.buttonCols.TryGetValue("CATAPULT", out rect);
            if (buttonClick(rect))
                InfoCard.info(cat);
            gui.buttonCols.TryGetValue("COMMANDER", out rect);
            if (buttonClick(rect))
                InfoCard.info(com);
            gui.buttonCols.TryGetValue("CLERIC", out rect);
            if (buttonClick(rect))
                InfoCard.info(cle);
            gui.buttonCols.TryGetValue("PAWN", out rect);
            if (buttonClick(rect))
                InfoCard.info(paw);
            gui.buttonCols.TryGetValue("ROGUE", out rect);
            if (buttonClick(rect))
                InfoCard.info(rog);
            gui.buttonCols.TryGetValue("TOWER", out rect);
            if (buttonClick(rect))
                InfoCard.info(tow);
            gui.buttonCols.TryGetValue("WARRIOR", out rect);
            if (buttonClick(rect))
                InfoCard.info(war);
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

        public static bool IsKeyADigit(Keys key)
        {
            return (key >= Keys.D0 && key <= Keys.D9) || (key >= Keys.NumPad0 && key <= Keys.NumPad9);
        }

        public void deselect(ref Dictionary<Keys, List<GameObject>> groupedUnits, Keys pressedKey) 
        {
            foreach (KeyValuePair<Keys, List<GameObject>> pair in groupedUnits)
            {
                if (pair.Key != pressedKey)
                {
                    foreach (GameObject gobj in pair.Value)
                    {
                        gobj.selected = false;
                    }
                }
            }
        }

        public int getCost(GameObject gobj)
        {
            switch (gobj.GetType().Name.ToString())
            {
                case "Archer":
                    return Archer.cost;
                case "Tower":
                    return Tower.cost;
                case "Warrior":
                    return Warrior.cost;
                case "PAWN":
                    return Pawn.cost;
                case "APPRENTICE":
                    return Apprentice.cost;
                case "COMMANDER":
                    return Commander.cost;
                case "CATAPULT":
                    return Catapult.cost;
                case "ROGUE":
                    return Rogue.cost;
                case "CLERIC":
                    return Cleric.cost;
            }

            return 0;
        }
    }
}
