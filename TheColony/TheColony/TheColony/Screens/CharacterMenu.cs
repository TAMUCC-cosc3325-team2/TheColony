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
    public class CharacterMenu : Screen
    {
        int Scenario;
        SpriteFont headerFont, textFont;
        String header = "Select Character";
        Texture2D card10;
        Texture2D card1;
        Texture2D card2;
        Texture2D card3;
        Texture2D card4;
        Texture2D card5;
        Texture2D card6;
        Texture2D card7;
        Texture2D card8;
        Texture2D card9;
        Rectangle cardPos10;
        Rectangle cardPos1;
        Rectangle cardPos2;
        Rectangle cardPos3;
        Rectangle cardPos4;
        Rectangle cardPos5;
        Rectangle cardPos6;
        Rectangle cardPos7;
        Rectangle cardPos8;
        Rectangle cardPos9;
        
        //menu item colors
        Color normal = Color.White;
        Color selected = Color.Orange;
        
        #region vertical menu attributes
        
        //menu attributes
        String[] vMenuItems = { "Wait in Lobby", "Back", "Exit Game" };
        int vSelectedIndex;
        //holds position and dimensions of menu items
        Vector2 vPosition;
        float vWidth = 0;
        float vHeight = 0;
        
        #endregion

        #region horizontal menu attributes

        //menu attributes
        String[] hMenuItems = { "Character 1", "Character 2", "Character 3", "Character 4", "Character 5", "Character 6", "Character 7", "Character 8" };
        int hSelectedIndex;
        //holds position and dimensions of menu items
        Vector2 hPosition;
        float hWidth = 0;
        float hHeight = 0;

        #endregion

        public CharacterMenu(int s) {
            Scenario = s;
        }

        public override void Activate()
        {
            //backgroundTexture = ScreenManager.Game.Content.Load<Texture2D>("GameMenu");
            headerFont = ScreenManager.HeaderFont;
            textFont = ScreenManager.TextFont;
            vMeasureMenu();
            hMeasureMenu();

            //center card
            cardPos5 = new Rectangle((ScreenManager.GraphicsDevice.Viewport.Width - 484) / 2, (ScreenManager.GraphicsDevice.Viewport.Height - 247) / 2, 484, 247);
            //cards to the right
            cardPos6 = new Rectangle(cardPos5.Right, cardPos5.Center.Y - 50, 200, 102);
            cardPos7 = new Rectangle(cardPos6.Right, cardPos6.Bottom - 89, 200, 102);
            cardPos8 = new Rectangle(cardPos7.Right, cardPos7.Bottom - 89, 200, 102);
            cardPos9 = new Rectangle(cardPos8.Right, cardPos8.Bottom - 89, 200, 102);
            cardPos10 = new Rectangle(cardPos9.Right, cardPos9.Bottom - 89, 200, 102);
            //cards to the left
            cardPos4 = new Rectangle(cardPos5.Left - 200, cardPos5.Center.Y - 50, 200, 102);
            cardPos3 = new Rectangle(cardPos4.Left - 200, cardPos4.Bottom - 89, 200, 102);
            cardPos2 = new Rectangle(cardPos3.Left - 200, cardPos3.Bottom - 89, 200, 102);
            cardPos1 = new Rectangle(cardPos2.Left - 200, cardPos2.Bottom - 89, 200, 102);

            hSelectedIndex = 1;
            setCardPos(1);
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
                        /* passes scenario and character to game screen. base defaults to 0
                         * 0: Richard Blase
                         * 1: Samantha Villarreal
                         * 2: Michael Canders
                         * 3: Leroy Jenkins
                         * 4: Cara Diego
                         * 5: Steven Howler
                         * 6: JR Wilson
                         * 7: Jessica Craft
                         * 8: Shawna Johnson
                         * 9: Tyson Beetley
                         */
                        ScreenManager.AddScreen(new GameScreen(Scenario, hSelectedIndex, 0));
                        break;
                    case 1:
                        ScreenManager.AddScreen(new ScenarionMenu());
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
                if (hSelectedIndex < 1)
                {
                    hSelectedIndex = 10;
                }
                
                setCardPos(hSelectedIndex);
            }
            if (ScreenManager.input.IsNewKeyPress(Keys.Right))
            {
                hSelectedIndex++;
                if (hSelectedIndex > 10)
                {
                    hSelectedIndex = 1;
                }

                setCardPos(hSelectedIndex);
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

            //display horizontal menu
            spriteBatch.Draw(card1, cardPos1, new Color(55, 55, 55));
            spriteBatch.Draw(card2, cardPos2, new Color(105, 105, 105));
            spriteBatch.Draw(card3, cardPos3, new Color(155, 155, 155));
            spriteBatch.Draw(card4, cardPos4, new Color(205, 205, 205));
            spriteBatch.Draw(card5, cardPos5, Color.White);
            spriteBatch.Draw(card6, cardPos6, new Color(205, 205, 205));
            spriteBatch.Draw(card7, cardPos7, new Color(155, 155, 155));
            spriteBatch.Draw(card8, cardPos8, new Color(105, 105, 105));
            spriteBatch.Draw(card9, cardPos9, new Color(55, 55, 55));
            spriteBatch.Draw(card10, cardPos10, new Color(5, 5, 5));
            
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

        #region set card positions
        private void setCardPos(int card)
        {
            switch (card)
            {
                case 1:
                    card5 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard1");
                    card6 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard2");
                    card7 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard3");
                    card8 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard4");
                    card9 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard5");
                    card10 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard6");
                    card1 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard7");
                    card2 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard8");
                    card3 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard9");
                    card4 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard10");
                    break;
                case 2:
                    card4 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard1");
                    card5 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard2");
                    card6 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard3");
                    card7 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard4");
                    card8 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard5");
                    card9 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard6");
                    card10 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard7");
                    card1 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard8");
                    card2 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard9");
                    card3 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard10");
                    break;
                case 3:
                    card3 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard1");
                    card4 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard2");
                    card5 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard3");
                    card6 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard4");
                    card7 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard5");
                    card8 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard6");
                    card9 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard7");
                    card10 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard8");
                    card1 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard9");
                    card2 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard10");
                    break;
                case 4:
                    card2 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard1");
                    card3 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard2");
                    card4 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard3");
                    card5 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard4");
                    card6 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard5");
                    card7 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard6");
                    card8 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard7");
                    card9 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard8");
                    card10 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard9");
                    card1 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard10");
                    break;
                case 5:
                    card1 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard1");
                    card2 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard2");
                    card3 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard3");
                    card4 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard4");
                    card5 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard5");
                    card6 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard6");
                    card7 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard7");
                    card8 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard8");
                    card9 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard9");
                    card10 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard10");
                    break;
                case 6:
                    card10 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard1");
                    card1 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard2");
                    card2 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard3");
                    card3 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard4");
                    card4 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard5");
                    card5 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard6");
                    card6 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard7");
                    card7 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard8");
                    card8 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard9");
                    card9 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard10");
                    break;
                case 7:
                    card9 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard1");
                    card10 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard2");
                    card1 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard3");
                    card2 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard4");
                    card3 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard5");
                    card4 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard6");
                    card5 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard7");
                    card6 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard8");
                    card7 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard9");
                    card8 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard10");
                    break;
                case 8:
                    card8 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard1");
                    card9 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard2");
                    card10 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard3");
                    card1 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard4");
                    card2 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard5");
                    card3 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard6");
                    card4 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard7");
                    card5 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard8");
                    card6 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard9");
                    card7 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard10");
                    break;
                case 9:
                    card7 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard1");
                    card8 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard2");
                    card9 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard3");
                    card10 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard4");
                    card1 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard5");
                    card2 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard6");
                    card3 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard7");
                    card4 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard8");
                    card5 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard9");
                    card6 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard10");
                    break;
                case 10:
                    card6 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard1");
                    card7 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard2");
                    card8 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard3");
                    card9 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard4");
                    card10 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard5");
                    card1 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard6");
                    card2 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard7");
                    card3 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard8");
                    card4 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard9");
                    card5 = ScreenManager.Game.Content.Load<Texture2D>(@"Cards\CharCard10");
                    break;
            }
        }
        #endregion
    }
}

