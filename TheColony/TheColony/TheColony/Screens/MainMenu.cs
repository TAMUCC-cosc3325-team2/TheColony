using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GameStateManagement;

namespace TheColony
{
    public class MainMenu : Screen
    {
        SpriteFont headerFont, textFont;
        String header = "The Colony";
        
        #region menu attributes
        
        //menu attributes
        String[] menuItems = { "Start Game", "Help", "Exit Game" };
        int selectedIndex;
        Color normal = Color.White;
        Color selected = Color.Orange;
        //holds position and dimensions of menu items
        Vector2 position;
        float width = 0;
        float height = 0;
        
        #endregion
        
        public MainMenu() { }

        public override void Activate()
        {
            headerFont = ScreenManager.HeaderFont;
            textFont = ScreenManager.TextFont;
            
            //get menu dimensions
            MeasureMenu();
        }
        
        public override void Unload() { }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            //choose selected menu item
            if (ScreenManager.input.IsNewKeyPress(Keys.Enter))
            {
                switch (selectedIndex)
                {
                    case 0:
                        ScreenManager.AddScreen(new ScenarionMenu());
                        ScreenManager.RemoveScreen(this);
                        break;
                    case 1:
                        ScreenManager.AddScreen(new HelpMenu());
                        ScreenManager.RemoveScreen(this);
                        break;
                    case 2:
                        ScreenManager.game.Exit();
                        break;
                }
            }
            //scrolls through menu depending on key pressed
            if (ScreenManager.input.IsNewKeyPress(Keys.Up))
            {
                selectedIndex--;
                if (selectedIndex < 0)
                {
                    selectedIndex = menuItems.Length - 1;
                }
            }
            if (ScreenManager.input.IsNewKeyPress(Keys.Down))
            {
                selectedIndex++;
                if (selectedIndex == menuItems.Length)
                {
                    selectedIndex = 0;
                }
            }
            
            base.Update(gameTime, otherScreenHasFocus, false);
        }
        
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 location = position;
            Color tint;
            Random random = new Random();

            spriteBatch.Begin();
            
            //display title
            spriteBatch.DrawString(headerFont, header, new Vector2((((ScreenManager.GraphicsDevice.Viewport.Width - 437) - 4) / 2) + random.Next(5), (((ScreenManager.GraphicsDevice.Viewport.Height - 164) - 4) / 4) + random.Next(5)), Color.White);
            
            //display menu items
            for (int i = 0; i < menuItems.Length; i++)
            {
                if (i == selectedIndex)
                {
                    tint = selected;
                }
                else
                {
                    tint = normal;
                }
                spriteBatch.DrawString(textFont, menuItems[i], location, tint);
                location.Y += textFont.LineSpacing + 5;
            }

            spriteBatch.End();
        }

        //calculates menu dimension and point of origin 
        private void MeasureMenu()
        {
            height = 0;
            width = 0;
            
            //iterate for each menu item
            foreach (string item in menuItems)
            {
                Vector2 size = textFont.MeasureString(item);

                //set the width to the greatest width among menu items for centering purposes
                if (size.X > width)
                {
                    width = size.X;
                }

                //set the height equal to all menu items stacked
                height += textFont.LineSpacing + 5;
            }
            
            position = new Vector2((ScreenManager.GraphicsDevice.Viewport.Width - width) / 2, ((ScreenManager.GraphicsDevice.Viewport.Height - height) * 3) / 4);
        }
    }
}

