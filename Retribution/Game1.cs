#region Using Statements
using System;
using System.Media;
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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Map mainScreen;
        Map levelSelect;
        Boolean testBeta = true;
        // Used for Selector deactivation
        String currMap;
        Selector mainScreenSelector;
        Selector riverDefenseSelector;
        Selector dummyUnlockSelector;
        Map riverDefense;
        MouseState mouseCurrent, mousePrev;
        HealthSystem healthChecker;
        AttackSystem attackChecker;
        InputManager inputManager;
        MovementManager movementManager;
        ProjectileManager projMan;
        ModelManager modMan;
        LoadManager loadMan;
        AIManager aiManager;

        SoundPlayer player;
      //  Warrior theCommander;
        int aiStartDelay;
        int attackDelay;
        int playerResources = 9000;

        //  TyDigit digit test to display amount of resources left
        Digits theResource;


        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 704;
            graphics.PreferredBackBufferWidth = 704;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: Add your initialization logic here
            mainScreen = new Map("Content/MainScreen.txt");
            levelSelect = new Map("Content/levelSelect.txt");
            riverDefense = new Map("Content/RiverDefense.txt");
            mainScreenSelector = new Selector(new Rectangle(288, 0, 128, 64), mainScreen, levelSelect, true);
            riverDefenseSelector = new Selector(new Rectangle(32, 320, 128, 64), levelSelect, riverDefense, true);
            dummyUnlockSelector = new Selector(new Rectangle(288, 320, 128, 64), levelSelect, riverDefense, false);
            modMan = ModelManager.getInstance(ref mainScreen);
            loadMan = LoadManager.getInstance();
            projMan = ProjectileManager.getInstance();
            healthChecker = new HealthSystem(modMan.player, modMan.artificial);
            attackChecker = new AttackSystem(ref modMan.player, ref modMan.artificial);
            attackChecker.linkSystem(projMan);
            attackChecker.linkContent(Content);
            movementManager = MovementManager.getInstance();
            movementManager.setMap(mainScreen);
            inputManager = new InputManager(ref modMan);
            aiStartDelay = 0;
            aiManager = AIManager.getInstance(ref mainScreen);
            mousePrev = Mouse.GetState();
           
           
          
            
            //Create Player's units
            //testInitialization();
            
            
            testCommander();
            base.Initialize();
            this.IsMouseVisible = true;
           
        }

        public void updateMap(Map myMap)
        {
            modMan = ModelManager.getInstance(ref myMap);
            movementManager.setMap(myMap);
            aiManager = AIManager.getInstance(ref myMap);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            loadMan.load(this.Content, modMan.player);
            loadMan.load(this.Content, modMan.artificial);
            player = new System.Media.SoundPlayer("bow.wav");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public void betaInitialization()
        {
            //if (player != null)
            //    player.Play();
            modMan.addUnit("ARTIFICIAL", "TOWER", new Vector2(150, 250));
            modMan.addUnit("ARTIFICIAL", "TOWER", new Vector2(280, 250));
            modMan.addUnit("ARTIFICIAL", "TOWER", new Vector2(672-280, 250));
            modMan.addUnit("ARTIFICIAL", "TOWER", new Vector2(672-150, 250));
            modMan.artificial[0].health = 99999999;
            int toweroffset = 0;
            for (int i = 0; i < 10; i++)
            {
                modMan.addUnit("ARTIFICIAL", "ARCHER", new Vector2(30 + toweroffset, 50));
                modMan.addUnit("ARTIFICIAL", "ARCHER", new Vector2(30 + toweroffset, 190));
               
                //gameobj.Add(new Archer(new Vector2(60 + toweroffset, 100)));
                toweroffset +=70;
            }
            modMan.addUnit("ARTIFICIAL", "WARRIOR", new Vector2(180, 250));
            modMan.addUnit("ARTIFICIAL", "WARRIOR", new Vector2(248, 250));
            modMan.addUnit("ARTIFICIAL", "WARRIOR", new Vector2(672-280+32, 250));
            modMan.addUnit("ARTIFICIAL", "WARRIOR", new Vector2(672-150-32, 250));

            toweroffset = 50;
            for (int i = 0; i < 5; i++)
            {
                modMan.addUnit("PLAYER", "TOWER", new Vector2(20 + toweroffset, 600));
                //gameobj.Add(new Archer(new Vector2(20 + toweroffset, 400)));
                toweroffset += 50;
            }
            toweroffset = 0;
            //towers = new List<Tower>();
            for (int i = 0; i < 10; i++)
            {
                modMan.addUnit("PLAYER", "ARCHER", new Vector2(20 + toweroffset, 550));
                //towers.Add(new Tower(new Vector2(20 + toweroffset, 600)));
                toweroffset += 50;
            }
            modMan.addUnit("PLAYER", "WARRIOR", new Vector2(150, 500));
            modMan.addUnit("PLAYER", "WARRIOR", new Vector2(190, 500));
            modMan.addUnit("PLAYER", "WARRIOR", new Vector2(215, 500));
            modMan.addUnit("PLAYER", "WARRIOR", new Vector2(250, 500));
            modMan.addUnit("PLAYER", "WARRIOR", new Vector2(290, 500));

            theResource = new Digits(new Vector2(0, 672));
            theResource.LoadContent(this.Content);
        }
      


        public void testCommander()
        {
            //theCommander = new Warrior(new Vector2(600, 400));
            modMan.addUnit("PLAYER", "WARRIOR", new Vector2(600, 400));
            ((Mobile)modMan.player[0]).moveSpeed = 3;
        }

        public void testInitialization()
        {
            //  TySoundTest (Must add own filepath here.)
            //System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"c:\Users\TyDang\cs4730retribution\cs4730retribution\Retribution\Content\bow.wav");
            //player.Play();

     if (player!=null)
            player.Play();

            int toweroffset = 50;
            for (int i = 0; i < 5; i++)
            {
                modMan.addUnit("PLAYER", "TOWER", new Vector2(20 + toweroffset, 600));
                //gameobj.Add(new Archer(new Vector2(20 + toweroffset, 400)));
                toweroffset += 50;
            }
            toweroffset = 0;
            //towers = new List<Tower>();
            for (int i = 0; i < 5; i++)
            {
                modMan.addUnit("PLAYER", "ARCHER", new Vector2(20 + toweroffset, 500));
                //towers.Add(new Tower(new Vector2(20 + toweroffset, 600)));
                toweroffset += 50;
            }
            //modMan.player[9].attackRange = 600;
            toweroffset = 0;
            for (int i = 0; i < 5; i++)
            {
                modMan.addUnit("ARTIFICIAL", "TOWER", new Vector2(20 + toweroffset, 20));
                //gameobj.Add(new Archer(new Vector2( 60 + toweroffset , 20)));
                toweroffset += 50;
            }

            toweroffset = 0;
            for (int i = 0; i < 5; i++)
            {
                modMan.addUnit("ARTIFICIAL", "ARCHER", new Vector2(64 + toweroffset, 32*6));
                //gameobj.Add(new Archer(new Vector2(60 + toweroffset, 100)));
                toweroffset += 64;
            }
            modMan.addUnit("PLAYER", "WARRIOR", new Vector2(400,150));
            modMan.addUnit("PLAYER", "WARRIOR", new Vector2(400, 190));
            modMan.addUnit("PLAYER", "WARRIOR", new Vector2(400, 215));
            modMan.addUnit("PLAYER", "WARRIOR", new Vector2(400, 250));
            modMan.addUnit("PLAYER", "WARRIOR", new Vector2(400, 290));

            //  TyDigit added digits object to display resources
            theResource = new Digits(new Vector2(0, 672));
            theResource.LoadContent(this.Content);
        }
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //if (riverDefenseSelector.getOccupied())
            //    betaInitialization();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            mouseCurrent = Mouse.GetState();

            KeyboardState keyboardState = Keyboard.GetState();
            // TODO: Add your update logic here


            //inputManager.Update(mouseCurrent, mousePrev, keyboardState, ref towers, ref gameobj);
            inputManager.Update(mouseCurrent, mousePrev, keyboardState, ref modMan.player, ref loadMan, Content, ref playerResources);
            healthChecker.Update(modMan.player, modMan.artificial);
            healthChecker.checkHealth();
            modMan.player = healthChecker.player;
            modMan.artificial = healthChecker.artificial;
            attackChecker.Update(ref modMan.player, ref modMan.artificial );
            attackChecker.autoAttacks();
            projMan.fireProjectiles();
            movementManager.moveObjects(modMan.player, modMan.artificial);

            aiManager.SetAIDestinations2(modMan.artificial);


            //aiManager.SetAIDestinations(modMan.artificial);

            //  TyDigit: Change the digit based on amount of resources left
            // theResource.ssY = playerResources * 32;

            mousePrev = mouseCurrent;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            // Create a list of Selectors and simply remove/add them depending on the map loaded. Associate selectors with Map.cs
            spriteBatch.Begin();
            if (modMan.player.Count != 0 && riverDefenseSelector.getOccupied() != true)
            {
                mainScreenSelector.isColliding(modMan.player[0]);
            }
                if ((mainScreenSelector.getOccupied() == true) && mainScreenSelector.getInteraction() == true)
                {
                    // GO back and look at this code later, because it's a bit screwy.
                    // mainScreenSelector.setInteraction(false);
                    if (modMan.player.Count != 0)
                    {
                        riverDefenseSelector.isColliding(modMan.player[0]);
                    }
                    if ((riverDefenseSelector.getOccupied() == true) && riverDefenseSelector.getInteraction() == true)
                    {
                        riverDefense.DrawMap(spriteBatch);
                        currMap = "levelSelect";
                        if (currMap.Equals("levelSelect"))
                        {
                            //  TyDigit adding call to draw method of digits
                            if (testBeta)
                            {
                                updateMap(riverDefense);
                                modMan.player.Remove(modMan.player[0]);
                                betaInitialization();
                                loadMan.load(this.Content, modMan.player);
                                loadMan.load(this.Content, modMan.artificial);
                                testBeta = false;
                            }
                            theResource.ssY = playerResources * 32;
                            theResource.Draw(spriteBatch);
                        }
                        //currMap = "riverDefense";
                    }
                    else if ((dummyUnlockSelector.getOccupied() == true) && dummyUnlockSelector.getInteraction() == true)
                    {
                        updateMap(riverDefense);
                        riverDefense.DrawMap(spriteBatch);
                    }
                    else
                    {
                        updateMap(levelSelect);
                        levelSelect.DrawMap(spriteBatch);
                    }

                }
                else mainScreen.DrawMap(spriteBatch);
            foreach(Projectile item in projMan.proj)
            {
                ((Arrow)item).Draw(spriteBatch);
            }
            for (int i = 0; i < modMan.player.Count; i++)
            {
                (modMan.player[i]).Draw(spriteBatch);
            }
           

            for (int i = 0; i < modMan.artificial.Count; i++)
            {
                modMan.artificial[i].Draw(spriteBatch);
            }

            inputManager.DrawMouseRectangle(spriteBatch, Content);

            spriteBatch.End();

            

            base.Draw(gameTime);
        }

      
    }
}
