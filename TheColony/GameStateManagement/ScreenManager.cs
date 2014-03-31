using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameStateManagement
{
    public class ScreenManager : DrawableGameComponent
    {
        //arrays to hold screens
        List<GameScreen> screens = new List<GameScreen>();
        List<GameScreen> tempScreensList = new List<GameScreen>();

        //monitors player input
        public InputState input = new InputState();

        SpriteBatch spriteBatch;
        SpriteFont font;
        Texture2D blankTexture;

        bool isInitialized;
        
        //constructor
        public ScreenManager(Game game) : base(game)
        {

        }

        public override void Initialize()
        {
            base.Initialize();

            isInitialized = true;
        }

        protected override void LoadContent()
        {
            // Load content belonging to the screen manager.
            ContentManager content = Game.Content;

            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = content.Load<SpriteFont>("menufont");
            blankTexture = content.Load<Texture2D>("blank");

            //load each screen's content
            foreach (GameScreen screen in screens)
            {
                screen.Load();
            }
        }

        protected override void UnloadContent()
        {
            //unload each screen's content
            foreach (GameScreen screen in screens)
            {
                screen.Unload();
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) == true)
            {
                Game.Exit();
            }

            //read the keyboard
            input.Update();

            //make a copy of the screen list if updating one scene adds/removes others
            tempScreensList.Clear();

            foreach (GameScreen screen in screens)
            {
                tempScreensList.Add(screen);
            }

            //checks if current screen is active
            bool otherScreenHasFocus = !Game.IsActive;
            bool coveredByOtherScreen = false;

            //iterates for each screen waiting to update
            while (tempScreensList.Count > 0)
            {
                //pop the topmost screen
                GameScreen screen = tempScreensList[tempScreensList.Count - 1];
                tempScreensList.RemoveAt(tempScreensList.Count - 1);

                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                if (screen.ScreenState == ScreenState.TransitionOn || screen.ScreenState == ScreenState.Active)
                {
                    //first active screen will handle input
                    if (!otherScreenHasFocus)
                    {
                        screen.HandleInput(gameTime, input);
                        otherScreenHasFocus = true;
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (GameScreen screen in screens)
            {
                //draw only visible screens
                if (screen.ScreenState == ScreenState.Hidden)
                {
                    continue;
                }

                screen.Draw(gameTime);
            }
        }

        //add a new screen
        public void AddScreen(GameScreen screen)
        {
            screen.ScreenManager = this;
            screen.IsExiting = false;

            //load content
            if (isInitialized)
            {
                screen.Load();
            }

            screens.Add(screen);
            screen.ScreenManager = this;
        }

        //remove a screen
        public void RemoveScreen(GameScreen screen)
        {
            //unload content
            if (isInitialized)
            {
                screen.Unload();
            }

            screens.Remove(screen);
            tempScreensList.Remove(screen);
        }

        //fades screens in and out and darkens popup background
        public void FadeBackBufferToBlack(float alpha)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(blankTexture, GraphicsDevice.Viewport.Bounds, Color.Black * alpha);
            spriteBatch.End();
        }

        //SpriteBatch is shared by all the screens
        public SpriteBatch SpriteBatch
        {
            get
            {
                return spriteBatch;
            }
        }

        //SpriteFont shared by all the screens
        public SpriteFont Font
        {
            get
            {
                return font;
            }
        }

        //blank texture that can be used by screens
        public Texture2D BlankTexture
        {
            get
            {
                return blankTexture;
            }
        }

        ////return array copy of screens, for testing/debuggin
        //public GameScreen[] GetScreens()
        //{
        //    return screens.ToArray();
        //}
    }
}
