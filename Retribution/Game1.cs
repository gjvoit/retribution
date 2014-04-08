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
        Selector castleDefenseSelector;
        Selector castleSiegeSelector;
        Map riverDefense;
        Map castleDefense;
        Map castleSiege;
        MouseState mouseCurrent, mousePrev;
        HealthSystem healthChecker;
        AttackSystem attackChecker;
        InputManager inputManager;
        MovementManager movementManager;
        ProjectileManager projMan;
        ModelManager modMan;
        LoadManager loadMan;
        AIManager aiManager;
        ScreenManager screenManager;

        SoundPlayer player;
      //  Warrior theCommander;
        int aiStartDelay;
        int attackDelay;
        int playerResources = 9000;
        int buildResources = 10;
        // if built is false, player enters build phase; if built is true, that means player finished build phase and level starts
        bool built = false;
        // if initialized is false, that means AI units have not been initialized
        bool initialized = false;
        bool playable = false;
        int noresource = 0;

        //  TyDigit digit test to display amount of resources left
        Digits theResource;
        titleShell theTitle;

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
            riverDefenseSelector = new Selector(new Rectangle(288, 320, 128, 64), levelSelect, riverDefense, true);
            castleDefenseSelector = new Selector(new Rectangle(32, 320, 128, 64), levelSelect, castleDefense, false);
            castleSiegeSelector = new Selector(new Rectangle(544, 320, 128, 64), levelSelect, castleSiege, false);
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
            modMan.artificial[0].health = 5;
            int toweroffset = 0;
            for (int i = 0; i < 10; i++)
            {
                modMan.addUnit("ARTIFICIAL", "ARCHER", new Vector2(30 + toweroffset, 50));
                modMan.addUnit("ARTIFICIAL", "ARCHER", new Vector2(30 + toweroffset, 190));
                modMan.artificial[5].health = 10000;
                modMan.artificial[5].damage = 1000;
               
                //gameobj.Add(new Archer(new Vector2(60 + toweroffset, 100)));
                toweroffset +=70;
            }
            modMan.addUnit("ARTIFICIAL", "WARRIOR", new Vector2(180, 250));
            modMan.addUnit("ARTIFICIAL", "WARRIOR", new Vector2(248, 250));
            modMan.addUnit("ARTIFICIAL", "WARRIOR", new Vector2(672-280+32, 250));
            modMan.addUnit("ARTIFICIAL", "WARRIOR", new Vector2(672-150-32, 250));

            toweroffset = 50;
            /* This part is commented out because we want the player to spawn own units, not auto-generated anymore */
            //toweroffset = 50;
            //for (int i = 0; i < 5; i++)
            //{
            //    modMan.addUnit("PLAYER", "TOWER", new Vector2(20 + toweroffset, 600));
            //    //gameobj.Add(new Archer(new Vector2(20 + toweroffset, 400)));
            //    toweroffset += 50;
            //}
            //toweroffset = 0;
            ////towers = new List<Tower>();
            //for (int i = 0; i < 10; i++)
            //{
            //    modMan.addUnit("PLAYER", "ARCHER", new Vector2(20 + toweroffset, 550));
            //    //towers.Add(new Tower(new Vector2(20 + toweroffset, 600)));
            //    toweroffset += 50;
            //}
            //modMan.addUnit("PLAYER", "WARRIOR", new Vector2(150, 500));
            //modMan.addUnit("PLAYER", "WARRIOR", new Vector2(190, 500));
            //modMan.addUnit("PLAYER", "WARRIOR", new Vector2(215, 500));
            //modMan.addUnit("PLAYER", "WARRIOR", new Vector2(250, 500));
            //modMan.addUnit("PLAYER", "WARRIOR", new Vector2(290, 500));
            /* ---------------------------------------------------------------------------------------------------- */
            theResource = new Digits(new Vector2(0, 672));
            theTitle = new titleShell(new Vector2(375, 375));
            theResource.LoadContent(this.Content);
            theTitle.LoadContent(this.Content);
        }
      


        public void testCommander()
        {
            //theCommander = new Warrior(new Vector2(600, 400));
            modMan.addUnit("PLAYER", "WARRIOR", new Vector2(400, 600));
            ((Mobile)modMan.player[0]).moveSpeed = 7;
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
            /*
            for (int i = 0; i < 5; i++)
            {
                modMan.addUnit("PLAYER", "ARCHER", new Vector2(20 + toweroffset, 500));
                //towers.Add(new Tower(new Vector2(20 + toweroffset, 600)));
                toweroffset += 50;
            }
             */
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
            theTitle = new titleShell(new Vector2(375, 375));
            theResource.LoadContent(this.Content);
            theTitle.LoadContent(this.Content);
        }

        // Change map and set managers to be equal to the current map
        public void updateMap(Map myMap)
        {
            modMan = ModelManager.getInstance(ref myMap);
            movementManager.setMap(myMap);
            aiManager = AIManager.getInstance(ref myMap);
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

            //inputManager.Update(mouseCurrent, mousePrev, keyboardState, ref towers, ref gameobj);

            if (!built && playable) // Enter build phase if built is False, notice true flag at the end indicating we're in build phase
            {
                inputManager.Update(mouseCurrent, mousePrev, keyboardState, ref modMan.player, ref loadMan, this.Content, ref buildResources, true);
                if (buildResources <= 1) // once we deplete our build resources, set built to true (doing so will initialize enemy AI units and starts the level)
                    built = true;
            }
            else if (built && initialized)// player is not building in build phase but rather building reinforcements - notice the false flag at the end indicating not build phase
            {
                inputManager.Update(mouseCurrent, mousePrev, keyboardState, ref modMan.player, ref loadMan, Content, ref playerResources, false);
            }
            else
            {
                inputManager.Update(mouseCurrent, mousePrev, keyboardState, ref modMan.player, ref loadMan, Content, ref noresource, false);
            }
            if (built && !initialized && riverDefenseSelector.getOccupied() == true)
            {
                betaInitialization();
                initialized = true; // set initialized to true to prevent looped enemy unit spawning
            }
            
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
                    // Draw riverDefense until an end condition has been met:
                    // Either player has won or lost
                    // Case that player loses, return to level select and 
                    riverDefense.DrawMap(spriteBatch);
                    playable = true;
                    loadMan.load(this.Content, modMan.player);
                    loadMan.load(this.Content, modMan.artificial);
                    currMap = "levelSelect";
                    if (currMap.Equals("levelSelect"))
                    {
                        //  TyDigit adding call to draw method of digits
                        if (testBeta)
                        {
                            updateMap(riverDefense);
                            modMan.player.Remove(modMan.player[0]);
                            loadMan.load(this.Content, modMan.player);
                            loadMan.load(this.Content, modMan.artificial);
                            //riverDefenseSelector.setOccupied(false);
                            testBeta = false;
                            riverDefenseSelector.setInteraction(true);
                        }
                        if (built && initialized)  // hopefully get digits working and draw it to the screen (only when game starts)
                        {
                            theResource.ssY = playerResources * 32;
                            theResource.Draw(spriteBatch);
                        }
                    }
                    // You lost! This happens, and takes you back to levelSelect to replay!
                    if ((modMan.player.Count == 0) && built)
                    {
                        modMan.artificial.Clear();
                        testCommander();
                        modMan.player[0].LoadContent(Content);
                        updateMap(levelSelect);
                        riverDefenseSelector.setOccupied(false);
                        testBeta = true;
                        initialized = false;
                        built = false;
                        riverDefenseSelector.setInteraction(true);
                        buildResources = 10;
                    }
                    riverDefense.DrawMap(spriteBatch);
                    //currMap = "riverDefense";
                }
                /*else if ((.getOccupied() == true) && dummyUnlockSelector.getInteraction() == true)
                {
                    updateMap(riverDefense);
                    loadMan.load(this.Content, modMan.player);
                    loadMan.load(this.Content, modMan.artificial);
                    riverDefense.DrawMap(spriteBatch);
                }
                 */
                else
                {
                    updateMap(levelSelect);
                    levelSelect.DrawMap(spriteBatch);
                }
                
            }
            else
            {
                mainScreen.DrawMap(spriteBatch);
                spriteBatch.Draw(Content.Load<Texture2D>("ret.png"), new Rectangle(102, 37, 500, 200), Color.White);
            }
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
