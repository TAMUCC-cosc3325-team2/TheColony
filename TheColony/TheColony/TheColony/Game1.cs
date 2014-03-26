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

namespace TheColony
{
    //scenes
    public enum Screen
    {
        MenuScreen,
        MapScreen,
        CharacterScreen,
        GameScreen,
        GameOverScreen
    }

    
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //skin cursor
        private Texture2D cursorTexture;
        public Vector2 cursorPosition;

        //instantiate scenes
        MenuScreen menuScreen;
        MapScreen mapScreen;
        CharacterScreen characterScreen;
        GameScreen gameScreen;
        GameOverScreen gameOverScreen;

        Screen currentScreen;

        //fonts
        public SpriteFont buttonFont;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //set game to fullscreen
            graphics.IsFullScreen = true;
            
            //set resolution to current screen resolution
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            
            //enable mouse
            //IsMouseVisible = true; 
        }

        protected override void Initialize()
        {
            //set default screen to main menu
            menuScreen = new MenuScreen(this);
            currentScreen = Screen.MenuScreen;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            //load custom cursor
            cursorTexture = Content.Load<Texture2D>("Pointer");
            //load button font
            buttonFont = Content.Load<SpriteFont>("ButtonSpriteFont");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            //quit game if Escape is pressed
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) == true)
            {
                this.Exit();
            }

            //update according to current scene
            switch (currentScreen)
            {
                case Screen.MenuScreen:
                    menuScreen.Update();
                    break;
/*                case Screen.MapScreen:
                    mapScreen.Update();
                    break;
                case Screen.CharacterScreen:
                    characterScreen.Update();
                    break;
*/
                case Screen.GameScreen:
                    gameScreen.Update();
                    break;
/*
                case Screen.GameOverScreen:
                    gameOverScreen.Update();
                    break;
*/
            }

            cursorPosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //spriteBatch.Begin();

            
            //draw according to current scene
            switch (currentScreen)
            {
                case Screen.MenuScreen:
                    menuScreen.Draw();
                    break;
/*                case Screen.MapScreen:
                    mapScreen.Draw(spriteBatch);
                    break;
                case Screen.CharacterScreen:
                    characterScreen.Draw(spriteBatch);
                    break;
*/
                case Screen.GameScreen:
                    gameScreen.Draw();
                    break;
/*
                case Screen.GameOverScreen:
                    gameOverScreen.Draw(spriteBatch);
                    break;
*/
            }

            //draw cursor on top
            //spriteBatch.Draw(cursorTexture, cursorPosition, Color.White);
            
            //spriteBatch.End();

            base.Draw(gameTime);
        }

        //instantiate scene classes and remove loaded scene object
        public void switchScreen(Screen newScreen)
        {
            switch (newScreen)
            {
                case Screen.MenuScreen:
                    menuScreen = new MenuScreen(this);
                    
                    mapScreen = null;
                    characterScreen = null;
                    gameScreen = null;
                    gameOverScreen = null;
                    break;
/*                case Screen.MapScreen:
                    mapScreen = new MapScreen(this);
                    
                    menuScreen = null;
                    characterScreen = null;
                    gameScreen = null;
                    gameOverScreen = null;
                    break;
                case Screen.CharacterScreen:
                    characterScreen = new CharacterScreen(this);
                    
                    menuScreen = null;
                    mapScreen = null;
                    gameScreen = null;
                    gameOverScreen = null;
                    break;
*/
                case Screen.GameScreen:
                    gameScreen = new GameScreen(this);
                    
                    menuScreen = null;
                    mapScreen = null;
                    characterScreen = null;
                    gameOverScreen = null;
                    break;
/*
                case Screen.GameOverScreen:
                    gameOverScreen = new GameOverScreen(this);
                    
                    menuScreen = null;
                    mapScreen = null;
                    characterScreen = null;
                    gameScreen = null;
                    break;
*/
            }

            //set currentScreen to new screen
            currentScreen = newScreen;
        }
    }
}
