using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TheColony
{
    public class GameScreen
    {
        private Game1 game;
        private KeyboardState lastKeyboardState;
        private MouseState lastMouseState;
        private Texture2D tHud;
        private Texture2D player_temp;
        private Texture2D large_background;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Viewport defaultView;
        private Viewport gameView;
        private Camera camera;

        private KeyboardState key;
        private MouseState mouse;

        private Texture2D radSprite;
        private Texture2D background;
        private Texture2D cursor;
        //private SpriteFont fHud;

        private Vector2 mousePosition;
        private Vector2 playerPosition;
        private Vector2 playerOffset;
        private Vector2 playerNewPosition;
        private Matrix isoProjectionMatrix;
        
        public GameScreen(Game1 game)
        {
            this.game = game;
            tHud = game.Content.Load<Texture2D>("gameHUD");
            lastKeyboardState = Keyboard.GetState();
            lastMouseState = Mouse.GetState();

            camera = new Camera();
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            defaultView = game.GraphicsDevice.Viewport;
            gameView = defaultView;
            gameView.Width = gameView.Width - tHud.Width;

            player_temp = game.Content.Load<Texture2D>("radSprite_temp");
            cursor = game.Content.Load<Texture2D>("Pointer");
            radSprite = game.Content.Load<Texture2D>("radSprite");
            background = game.Content.Load<Texture2D>("background");
            large_background = game.Content.Load<Texture2D>("Game_Background");
            //cursor = Content.Load<Texture2D>("cursor");
            //fHud = game.Content.Load<SpriteFont>("HudFont");

            playerPosition = new Vector2(700,600);
            playerNewPosition = playerPosition;
            playerOffset = new Vector2(player_temp.Width / 2, player_temp.Height - 10);
            playerPosition -= playerOffset;
            isoProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4, 2.0f / 3.0f, 1.0f, 10000f);
        }

        public void Update()
        {
            key = Keyboard.GetState();
            mouse = Mouse.GetState();

            mousePosition = new Vector2(mouse.X, mouse.Y);

            if (key.IsKeyDown(Keys.Escape))
                game.Exit();

            if (key.IsKeyDown(Keys.F))
                graphics.ToggleFullScreen();

            if (mouse.X < 50)
                camera.Pan(new Vector2(-1, 1));
            if (mouse.X > game.GraphicsDevice.DisplayMode.Width - 166 - 50 && mouse.X < game.GraphicsDevice.DisplayMode.Width - 166)
                camera.Pan(new Vector2(1, -1));
            if (mouse.Y < 50 && mouse.X < game.GraphicsDevice.DisplayMode.Width - 166)
                camera.Pan(new Vector2(-1, -1));
            if (mouse.Y > game.GraphicsDevice.DisplayMode.Height - 50 && mouse.X < game.GraphicsDevice.DisplayMode.Width - 166)
                camera.Pan(new Vector2(1, 1));

            if (mouse.LeftButton == ButtonState.Pressed)
            {
                playerNewPosition = mousePosition;
                playerNewPosition = playerNewPosition - playerOffset;
            }

            if (Math.Abs(playerPosition.X - playerNewPosition.X) < 2)
            {     // the player is already near his destination
                playerPosition.X = playerNewPosition.X;
            }
            if (Math.Abs(playerPosition.X - playerNewPosition.X) > 2)//move the player toward current destination
            {
                playerPosition.X += 2 * Math.Sign(playerNewPosition.X - playerPosition.X);   
            }
            if (Math.Abs(playerPosition.Y - playerNewPosition.Y) != 0)// move the player toward the current destination
            {
                playerPosition.Y += Math.Sign(playerNewPosition.Y - playerPosition.Y);
            }

            /*
            if (playerPosition != playerNewPosition)
            {
                if (playerNewPosition.X - playerPosition.X < 1)
                    playerPosition.X = playerPosition.X - 2;
                else if (playerNewPosition.X - playerPosition.X > 1)
                    playerPosition.X = playerPosition.X + 2;
                if (playerNewPosition.Y - playerPosition.Y < 1)
                    playerPosition.Y = playerPosition.Y - 1;
                else if (playerNewPosition.Y - playerPosition.Y > 1)
                    playerPosition.Y = playerPosition.Y + 1;
                if (playerNewPosition.X - playerPosition.X <= 1 && playerNewPosition.X - playerPosition.X >= -1)
                    playerPosition.X = playerNewPosition.X;
            }*/


            //lastKeyboardState = currentKeyboardState;
            //lastMouseState = currentMouseState;
        }

        public void Draw()//SpriteBatch spriteBatch)
        {
            //hud
            game.GraphicsDevice.Viewport = defaultView;
            game.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.Draw(tHud, new Rectangle(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 166, 0, 166, 768), Color.White);
            //spriteBatch.DrawString(hud, "PUT", new Vector2(GraphicsDevice.DisplayMode.Width - 250, GraphicsDevice.DisplayMode.Height / 2 - 140), Color.White);
            //spriteBatch.DrawString(hud, "HUD", new Vector2(GraphicsDevice.DisplayMode.Width - 250, GraphicsDevice.DisplayMode.Height / 2 - 70), Color.White);
            //spriteBatch.DrawString(hud, "HERE", new Vector2(GraphicsDevice.DisplayMode.Width - 250, GraphicsDevice.DisplayMode.Height / 2), Color.White);
            spriteBatch.End();

            game.GraphicsDevice.Viewport = gameView;
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, camera.getTransformation(game.GraphicsDevice));
            spriteBatch.Draw(large_background, new Vector2(-large_background.Width/2, -large_background.Height/2), Color.White);
            spriteBatch.End();
            spriteBatch.Begin();
            spriteBatch.Draw(player_temp, playerPosition, Color.White);
            spriteBatch.End();
            
            game.GraphicsDevice.Viewport = defaultView;
            spriteBatch.Begin();
            spriteBatch.Draw(cursor, mousePosition, Color.White);
            spriteBatch.End();
            
        }
    }
}
