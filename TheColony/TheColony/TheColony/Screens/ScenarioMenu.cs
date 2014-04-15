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
    public class ScenarionMenu : Screen
    {
        SpriteFont headerFont, textFont;
        String header = "Select Apocalypse";
        Texture2D ScenarioBg;

        //menu item colors
        Color normal = Color.White;
        Color selected = Color.Orange;
        
        #region vertical menu attributes
        
        //menu attributes
        String[] vMenuItems = { "Select Player", "Back", "Exit Game" };
        int vSelectedIndex;
        //holds position and dimensions of menu items
        Vector2 vPosition;
        float vWidth = 0;
        float vHeight = 0;
        
        #endregion

        #region horizontal menu attributes

        //menu attributes
        String[] hMenuItems = { "Nuclear Warfare", "Chemical Warfare", "Biblical Apocalypse", "Natural Disaster"};
        int hSelectedIndex;
        //holds position and dimensions of menu items
        Vector2 hPosition;
        float hWidth = 0;
        float hHeight = 0;

        #endregion
        
        public ScenarionMenu() { }

        public override void Activate()
        {
            headerFont = ScreenManager.HeaderFont;
            textFont = ScreenManager.TextFont;
            vMeasureMenu();
            hMeasureMenu();
            //ScenarioBg = ScreenManager.Game.Content.Load<Texture2D>(@"Backgrounds\biblicalwarfare");
            setScenario(0);
        }

        public override void Unload() { }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {

            //move to selected screen
            if (ScreenManager.input.IsNewKeyPress(Keys.Enter))
            {
                switch (vSelectedIndex)
                {
                    case 0:
                        ScreenManager.AddScreen(new CharacterMenu());
                        ScreenManager.RemoveScreen(this);
                        break;
                    case 1:
                        ScreenManager.AddScreen(new MainMenu());
                        ScreenManager.RemoveScreen(this);
                        break;
                    case 2:
                        ScreenManager.game.Exit();
                        break;
                }
            }
            //scrolls through vertical menu depending on key pressed
            if (ScreenManager.input.IsNewKeyPress(Keys.Up))
            {
                vSelectedIndex--;
                if (vSelectedIndex < 0)
                {
                    vSelectedIndex = vMenuItems.Length - 1;
                }
            }
            if (ScreenManager.input.IsNewKeyPress(Keys.Down))
            {
                vSelectedIndex++;
                if (vSelectedIndex == vMenuItems.Length)
                {
                    vSelectedIndex = 0;
                }
            }
            //scrolls through horizontal menu depending on key pressed
            if (ScreenManager.input.IsNewKeyPress(Keys.Left))
            {
                hSelectedIndex--;
                if (hSelectedIndex < 0)
                {
                    hSelectedIndex = hMenuItems.Length - 1;
                    hPosition.X = hPosition.X - (hWidth * ((hMenuItems.Length - 1) * 2));
                }
                else
                {
                    hPosition.X = hPosition.X + (hWidth * 2);
                }
                setScenario(hSelectedIndex);
            }
            if (ScreenManager.input.IsNewKeyPress(Keys.Right))
            {
                hSelectedIndex++;
                if (hSelectedIndex == hMenuItems.Length)
                {
                    hSelectedIndex = 0;
                    hPosition.X = hPosition.X + (hWidth * ((hMenuItems.Length - 1) * 2));
                }
                else
                {
                    hPosition.X = hPosition.X - (hWidth * 2);
                }
                setScenario(hSelectedIndex);
            }
            
            base.Update(gameTime, otherScreenHasFocus, false);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 location = vPosition;
            Color tint;
            
            spriteBatch.Begin();

            spriteBatch.Draw(ScenarioBg, new Rectangle(Convert.ToInt32(viewport.Width * .25), Convert.ToInt32(viewport.Height * .25), Convert.ToInt32(viewport.Width * .5), Convert.ToInt32(viewport.Height * .5)), Color.White);

            //display title
            spriteBatch.DrawString(headerFont, header, new Vector2((ScreenManager.GraphicsDevice.Viewport.Width - headerFont.MeasureString(header).X) / 2, (ScreenManager.GraphicsDevice.Viewport.Height / 4) - (headerFont.MeasureString(header).Y / 2)), Color.White);
            
            //display vertical menu
            for (int i = 0; i < vMenuItems.Length; i++)
            {
                if (i == vSelectedIndex)
                {
                    tint = selected;
                }
                else
                {
                    tint = normal;
                }
                spriteBatch.DrawString(textFont, vMenuItems[i], location, tint);
                location.Y += textFont.LineSpacing + 5;
            }

            location = hPosition;

            //display horizontal menu
            for (int i = 0; i < hMenuItems.Length; i++)
            {
                if (i == hSelectedIndex)
                {
                    tint = selected;
                }
                else
                {
                    tint = normal;
                }
                spriteBatch.DrawString(textFont, hMenuItems[i], location, tint);
                location.X += (hWidth * 2);
            }

            spriteBatch.End();
        }

        #region calculate vertical menu dimensions
        
        private void vMeasureMenu()
        {
            vHeight = 0;
            vWidth = 0;
            
            //iterate for each menu item
            foreach (string item in vMenuItems)
            {
                Vector2 size = textFont.MeasureString(item);

                //set the width to the greatest width among menu items for centering purposes
                if (size.X > vWidth)
                {
                    vWidth = size.X;
                }

                //set the height equal to all menu items stacked
                vHeight += textFont.LineSpacing + 5;
            }

            vPosition = new Vector2((ScreenManager.GraphicsDevice.Viewport.Width - vWidth) / 2, ((ScreenManager.GraphicsDevice.Viewport.Height * 3) / 4) - (vHeight / 2));
        }
        
        #endregion

        #region calculate horizontal menu dimensions

        private void hMeasureMenu()
        {
            hHeight = textFont.LineSpacing;
            hWidth = 0;

            //iterate for each menu item
            foreach (string item in hMenuItems)
            {
                float size = textFont.MeasureString(item).X;

                if (size > hWidth)
                {
                    hWidth = size;
                }
            }

            hPosition = new Vector2(((ScreenManager.GraphicsDevice.Viewport.Width - hWidth) / 2), (ScreenManager.GraphicsDevice.Viewport.Height - hHeight) / 2);
        }

        #endregion

        private void setScenario(int index)
        {
            switch (index)
            {
                case 0:
                    ScenarioBg = ScreenManager.Game.Content.Load<Texture2D>(@"Backgrounds\NuclearExplosion");
                    break;
                case 1:
                    ScenarioBg = ScreenManager.Game.Content.Load<Texture2D>(@"Backgrounds\chemicalwarfare");
                    break;
                case 2:
                    ScenarioBg = ScreenManager.Game.Content.Load<Texture2D>(@"Backgrounds\biblicalwarfare");
                    break;
                case 3:
                    ScenarioBg = ScreenManager.Game.Content.Load<Texture2D>(@"Backgrounds\naturaldisaster");
                    break;
            }
        }
    }
}

