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
        Tower tower;
        Tower tower2;
        List<Tower> towers;
        List<Archer> archers;
        HealthSystem healthChecker;
        AttackSystem attackChecker;
        int attackDelay;

        //Mobiles[] mobiles;        
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
            // TODO: Add your initialization logic here
            riverDefense = new Map("Content/RiverDefense.txt");

            int attackDelay = 60;

            dummy = new Builder(new Sprite(32, 32, 32, 32), this.Content);
            int toweroffset = 50;
            towers = new List<Tower>();
            for (int i = 0; i < 5; i++)
            {
                towers.Add(new Tower(new Vector2(20 + toweroffset, 600)));
                toweroffset += 50;
            }

   
            archers = new List<Archer>();

            toweroffset = 0;
            for (int i = 0; i < 5; i++)
            {
                archers.Add(new Archer(new Vector2( 60 + toweroffset , 20)));
                toweroffset += 50;
            }
            toweroffset = 0;
            for (int i = 0; i < 5; i++)
            {
                archers.Add(new Archer(new Vector2(60 + toweroffset, 100)));
                toweroffset += 50;
            }
            toweroffset = 0;
            for (int i = 0; i < 5; i++)
            {
                archers.Add(new Archer(new Vector2(60 + toweroffset, 180)));
                toweroffset += 50;
            }

            tower = new Tower(new Vector2(20, 20));
            tower.health = 50;
            tower.damage = 2;
            tower.attackRange = 200;
            tower2 = new Tower(new Vector2(600, 600));
<<<<<<< HEAD
            towers.Add(tower2);
=======

            towers.Add(tower2);
            towers.Add(tower);

            healthChecker = new HealthSystem(towers, archers);
            attackChecker = new AttackSystem(towers, archers);

>>>>>>> 3d0754205313a76004001e81ca806258029c275e
            base.Initialize();
            this.IsMouseVisible = true;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            tower.LoadContent(Content);
            tower2.LoadContent(Content);
            for (int i = 0; i < towers.Count; i++)
            {
                towers[i].LoadContent(Content);
            }
            for (int i = 0; i < archers.Count; i++)
            {
                archers[i].LoadContent(Content);
            }

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
            if (mouseCurrent.LeftButton == ButtonState.Pressed
                && mousePrev.LeftButton == ButtonState.Released
                && dummy.IsSelectable(mouseCurrent))
            {
                dummy.selected = true;
            }
            else if (mouseCurrent.LeftButton == ButtonState.Pressed
                && mousePrev.LeftButton == ButtonState.Released)
            {
                dummy.selected = false;
            }

            if (mouseCurrent.RightButton == ButtonState.Pressed
                && mousePrev.RightButton == ButtonState.Released
                && dummy.selected == true)
            {
                dummy.Move(mouseCurrent);
            }
            for (int i = 0; i < towers.Count; i++)
            {
                if (towers[i].isAlive() == false) towers.Remove(towers[i]);
            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                tower.Attack(tower2);
            }

            healthChecker.Update(towers, archers);
            healthChecker.checkHealth();
            towers = healthChecker.towers;
            archers = healthChecker.archers;

            if (attackDelay == 0)
            {
                attackChecker.Update(towers, archers);
                attackChecker.autoAttacks();
                towers = attackChecker.towers;
                archers = attackChecker.archers;
                attackDelay = 60;
            }

            else attackDelay--;
 
            mousePrev = mouseCurrent;
<<<<<<< HEAD
            tower.Update(gameTime);
            tower2.Update(gameTime);
=======

            //tower.Update(gameTime);
            //tower2.Update(gameTime);
>>>>>>> 3d0754205313a76004001e81ca806258029c275e
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
            for (int i = 0; i < towers.Count; i++)
            {
                towers[i].Draw(spriteBatch);
            }
<<<<<<< HEAD
            dummy.builderSprite.Draw(spriteBatch);
            tower.Draw(spriteBatch);
=======

            for (int i = 0; i < archers.Count; i++)
            {
                archers[i].Draw(spriteBatch);
            }

            //dummy.builderSprite.Draw(spriteBatch);
            //tower.Draw(spriteBatch);
            //tower2.Draw(spriteBatch);
>>>>>>> 3d0754205313a76004001e81ca806258029c275e
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void checkHealth()
        {
                        for (int i = 0; i < towers.Count; i++)
            {
                if (towers[i].isAlive() == false)
                {
                    towers.Remove(towers[i]);
                }
            }

            for (int i = 0; i < archers.Count; i++)
            {
                if (archers[i].isAlive() == false)
                {
                    archers.Remove(archers[i]);
                }
            }
        }
    }
}
