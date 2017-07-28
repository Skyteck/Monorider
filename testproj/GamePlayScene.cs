using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
namespace MonoRider
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GamePlayScene : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ContentManager CM_Play;
        //ContentManager CM_GO;
        Texture2D background;
        SteeringWheel wheel;
        bool debug = false;
        bool paused = false;
        bool pausedPressed = false;
        int midPoint;
        private SpriteFont font;
        Random Ranum = new Random();

        //Managers
        Managers.NPCManager _NPCManager;
        Managers.PlayerManager _PlayerManager;
        Managers.WorldGenManager _WorldManager;
        enum GamePlayState
        {
            kStatePlay,
            kStateGO
        };
        GamePlayState currentState = GamePlayState.kStatePlay;

        public GamePlayScene()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 320;
            graphics.PreferredBackBufferHeight = 480;
            graphics.ApplyChanges();
            CM_Play = new ContentManager(Content.ServiceProvider);
            CM_Play.RootDirectory = "Content";
            this.IsMouseVisible = true;
            //currentState = GamePlayState.kStatePlay;
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
            Player player = new Player();
            _NPCManager = new Managers.NPCManager(CM_Play, player);
            _PlayerManager = new Managers.PlayerManager(player, _NPCManager);
            _WorldManager = new Managers.WorldGenManager(_NPCManager);
            wheel = new SteeringWheel();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = CM_Play.Load<SpriteFont>("Fipps");
            midPoint = (GraphicsDevice.Viewport.Width / 2);
            if (currentState == GamePlayState.kStatePlay)
            {
                // Create a new SpriteBatch, which can be used to draw textures.

                // TODO: use this.Content to load your game content here
                background = CM_Play.Load<Texture2D>("grassBackground");
                _PlayerManager.LoadContent(CM_Play.Load<Texture2D>("car1"));
                _PlayerManager.PlacePlayer(new Vector2(midPoint, 320));
                _NPCManager.LoadContent(CM_Play);
                wheel.LoadContent("wheel", CM_Play);
                wheel._Position = new Vector2(160, 520);

                _NPCManager.CreateNPC(Enums.SpriteTags.kCarType, 50);
                _NPCManager.CreateNPC(Enums.SpriteTags.kRockType, 50);
                _NPCManager.CreateNPC(Enums.SpriteTags.kGearType, 50);
                _NPCManager.CreateNPC(Enums.SpriteTags.kOilType, 50);
                _NPCManager.CreateNPC(Enums.SpriteTags.kShieldType, 20);

                font = CM_Play.Load<SpriteFont>("test");
            }
            else if(currentState == GamePlayState.kStateGO)
            {
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            CM_Play.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (currentState == GamePlayState.kStatePlay)
            {
                //Gameplay spawn objects
                HandleInput();
                if (!paused)
                {
                    _PlayerManager.Update(gameTime);
                    _NPCManager.UpdateNPCs(gameTime);
                    _WorldManager.Update(gameTime);
                    wheel.Update(gameTime, _PlayerManager._Momentum);
                    base.Update(gameTime);
                }

                if (_PlayerManager._PlayerActive != true)
                {
                    this.ResetGame();
                }
            }
            else if(currentState == GamePlayState.kStateGO)
            {
                MouseState msState = Mouse.GetState();

                if (msState.LeftButton == ButtonState.Pressed)
                {
                    this.ResetGame();
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            // Start drawing
            if(debug)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
                RasterizerState state = new RasterizerState();
                state.FillMode = FillMode.WireFrame;
                spriteBatch.GraphicsDevice.RasterizerState = state;
            }
            else
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                // Sprite effects! https://msdn.microsoft.com/en-us/library/bb203872(v=xnagamestudio.40).aspx
            }
            if (currentState == GamePlayState.kStatePlay)
            {
                //spriteBatch.Draw(background, new Rectangle(midPoint-160, 0, 320, 480), Color.White);
                float test = midPoint - (background.Width / 2);
                spriteBatch.Draw(background, new Vector2(test, 0), Color.White);
                
                _NPCManager.DrawNPCs(spriteBatch);
                _PlayerManager.Draw(spriteBatch);
                wheel.Draw(spriteBatch);

                spriteBatch.DrawString(font, "Score: " + _PlayerManager._GearsCollected, new Vector2(5, 5), Color.Black);
            }
            else if(currentState == GamePlayState.kStateGO)
            {
                spriteBatch.DrawString(font, "Game over!", new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.Black);
            }

            // Stop drawing

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void HandleInput()
        {
            KeyboardState state = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || state.IsKeyDown(Keys.Escape))
                Exit();
            if(state.IsKeyDown(Keys.Enter))
            {
                if(!pausedPressed)
                {
                    if(paused)
                    {
                        paused = false;
                    }
                    else
                    {
                        paused = true;
                    }
                }
                pausedPressed = true;
            }
            if(state.IsKeyUp(Keys.Enter))
            {
                pausedPressed = false;
            }
            if(state.IsKeyDown(Keys.End))
            {
                ResetGame();
            }
        }

        private void ResetGame()
        {
            _NPCManager.ResetAll();
            
            paused = false;
            pausedPressed = false;

            _PlayerManager._Speed = 0;
            _PlayerManager._GearsCollected = 0;
            midPoint = GraphicsDevice.Viewport.Width / 2;
            Ranum = new Random();
            currentState = GamePlayState.kStatePlay;
            
            this.LoadContent();
        }
    }
}
