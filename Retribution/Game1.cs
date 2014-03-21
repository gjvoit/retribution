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
<<<<<<< HEAD
        Builder dummy;
        MouseState mouseCurrent, mousePrev;

=======
        Tower tower;
        Tower tower2;
>>>>>>> 9fe80ec34c488a576d2a34ac1c21a0de3ea5ffc6

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
<<<<<<< HEAD
            dummy = new Builder(new Sprite(32, 32, 32, 32), this.Content);
=======
            tower = new Tower(new Vector2(20, 20));
            tower.health = 50;
            tower.damage = 2;
            tower.attack_range = 40;
            tower2 = new Tower(new Vector2(600, 600));
>>>>>>> 9fe80ec34c488a576d2a34ac1c21a0de3ea5ffc6
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

            // KeyboardState keyboardState = Keyboard.GetState();
            // TODO: Add your update logic here
<<<<<<< HEAD
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

            mousePrev = mouseCurrent;
=======
            tower.Attack(tower2);
            tower.Update(gameTime);
            tower2.Update(gameTime);
>>>>>>> 9fe80ec34c488a576d2a34ac1c21a0de3ea5ffc6
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
            dummy.builderSprite.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
            tower.Draw(spriteBatch);
            tower2.Draw(spriteBatch);
        }
    }
}
