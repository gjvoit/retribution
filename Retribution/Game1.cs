﻿#region Using Statements
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
        Selector defeatScreenSelector, mainScreenSelector, riverDefenseSelector, castleDefenseSelector,
            castleSiegeSelector, victoryScreenSelector;
        Map defeatScreen, mainScreen, levelSelect, castleDefense, riverDefense, castleSiege, victoryScreen;
        MouseState mouseCurrent, mousePrev;
        String currentTransition = "levelSelect";
        Sprite levelLock;
        SpriteFont transitionText;
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
        MoraleBar mBar;
        GUIButtons gui;
        InfoCard info;
        Texture2D transitionScreen;
        int prevResources = 10;
        Dictionary<Keys, List<GameObject>> groupedUnits;
        bool completeTransition = true;
      //  Warrior theCommander;
        int playerResources = 10;
        int buildResources = 0;
        // if built is false, player enters build phase; if built is true, that means player finished build phase and level starts
        static bool built = false;
        // if initialized is false, that means AI units have not been initialized
        bool initialized = false;
        bool whatIsTruth = true;
        static bool playable = false;
        static bool preventBuilding = false;
        bool casdefbuilt = false;
        double ClickTimer;

        //  TyDigit digit test to display amount of resources left
        Digits theResource;
        titleShell theTitle;

        public Game1()
            : base()
        {
            try
            {
                graphics = new GraphicsDeviceManager(this);

            }
            catch
            {
            }
        }
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
            ClickTimer = 0;
            groupedUnits = new Dictionary<Keys, List<GameObject>>();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: Add your initialization logic here
            defeatScreen = new Map("Content/defeatScreen.txt");
            mainScreen = new Map("Content/MainScreen.txt");
            levelSelect = new Map("Content/levelSelect.txt");
            castleDefense = new Map("Content/castleDefense.txt");
            riverDefense = new Map("Content/riverDefense.txt");
            castleSiege = new Map("Content/castleSiege.txt");
            victoryScreen = new Map("Content/victoryScreen.txt");
            mainScreenSelector = new Selector(new Rectangle(448, 0, 128, 64), mainScreen, levelSelect, true);
            castleDefenseSelector = new Selector(new Rectangle(96, 320, 192, 96), levelSelect, castleDefense, false);
            riverDefenseSelector = new Selector(new Rectangle(416, 320, 192, 96), levelSelect, riverDefense, false);
            castleSiegeSelector = new Selector(new Rectangle(736, 320, 192, 96), levelSelect, castleSiege, false);
            defeatScreenSelector = new Selector(new Rectangle(448, 256, 128, 64), defeatScreen, mainScreen, false);
            victoryScreenSelector = new Selector(new Rectangle(448, 288, 128, 64), victoryScreen, mainScreen, false);
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
            healthChecker = new HealthSystem(ModelManager.player, ModelManager.artificial);
            attackChecker = new AttackSystem(ref ModelManager.player, ref ModelManager.artificial);
            attackChecker.linkSystem(projMan);
            attackChecker.linkContent(Content);
            movementManager = MovementManager.getInstance();
            movementManager.setMap(mainScreen);
            inputManager = new InputManager(ref modMan);
            aiManager = AIManager.getInstance(ref mainScreen);
            mBar = new MoraleBar(ref modMan);
            gui = new GUIButtons(ref modMan);
            info = new InfoCard(ref modMan);
            inputManager.linkGUI(gui);
            MoraleBar.resourceVal(buildResources);
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
            InfoCard.texture = Content.Load<Texture2D>("blank");
            transitionScreen = Content.Load<Texture2D>("blank");
            transitionText = Content.Load<SpriteFont>("Times New Roman");
            info.txt = Content.Load<SpriteFont>("Times New Roman");
            mBar.txt = Content.Load<SpriteFont>("Times New Roman");
            //loadMan.loadContent(this.Content);
            loadMan.load(this.Content, ModelManager.player);
            loadMan.load(this.Content, ModelManager.artificial);
            gui.LoadContent(this.Content);
            healthChecker.clink = Content.Load<SoundEffect>("coins.wav");
            mBar.horn = Content.Load<SoundEffect>("horn.wav");
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

        public void castleDefenseSpawn()
        {
            int toweroffset = 0;
            // This only gets called if built and initialized (aka player resources = 0)
            
            for (int i = 0; i < 5; i++)
            {
                modMan.addUnit("ARTIFICIAL", "ARCHER", new Vector2(375 + toweroffset, 75));
                toweroffset += 50;
            }
            // Draw 2 towers for Player
            modMan.addUnit("ARTIFICIAL", "WARRIOR", new Vector2(425, 175));
            modMan.addUnit("ARTIFICIAL", "WARRIOR", new Vector2(475, 175));
            modMan.addUnit("ARTIFICIAL", "WARRIOR", new Vector2(525, 175));
           //ModelManager.artificial.Add(new BossUnit(new Vector2(475,590),400));
           //ModelManager.player.Add(new BossUnit(new Vector2(475,590),4000));
            //modMan.addUnit("PLAYER", "CLERIC", new Vector2(250,550));
            /* ---------------------------------------------------------------------------------------------------- */
            theResource = new Digits(new Vector2(0, 672));
            theTitle = new titleShell(new Vector2(375, 375));
            theResource.LoadContent(this.Content);
            theTitle.LoadContent(this.Content);
            loadMan.load(this.Content, ModelManager.player);
            loadMan.load(this.Content, ModelManager.artificial);
        }

        public void castleSiegeSpawn()
        {
            int toweroffset = 0;

            for (int i = 0; i < 5; i++)
            {
                modMan.addUnit("ARTIFICIAL", "ARCHER", new Vector2(375 + toweroffset, 75));
                toweroffset += 50;
            }
            modMan.addUnit("ARTIFICIAL", "TOWER", new Vector2(384, 224));
            modMan.addUnit("ARTIFICIAL", "TOWER", new Vector2(576, 224));
            modMan.addUnit("ARTIFICIAL", "CLERIC", new Vector2(475, 175));
            modMan.addUnit("ARTIFICIAL", "WARRIOR", new Vector2(475, 225));
            //modMan.addUnit("PLAYER", "CLERIC", new Vector2(250,550));
            mBar.bossSpawn = false;
            /* ---------------------------------------------------------------------------------------------------- */
            theResource = new Digits(new Vector2(0, 672));
            theTitle = new titleShell(new Vector2(375, 375));
            theResource.LoadContent(this.Content);
            theTitle.LoadContent(this.Content);
            loadMan.load(this.Content, ModelManager.player);
            loadMan.load(this.Content, ModelManager.artificial);
        }

        public void riverDefenseSpawn() 
        {
            int toweroffset = 0;

            for (int i = 0; i < 5; i++)
            {
                modMan.addUnit("ARTIFICIAL", "ARCHER", new Vector2(375 + toweroffset, 75));
                toweroffset += 50;
            }
            modMan.addUnit("ARTIFICIAL", "WARRIOR", new Vector2(475, 150));
            //modMan.addUnit("PLAYER", "CLERIC", new Vector2(250,550));
            /* ---------------------------------------------------------------------------------------------------- */
            theResource = new Digits(new Vector2(0, 672));
            theTitle = new titleShell(new Vector2(375, 375));
            theResource.LoadContent(this.Content);
            theTitle.LoadContent(this.Content);
            loadMan.load(this.Content, ModelManager.player);
            loadMan.load(this.Content, ModelManager.artificial);
        }
      


        public void testCommander()
        {
            //theCommander = new Warrior(new Vector2(600, 400));
            modMan.addUnit("PLAYER", "WARRIOR", new Vector2(400, 600));
            ((Mobile)ModelManager.player[0]).moveSpeed = 7;
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
            ClickTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            mouseCurrent = Mouse.GetState();

            KeyboardState keyboardState = Keyboard.GetState();


            healthChecker.Update(ModelManager.player, ModelManager.artificial);
            healthChecker.checkHealth(this.Content);
            ModelManager.player = healthChecker.player;
            ModelManager.artificial = healthChecker.artificial;
            attackChecker.Update(ref ModelManager.player, ref ModelManager.artificial);
            attackChecker.autoAttacks();
            projMan.fireProjectiles();
            if (built)
            {
                mBar.calculateScore();
            }
           
            
            
            
            
            //inputManager.Update(mouseCurrent, mousePrev, keyboardState, ref towers, ref gameobj);

            if (!built && playable) // Enter build phase if built is False, notice true flag at the end indicating we're in build phase
            {
                //Console.WriteLine("a");
                // Why is the "buildPhase" Boolean always true? Should it be equal to "built"?
                // 2 checks: either you don’t want to use all your resources, and want to start the game now, or you’ve used all your resources
                // and the player should receive a “ready check”
                if(keyboardState.IsKeyDown(Keys.Enter))
                { // once we deplete our build resources, set built to true (doing so will initialize enemy AI units and starts the level)
                    built = true;
                    MoraleBar.resourceAdd(playerResources);
                }
                    inputManager.Update(mouseCurrent, mousePrev, ref ClickTimer, keyboardState, ref groupedUnits, ref ModelManager.player, ref ModelManager.artificial, ref loadMan, ref projMan, this.Content, ref MoraleBar.resources, true);
                //MoraleBar.resourceVal(buildResources);
                
            }
            else if (built && initialized)// player is not building in build phase but rather building reinforcements - notice the false flag at the end indicating not build phase
            {
                //Console.WriteLine("b");
                inputManager.Update(mouseCurrent, mousePrev, ref ClickTimer, keyboardState, ref groupedUnits, ref ModelManager.player, ref ModelManager.artificial, ref loadMan, ref projMan, Content, ref MoraleBar.resources, false);
                //MoraleBar.resourceVal(playerResources);
            }
            else
            {
                int noResources = 0;
                inputManager.Update(mouseCurrent, mousePrev, ref ClickTimer, keyboardState, ref groupedUnits, ref ModelManager.player, ref ModelManager.artificial, ref loadMan, ref projMan, Content, ref noResources, false);
            }
            if (built && !initialized)// && castleDefenseSelector.getOccupied() == true)
            {
                //Console.WriteLine("c");
                if (screenManager.currentMap.name.Equals("Content/castleDefense.txt"))
                {
                    castleDefenseSpawn();
                }
                else if (screenManager.currentMap.name.Equals("Content/castleSiege.txt"))
                {
                    castleSiegeSpawn();
                }
                else if (screenManager.currentMap.name.Equals("Content/riverDefense.txt"))
                {
                    riverDefenseSpawn();
                }
                initialized = true; // set initialized to true to prevent looped enemy unit spawning
            }

            if (initialized)
            {
                //Console.WriteLine("d");
                theResource.ssY = playerResources * 32;
            }
            loadMan.load(Content, ModelManager.artificial);
            loadMan.load(Content, ModelManager.player);
            movementManager.moveObjects(ModelManager.player, ModelManager.artificial);
            aiManager.SetAIDestinations2(ModelManager.artificial);
            if ((ModelManager.player.Count == 0) && built)
            {
                screenManager.victory = "defeat";
                SoundEffect defeatEffect = Content.Load<SoundEffect>("wilhelm.wav");
                defeatEffect.Play();
                ModelManager.artificial.Clear();
                testCommander();
                mBar.waveNum = 0;
                loadMan.load(Content, ModelManager.player);
                built = false;
                playable = false;
                initialized = false;
                if (screenManager.currentMap.name.Equals("Content/castleDefense.txt"))
                {
                    prevResources = 10;
                    buildResources = 0;
                    MoraleBar.resourceVal(buildResources);
                    currentTransition = "defeatScreen";
                    completeTransition = false;
                }

                else if (screenManager.currentMap.name.Equals("Content/riverDefense.txt"))
                {
                    prevResources = 10;
                    buildResources = 0;
                    MoraleBar.resourceVal(buildResources);
                    currentTransition = "fallBackToTheCastle";
                    completeTransition = false;
                }
                else if (screenManager.currentMap.name.Equals("Content/castleSiege.txt"))
                {
                    playerResources = 10;
                    prevResources = 15;
                    buildResources = 0;
                    MoraleBar.resourceVal(buildResources);
                    currentTransition = "fallBackToTheRiver";
                    completeTransition = false;
                }
                testBeta = true;
                //Console.WriteLine("interaction for defeatscreenselector: " + screenManager.allSelectors[0].getInteraction());
            }
            else if ((ModelManager.artificial.Count == 0) && built)
            {
                screenManager.victory = "victory";
                ModelManager.player.Clear();
                testCommander();
                mBar.waveNum = 0;
                loadMan.load(Content, ModelManager.player);
                built = false;
                playable = false;
                initialized = false;
                if (screenManager.currentMap.name.Equals("Content/castleDefense.txt"))
                {
                    if (MoraleBar.resources > 15)
                    {
                        prevResources = MoraleBar.resources;
                    }
                    else
                    {
                        prevResources = 15;
                    }
                    currentTransition = "onwardToTheRiver";
                    playerResources = 10;
                    buildResources = 0;
                    MoraleBar.resourceVal(buildResources);
                    completeTransition = false;
                }

                else if (screenManager.currentMap.name.Equals("Content/riverDefense.txt")) 
                {
                    if (MoraleBar.resources > 20)
                    {
                        prevResources = MoraleBar.resources;
                    }
                    else
                    {
                        prevResources = 20;
                    }
                    playerResources = 10;
                    buildResources = 0;
                    MoraleBar.resourceVal(buildResources);
                    currentTransition = "onwardToTheCastle";
                    completeTransition = false;
                }
                else if (screenManager.currentMap.name.Equals("Content/castleSiege.txt"))
                {
                    playerResources = 10;
                    prevResources = 10;
                    buildResources = 0;
                    MoraleBar.resourceVal(buildResources);
                    currentTransition = "victoryScreen";
                    completeTransition = false;
                }
                testBeta = true;
            }
            if (completeTransition == false && keyboardState.IsKeyDown(Keys.Enter))
            {
                completeTransition = true;
                currentTransition = "null";
            }

            screenManager.updateSelectors(screenManager.victory);
            if (screenManager.currentMap.name.Equals("Content/MainScreen.txt") && completeTransition == true && whatIsTruth == false) 
            {
                currentTransition = "levelSelect";
                whatIsTruth = true;
            }
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
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            // Create a list of Selectors and simply remove/add them depending on the map loaded. Associate selectors with Map.cs
            spriteBatch.Begin();
            updateManagerMap(screenManager.currentMap);
            
            if (ModelManager.player.Count != 0 && !playable)
            {
                screenManager.chooseSelector((Mobile)ModelManager.player[0]);
            }

            if (screenManager.currentMap.name.Equals("Content/castleDefense.txt")
                || screenManager.currentMap.name.Equals("Content/riverDefense.txt")
                || screenManager.currentMap.name.Equals("Content/castleSiege.txt"))
            {
                //Console.WriteLine("e");
                playable = true;
                inputManager.buildPhase = false;
                loadMan.load(this.Content, ModelManager.player);
                loadMan.load(this.Content, ModelManager.artificial);
            }
            else
            {
                //Console.WriteLine("f");
                playable = false;

            }

            if (playable && testBeta)
            {
                //Console.WriteLine("g");
                ModelManager.player.Clear();
                buildResources = prevResources;
                MoraleBar.resourceVal(buildResources);
                if (screenManager.currentMap.name.Equals("Content/castleDefense.txt"))
                {
                    modMan.addUnit("PLAYER", "TOWER", new Vector2(416, 416));
                    modMan.addUnit("PLAYER", "TOWER", new Vector2(576, 416));
                }

                loadMan.load(this.Content, ModelManager.player);
                //ModelManager.player.Remove(ModelManager.player[0]);
                testBeta = false;
            }
            if (screenManager.currentMap.name.Equals("Content/levelSelect.txt") && whatIsTruth == true) 
            {
                completeTransition = false;
                whatIsTruth = false;
            }
            if (completeTransition)
            {
                screenManager.currentMap.DrawMap(spriteBatch);
            }
            else
            {
                if (currentTransition.Equals("levelSelect"))
                {
                    spriteBatch.Draw(transitionScreen, new Rectangle(0, 0, 704, 1024), Color.Black);
                    spriteBatch.DrawString(transitionText, "Your city has long held tenuous relations with a nearby nation.\nMany times, each has tried to conquer the other, but 'til now,\nneither has ever succeeded. As the heir ascendant to the throne\nof your nation, it is your responsibility to protect your people\nfrom harm and to repel the enemy attacks. But your nation is\nnot a mere victim. Both nations have always housed sizable\narmies who wield impressive tactical prowess. With the recent\ndeath of your father, the king, the rival nation has been undergoing\nobvious plans to launch yet another campaign against your city.\n\nIt is up to you to not only repel their assault, but to turn their attack\non its head, and bring the fight to them, once and for all.\n\nPress Enter to proceed to Level Select", new Vector2(350, 200), Color.White);
                }
                if (currentTransition.Equals("onwardToTheRiver"))
                {
                    spriteBatch.Draw(transitionScreen, new Rectangle(0, 0, 704, 1024), Color.Black);
                    spriteBatch.DrawString(transitionText, "You successfully repeled the enemy attack.\nNow is your chance to strike back!\nBut be wary, this isn't over yet...", new Vector2(350, 270), Color.White);
                }
                if (currentTransition.Equals("onwardToTheCastle"))
                {
                    spriteBatch.Draw(transitionScreen, new Rectangle(0, 0, 704, 1024), Color.Black);
                    spriteBatch.DrawString(transitionText, "You've reached the enemy's primary city.\nDestroy them now and end this conflict..\nonce and for all.", new Vector2(350, 270), Color.White);
                }
                if (currentTransition.Equals("fallBackToTheCastle"))
                {
                    spriteBatch.Draw(transitionScreen, new Rectangle(0, 0, 704, 1024), Color.Black);
                    spriteBatch.DrawString(transitionText, "Your counter-attack has been thwarted, \nretreat to the castle!", new Vector2(350, 270), Color.White);
                }
                if (currentTransition.Equals("fallBackToTheRiver"))
                {
                    spriteBatch.Draw(transitionScreen, new Rectangle(0, 0, 704, 1024), Color.Black);
                    spriteBatch.DrawString(transitionText, "There's a reason this war has raged for so long. \nThe enemy is chasing you back to the river!", new Vector2(350, 270), Color.White);
                }
            }
            if (screenManager.currentMap.name.Equals("Content/MainScreen.txt")) 
            {
                spriteBatch.Draw(Content.Load<Texture2D>("ret.png"), new Rectangle(262, 37, 500, 200), Color.White);
            }
            if (screenManager.currentMap.name.Equals("Content/levelSelect.txt"))//ghetto right now, but it'll do.
            {
                if ((!currentTransition.Equals("levelSelect") || !currentTransition.Equals("onwardToTheCastle") || !currentTransition.Equals("onwardToTheRiver") || !currentTransition.Equals("fallBackToTheRiver") || !currentTransition.Equals("fallBackToTheCastle")) && completeTransition == true)
                {
                    spriteBatch.Draw(Content.Load<Texture2D>("CastleSiege.png"), new Rectangle(96 + 32 + 640, 356, 128, 64), Color.White);
                    spriteBatch.Draw(Content.Load<Texture2D>("TheRiver.png"), new Rectangle(96 + 346, 346, 128, 64), Color.White);
                    spriteBatch.Draw(Content.Load<Texture2D>("CastleDefence.png"), new Rectangle(96 + 32, 335, 128, 64), Color.White);
                    if (screenManager.allSelectors[3].getInteraction() == false)
                    {
                        spriteBatch.Draw(Content.Load<Texture2D>("lock.png"), new Rectangle(96 + 320, 320, 32, 32), Color.White);
                    }
                    if (screenManager.allSelectors[4].getInteraction() == false)
                    {
                        spriteBatch.Draw(Content.Load<Texture2D>("lock.png"), new Rectangle(96 + 640, 320, 32, 32), Color.White);
                    }
                }
            }
            if (screenManager.currentMap.name.Equals("Content/defeatScreen.txt"))
            {
                if (currentTransition.Equals("defeatScreen"))
                {
                    spriteBatch.Draw(transitionScreen, new Rectangle(0, 0, 704, 1024), Color.Black);
                    spriteBatch.DrawString(transitionText, "Your kingdom has fallen to the invaders.\nYour enemies were right to assume your \nweakness would lead to your kingdom's \ndownfall.", new Vector2(350, 270), Color.White);
                    spriteBatch.Draw(Content.Load<Texture2D>("Defeat.png"), new Rectangle(262, 37, 500, 200), Color.White);
                    // Write about how your kingdom was destroyed
                }
            }
            if (screenManager.currentMap.name.Equals("Content/victoryScreen.txt"))
            {
                if (currentTransition.Equals("victoryScreen"))
                {
                    spriteBatch.Draw(transitionScreen, new Rectangle(0, 0, 704, 1024), Color.Black);
                    spriteBatch.DrawString(transitionText, "You have conquered the capital city of your enemies.\nYour father smiles down on you from the heavens,\nproud that you have fulfilled your destiny and \nbrought peace to the kingdom.", new Vector2(350, 270), Color.White);
                    spriteBatch.Draw(Content.Load<Texture2D>("Victory.png"), new Rectangle(262, 37, 500, 200), Color.White);
                }
            }

            //if (built && initialized)  // hopefully get digits working and draw it to the screen (only when game starts)
            //{
            //    //Console.WriteLine("h");
            //    theResource.ssY = playerResources * 32;
            //    theResource.Draw(spriteBatch);
            //}
            //}
            
            // We want to start on the level castleDefense, then unlock riverDefense if victory, else exit to defeat screen
            // Need defeat and victory screen

            foreach(Projectile item in projMan.proj)//draw arrows
            {
                item.Draw(spriteBatch);
            }
            for (int i = 0; i < ModelManager.player.Count; i++)//draw player objects
            {
                    (ModelManager.player[i]).Animate();
                    (ModelManager.player[i]).Draw(spriteBatch);
            }

            for (int i = 0; i < ModelManager.artificial.Count; i++)//draw AI objects
            {
                ModelManager.artificial[i].Animate();
                ModelManager.artificial[i].Draw(spriteBatch,Color.Coral);
            }

            inputManager.DrawMouseRectangle(spriteBatch, Content);//draw select square?
            mBar.Draw(spriteBatch);
            gui.Draw(spriteBatch);
            info.Draw(spriteBatch);
            spriteBatch.End();

            

            base.Draw(gameTime);
        }

      
    }
}
