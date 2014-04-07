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
    public class HelpMenu : Screen
    {
        SpriteFont headerFont, textFont;
        String header = "Help";
        String text = "Back";
        
        public HelpMenu() { }

        public override void Activate()
        {
            headerFont = ScreenManager.HeaderFont;
            textFont = ScreenManager.TextFont;
        }
        
        public override void Unload() { }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            //return to main menu
            if (ScreenManager.input.IsNewKeyPress(Keys.Enter))
            {
                ScreenManager.AddScreen(new MainMenu());
                ScreenManager.RemoveScreen(this);
            }
            
            base.Update(gameTime, otherScreenHasFocus, false);
        }
        
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            
            spriteBatch.Begin();
            
            //display title
            spriteBatch.DrawString(headerFont, header, new Vector2((ScreenManager.GraphicsDevice.Viewport.Width - headerFont.MeasureString(header).X) / 2, (ScreenManager.GraphicsDevice.Viewport.Height - headerFont.MeasureString(header).Y) / 4), Color.White);
            
            //display text
            spriteBatch.DrawString(textFont, text, new Vector2((ScreenManager.GraphicsDevice.Viewport.Width - textFont.MeasureString(text).X) / 2, ((ScreenManager.GraphicsDevice.Viewport.Height - textFont.MeasureString(text).Y) * 3) / 4), Color.Orange);
            
            spriteBatch.End();
        }
    }
}