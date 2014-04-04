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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Map riverDefense;
        Builder dummy;
        MouseState mouseCurrent, mousePrev;

        List<Tower> towers;
        Tower tower;
        List<Projectile> proj;
        List<Archer> archers;
        List<Mobile> gameobj;
        List<GameObject> allObjects;
        HealthSystem healthChecker;
        AttackSystem attackChecker;
        InputManager inputManager;
        MovementManager movementManager;

        //List<Tower> towers;
        //Tower tower;
        //List<Archer> archers;
        //List<Mobile> gameobj;

        ModelManager modMan;
        LoadManager loadMan;
        //MovementManager movementManager;

        int attackDelay;

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
            riverDefense = new Map("Content/RiverDefense.txt");
            proj = new List<Projectile>();
 
            dummy = new Builder(new Sprite(32, 32, 32, 32), this.Content);
            int toweroffset = 50;
            gameobj = new List<Mobile>();
            allObjects = new List<GameObject>();

            for (int i = 0; i < 1; i++)

            //gameobj = new List<GameObject>();
            modMan = ModelManager.getInstance(riverDefense);
            loadMan = LoadManager.getInstance();
            //Create Player's units
            for (int i = 0; i < 5; i++)

            {
                modMan.addUnit("PLAYER", "TOWER", new Vector2(20+toweroffset,600));
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
            modMan.player[9].attackRange = 600;

            //archers = new List<Archer>();

            /*toweroffset = 0;
            for (int i = 0; i < 5; i++)
            {
               // gameobj.Add(new Archer(new Vector2( 60 + toweroffset , 20)));
                toweroffset += 50;
            }*/

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
                modMan.addUnit("ARTIFICIAL", "ARCHER", new Vector2(60 + toweroffset, 200));
                //gameobj.Add(new Archer(new Vector2(60 + toweroffset, 100)));
                toweroffset += 50;
            }
            //toweroffset = 0;
            //for (int i = 0; i < 5; i++)
            //{
            //    modMan.addUnit("PLAYER", "TOWER", new Vector2(60 + toweroffset, 180));
            //    //gameobj.Add(new Archer(new Vector2(60 + toweroffset, 180)));
            //    toweroffset += 50;
            //}


            allObjects.AddRange(gameobj);

            healthChecker = new HealthSystem(modMan.player, modMan.artificial);
            attackChecker = new AttackSystem(ref modMan.player, ref modMan.artificial);
            attackDelay = 0;
            //movementManager = new MovementManager();
            //inputManager = new InputManager(movementManager);
            inputManager = new InputManager(modMan);

            mousePrev = Mouse.GetState();

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
            /*for (int i = 0; i < modMan.player.Count; i++)
            {
                modMan.player[i].LoadContent(Content);
            }
            for (int i = 0; i < archers.Count; i++)
            {
                archers[i].LoadContent(Content);
            }
            for (int i = 0; i < gameobj.Count; i++)
            {
                Mobile temparch = gameobj[i];
                temparch.LoadContent(Content);
            }*/

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

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            mouseCurrent = Mouse.GetState();

            KeyboardState keyboardState = Keyboard.GetState();
            // TODO: Add your update logic here

            //inputManager.Update(mouseCurrent, mousePrev, keyboardState, ref towers, ref gameobj);
            inputManager.Update(mouseCurrent, mousePrev, keyboardState, ref modMan.player);


            //for (int i = 0; i < towers.Count; i++)
            //{
            //    if (towers[i].isAlive() == false) towers.Remove(towers[i]);
            //}

            healthChecker.Update(modMan.player, modMan.artificial);
            healthChecker.checkHealth();
            modMan.player = healthChecker.player;
            modMan.artificial = healthChecker.artificial;

            if (attackDelay == 0)
            {
                attackChecker.Update(ref modMan.player, ref modMan.artificial );
                attackChecker.autoAttacks(this.Content, ref proj);
                //towers = attackChecker.towers;
                //archers = attackChecker.archers;

                attackDelay = 60;
            }
            else attackDelay--;

            //movementManager.moveObjects(gameobj, towers);
            //movementManager.CheckPauses(gameobj);

            //MovementManager.moveObjects(gameobj);
            for (int i = 0; i < proj.Count; i++)
            {
                proj[i].move();
            }
            for (int i = 0; i < proj.Count; i++)
                if (proj[i].isAlive() == false)
                    proj.Remove(proj[i]);
            modMan.moveObjects(modMan.player, modMan.artificial);

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
            spriteBatch.Begin();
            riverDefense.DrawMap(spriteBatch);
            for (int i = 0; i < proj.Count; i++)
                if (proj[i].isAlive() == false)
                    proj.Remove(proj[i]);
            for (int i = 0; i < proj.Count; i++)
            {
                proj[i].Draw(spriteBatch);
            }
            for (int i = 0; i < modMan.player.Count; i++)
            {
                (modMan.player[i]).Draw(spriteBatch);
            }
            dummy.builderSprite.Draw(spriteBatch);

            for (int i = 0; i < modMan.artificial.Count; i++)
            {
                modMan.artificial[i].Draw(spriteBatch);
            }
            //for (int i = 0; i < gameobj.Count; i++)
            //{
            //    Mobile temparch = gameobj[i];
            //    temparch.Draw(spriteBatch);
            //}

            inputManager.DrawMouseRectangle(spriteBatch, Content);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        //public void checkHealth()
        //{
        //    for (int i = 0; i < towers.Count; i++)
        //    {
        //        if (towers[i].isAlive() == false)
        //        {
        //            towers.Remove(towers[i]);
        //        }
        //    }

        //    for (int i = 0; i < archers.Count; i++)
        //    {
        //        if (archers[i].isAlive() == false)
        //        {
        //            archers.Remove(archers[i]);
        //        }
        //    }
        //}
    }
}
