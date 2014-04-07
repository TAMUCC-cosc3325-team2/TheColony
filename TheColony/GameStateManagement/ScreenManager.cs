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
        List<Screen> screens = new List<Screen>();
        List<Screen> tempScreensList = new List<Screen>();

        //monitors player input
        public InputState input = new InputState();

        public Game game;
        bool isInitialized;

        #region SpriteBatch is shared by all the screens
        
        SpriteBatch spriteBatch;
        
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        #endregion

        #region SpriteFonts shared by all menu screens

        SpriteFont headerFont;
        SpriteFont textFont;
        
        public SpriteFont HeaderFont
        {
            get { return headerFont; }
        }
        public SpriteFont TextFont
        {
            get { return textFont; }
        }

        #endregion

        //constructor
        public ScreenManager(Game game) : base(game)
        {
            this.game = game;        
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
            headerFont = content.Load<SpriteFont>("headerFont");
            textFont = content.Load<SpriteFont>("textFont");

            //load each screen's content
            foreach (Screen screen in screens)
            {
                screen.Activate();
            }
        }

        protected override void UnloadContent()
        {
            //unload each screen's content
            foreach (Screen screen in screens)
            {
                screen.Unload();
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) == true) { game.Exit(); }

            //read the keyboard
            input.Update();

            //make a copy of the screen list if updating one scene adds/removes others
            tempScreensList.Clear();

            foreach (Screen screen in screens) { tempScreensList.Add(screen); }

            //checks if current screen is active
            bool otherScreenHasFocus = !Game.IsActive;
            bool coveredByOtherScreen = false;

            //iterates for each screen waiting to update
            while (tempScreensList.Count > 0)
            {
                //pop the topmost screen
                Screen screen = tempScreensList[tempScreensList.Count - 1];
                tempScreensList.RemoveAt(tempScreensList.Count - 1);

                //update screen
                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                if (screen.ScreenState == ScreenState.Active)
                {
                    //first active screen will handle input
                    if (!otherScreenHasFocus)
                    {
                        screen.HandleInput(gameTime, input);
                        otherScreenHasFocus = true;
                    }

                    coveredByOtherScreen = true;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Screen screen in screens)
            {
                //draw only visible screens
                if (screen.ScreenState == ScreenState.Hidden) { continue; }

                screen.Draw(gameTime);
            }
        }

        #region add/remove screens
        
        public void AddScreen(Screen screen)
        {
            screen.ScreenManager = this;
            
            //load content
            if (isInitialized)
            {
                screen.Activate();
            }

            screens.Add(screen);
            screen.ScreenManager = this;
        }

        public void RemoveScreen(Screen screen)
        {
            //unload content
            if (isInitialized)
            {
                screen.Unload();
            }

            screens.Remove(screen);
            tempScreensList.Remove(screen);
        }
        
        #endregion
    }
}
