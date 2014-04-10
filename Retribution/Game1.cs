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
using Microsoft.Xna.Framework.Audio;


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
        Boolean testBeta = true;
        // Used for Selector deactivation
        String currMap;
        Selector defeatScreenSelector, mainScreenSelector, riverDefenseSelector, castleDefenseSelector,
            castleSiegeSelector, victoryScreenSelector;
        Map defeatScreen, mainScreen, levelSelect, castleDefense, riverDefense, castleSiege, victoryScreen;
        List<Map> theMaps;
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
        SoundEffect player;
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
            try
            {
                graphics = new GraphicsDeviceManager(this);
                //graphics.IsFullScreen = false;
                //Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);
                
            }
            catch
            {
            }
        }
        //void Window_ClientSizeChanged(object sender, EventArgs e)
        //{
        //    OpenTKGameWindow window = sender as OpenTKGameWindow;
        //    Console.WriteLine(window.ClientBounds.ToString());
        //}
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            try
            {
                Window.AllowUserResizing = true;
                //graphics.CreateDevice();
                graphics.PreferredBackBufferHeight = 704;
                graphics.PreferredBackBufferWidth = 1024;
                graphics.IsFullScreen = false;
             //   graphics.ToggleFullScreen();
                graphics.ApplyChanges();
            }
            catch
            {
            }
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: Add your initialization logic here
            defeatScreen = new Map("Content/defeatScreen.txt");
            mainScreen = new Map("Content/MainScreen.txt");
            levelSelect = new Map("Content/levelSelect.txt");
            castleDefense = new Map("Content/castleDefense.txt");
            riverDefense = new Map("Content/riverDefense.txt");
            castleSiege = new Map("Content/castleSiege.txt");
            victoryScreen = new Map("Content/victoryScreen.txt");
            mainScreenSelector = new Selector(new Rectangle(288, 0, 128, 64), mainScreen, levelSelect, true);
            castleDefenseSelector = new Selector(new Rectangle(32, 320, 128, 64), levelSelect, castleDefense, false);
            riverDefenseSelector = new Selector(new Rectangle(288, 320, 128, 64), levelSelect, riverDefense, false);
            castleSiegeSelector = new Selector(new Rectangle(544, 320, 128, 64), levelSelect, castleSiege, false);
            defeatScreenSelector = new Selector(new Rectangle(188, 0, 128, 64), defeatScreen, mainScreen, false);
            victoryScreenSelector = new Selector(new Rectangle(388, 0, 128, 64), victoryScreen, mainScreen, false);
            modMan = ModelManager.getInstance(ref mainScreen);
            loadMan = LoadManager.getInstance();
            projMan = ProjectileManager.getInstance();
            screenManager = ScreenManager.getInstance();
            // Add all maps to screenManager.allMaps
            screenManager.allMaps.Add(defeatScreen);
            screenManager.allMaps.Add(mainScreen);
            screenManager.allMaps.Add(levelSelect);
            screenManager.allMaps.Add(castleDefense);
            screenManager.allMaps.Add(riverDefense);
            screenManager.allMaps.Add(castleSiege);
            screenManager.allMaps.Add(victoryScreen);
            // Add all selectors to screenManager.allSelectors
            screenManager.allSelectors.Add(defeatScreenSelector);
            screenManager.allSelectors.Add(mainScreenSelector);
            screenManager.allSelectors.Add(castleDefenseSelector);
            screenManager.allSelectors.Add(riverDefenseSelector);
            screenManager.allSelectors.Add(castleSiegeSelector);
            screenManager.allSelectors.Add(victoryScreenSelector);
            // Set initial maps; These will always be defeatScreen, mainScreen and levelSelect,
            // since the user spawns on mainScreen
            screenManager.prevMap = defeatScreen;
            screenManager.currentMap = mainScreen;
            screenManager.nextMap = levelSelect;
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

        // Ensures that the managers all know which map to work on.
        public void updateManagerMap(Map myMap)
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
            Content.RootDirectory = "Content";
            loadMan.load(this.Content, modMan.player);
            loadMan.load(this.Content, modMan.artificial);
            player = Content.Load<SoundEffect>("back.wav");
            SoundEffectInstance instance = player.CreateInstance();
            instance.IsLooped = true;
            instance.Play();
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
            modMan.addUnit("ARTIFICIAL", "TOWER", new Vector2(672 - 280, 250));
            modMan.addUnit("ARTIFICIAL", "TOWER", new Vector2(672 - 150, 250));
          //  modMan.artificial[0].health = 5;
            int toweroffset = 0;
            for (int i = 0; i < 10; i++)
            {
                modMan.addUnit("ARTIFICIAL", "ARCHER", new Vector2(30 + toweroffset, 50));
                modMan.addUnit("ARTIFICIAL", "ARCHER", new Vector2(30 + toweroffset, 190));
                // modMan.artificial[5].health = 10000;
                //modMan.artificial[5].damage = 1000;

                //gameobj.Add(new Archer(new Vector2(60 + toweroffset, 100)));
                toweroffset += 70;
            }
            modMan.addUnit("ARTIFICIAL", "WARRIOR", new Vector2(180, 250));
            modMan.addUnit("ARTIFICIAL", "WARRIOR", new Vector2(248, 250));
            modMan.addUnit("ARTIFICIAL", "WARRIOR", new Vector2(672 - 280 + 32, 250));
            modMan.addUnit("ARTIFICIAL", "WARRIOR", new Vector2(672 - 150 - 32, 250));
            //modMan.addUnit("PLAYER", "CLERIC", new Vector2(250,550));
            //toweroffset = 50;
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
            loadMan.load(this.Content, modMan.player);
            loadMan.load(this.Content, modMan.artificial);
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
                //Console.WriteLine("a");
                inputManager.Update(mouseCurrent, mousePrev, keyboardState, ref modMan.player, ref loadMan, ref projMan, this.Content, ref buildResources, true);
                if (buildResources <= 1) // once we deplete our build resources, set built to true (doing so will initialize enemy AI units and starts the level)
                    built = true;
            }
            else if (built && initialized)// player is not building in build phase but rather building reinforcements - notice the false flag at the end indicating not build phase
            {
                //Console.WriteLine("b");
                inputManager.Update(mouseCurrent, mousePrev, keyboardState, ref modMan.player, ref loadMan, ref projMan, Content, ref playerResources, false);
            }
            else
            {
                inputManager.Update(mouseCurrent, mousePrev, keyboardState, ref modMan.player, ref loadMan, ref projMan, Content, ref noresource, false);
            }
            if (built && !initialized)// && castleDefenseSelector.getOccupied() == true)
            {
                //Console.WriteLine("c");
                betaInitialization();
                initialized = true; // set initialized to true to prevent looped enemy unit spawning
            }

            if (initialized)
            {
                //Console.WriteLine("d");
                theResource.ssY = playerResources * 32;
            }

            healthChecker.Update(modMan.player, modMan.artificial);
            healthChecker.checkHealth(this.Content);
            modMan.player = healthChecker.player;
            modMan.artificial = healthChecker.artificial;
            attackChecker.Update(ref modMan.player, ref modMan.artificial );
            attackChecker.autoAttacks();
            projMan.fireProjectiles();
            movementManager.moveObjects(modMan.player, modMan.artificial);

            aiManager.SetAIDestinations2(modMan.artificial);

            if ((modMan.player.Count == 0) && built)
            {
                screenManager.victory = "defeat";
            }
            else if ((modMan.artificial.Count == 0) && built)
            {
                screenManager.victory = "victory";
            }

            screenManager.updateSelectors(screenManager.victory);

            /*if (screenManager.victory.Equals("victory") && screenManager.currentMap.name.Equals("castleSiege"))
            {
                screenManager.victory = "undef";
                screenManager.prevMap = screenManager.allMaps[screenManager.progressIndex];
                screenManager.progressIndex++;
                screenManager.currentMap = screenManager.allMaps[screenManager.progressIndex];
                screenManager.nextMap = screenManager.allMaps[screenManager.progressIndex + 1];

            }*/

            //aiManager.SetAIDestinations(modMan.artificial);

            //  TyDigit: Change the digit based on amount of resources left

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
            updateManagerMap(screenManager.currentMap);
            
            if (modMan.player.Count != 0 && !playable)
            {
                screenManager.chooseSelector((Mobile)modMan.player[0]);
            }
            screenManager.currentMap.DrawMap(spriteBatch);
            if (screenManager.currentMap.name.Equals("Content/castleDefense.txt")
                || screenManager.currentMap.name.Equals("Content/riverDefense.txt")
                || screenManager.currentMap.name.Equals("Content/castleSiege.txt"))
            {
                //Console.WriteLine("e");
                playable = true;
                //loadMan.load(this.Content, modMan.player);
                //loadMan.load(this.Content, modMan.artificial);
            }
            else
            {
                //Console.WriteLine("f");
                playable = false;
                initialized = false;
                built = false;
            }
            if (screenManager.currentMap.name.Equals("Content/MainScreen.txt")) 
            {
                spriteBatch.Draw(Content.Load<Texture2D>("ret.png"), new Rectangle(102, 37, 500, 200), Color.White);
            }
            if (screenManager.currentMap.name.Equals("Content/levelSelect.txt"))//ghetto right now, but it'll do.
            {
                spriteBatch.Draw(Content.Load<Texture2D>("Castle.png"), new Rectangle(32, 320, 128, 64), Color.White);
                spriteBatch.Draw(Content.Load<Texture2D>("Castle.png"), new Rectangle(32+256, 320, 128, 64), Color.White);
                spriteBatch.Draw(Content.Load<Texture2D>("Castle.png"), new Rectangle(32+512, 320, 128, 64), Color.White);
            }
            //if (playable)//screenManager.currentMap.name.Equals("Content/castleDefense.txt"))
            //{
            //    if (testBeta)
            //    {
            //        modMan.player.Remove(modMan.player[0]);
            //        loadMan.load(this.Content, modMan.player);
            //        loadMan.load(this.Content, modMan.artificial);
            //        //riverDefenseSelector.setOccupied(false);
            //        testBeta = false;
            //        riverDefenseSelector.setInteraction(true);
            //    }
            if (playable && testBeta)
            {
                //Console.WriteLine("g");
                modMan.player.Remove(modMan.player[0]);
                testBeta = false;
            }
            if (built && initialized)  // hopefully get digits working and draw it to the screen (only when game starts)
            {
                //Console.WriteLine("h");
                theResource.ssY = playerResources * 32;
                theResource.Draw(spriteBatch);
            }
            //}
            
            // We want to start on the level castleDefense, then unlock riverDefense if victory, else exit to defeat screen
            // Need defeat and victory screen

            foreach(Projectile item in projMan.proj)
            {
                item.Draw(spriteBatch);
            }
            for (int i = 0; i < modMan.player.Count; i++)
            {
                (modMan.player[i]).Draw(spriteBatch);
            }

            for (int i = 0; i < modMan.artificial.Count; i++)
            {
                modMan.artificial[i].Draw(spriteBatch,Color.Coral);
            }

            inputManager.DrawMouseRectangle(spriteBatch, Content);

            spriteBatch.End();

            

            base.Draw(gameTime);
        }

      
    }
}
