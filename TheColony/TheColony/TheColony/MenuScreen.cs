using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TheColony
{
    public class MenuScreen
    {
        private SpriteBatch spriteBatch;
        private Game1 game;
        private KeyboardState lastKeyboardState;
        private MouseState lastMouseState;
        private Texture2D bg;
        private Texture2D cursorTexture;
        //button
        private Texture2D button;
        private Rectangle buttonRec = new Rectangle((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2) - 102, (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 4 / 5) - 37, 204, 74);
        
        public Vector2 cursorPosition;
        //font
        public SpriteFont menuScreenFont;

        public MenuScreen(Game1 game)
        {
            this.game = game;
            //load background
            bg = game.Content.Load<Texture2D>("GameMenu");
            button = game.Content.Load<Texture2D>("GameMenuButton1");
            //load font
            menuScreenFont = game.Content.Load<SpriteFont>("MenuScreenFont");
            lastKeyboardState = Keyboard.GetState();
            lastMouseState = Mouse.GetState();
            cursorTexture = game.Content.Load<Texture2D>("Pointer");

            spriteBatch = new SpriteBatch(game.GraphicsDevice);
        }

        public void Update()
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();
            MouseState currentMouseState = Mouse.GetState();

            cursorPosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            if (currentMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
            {
                //var cursorPosition2 = new Point(currentMouseState.X, currentMouseState.Y);

                if (buttonRec.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                {
                    game.switchScreen(Screen.GameScreen);
                }
            }

            lastKeyboardState = currentKeyboardState;
            lastMouseState = currentMouseState;
        }

        public void Draw()//SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(bg, new Vector2(0f, 0f), Color.White);
            spriteBatch.Draw(button, buttonRec, Color.White);
            spriteBatch.DrawString(menuScreenFont, "The Colony", new Vector2((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2) - 252, (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2) - 43), Color.White);
            spriteBatch.DrawString(game.buttonFont, "Start Game", new Vector2((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2) - 77, (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 4 / 5) - 20), Color.Black);
            spriteBatch.Draw(cursorTexture, cursorPosition, Color.White);
            spriteBatch.End();
        }
    }
}
