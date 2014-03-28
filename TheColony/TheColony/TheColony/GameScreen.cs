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
        private List<Character> characterList;
        
        public GameScreen(Game1 game)
        {
            this.game = game;

            //load textures to be used in game
            tHud = game.Content.Load<Texture2D>("gameHUD");
            player_temp = game.Content.Load<Texture2D>("radSprite_temp");       //temporary sprite to test movement
            cursor = game.Content.Load<Texture2D>("Pointer");                   //sprite for cursor
            radSprite = game.Content.Load<Texture2D>("radSprite");              //radiation sprite sheet(unused currently)
            background = game.Content.Load<Texture2D>("background");            //image for background
            large_background = game.Content.Load<Texture2D>("Game_Background"); //temporary background cuz it looks better
            
            //currently not used but will probably be eventually
            lastKeyboardState = Keyboard.GetState();
            //currently not used but will probably be eventually
            lastMouseState = Mouse.GetState();

            camera = new Camera();          
            spriteBatch = new SpriteBatch(game.GraphicsDevice);

            //different viewports
            defaultView = game.GraphicsDevice.Viewport;
            gameView = defaultView;
            gameView.Width = gameView.Width - tHud.Width; //viewport for player's camera
            
            //cursor = Content.Load<Texture2D>("cursor");
            //fHud = game.Content.Load<SpriteFont>("HudFont");

            #region temporary player attributes just to test player movement 
            playerPosition = new Vector2(700,600);
            playerNewPosition = playerPosition;
            playerOffset = new Vector2(player_temp.Width / 2, player_temp.Height - 10);
            playerPosition -= playerOffset;
            #endregion 

            //projection Matrix used for uh... I forgot. Will probably remove soon
            isoProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4, 2.0f / 3.0f, 1.0f, 10000f);

            //addCharacters();        //Adds characters to the game(unfinished
        }

        public void Update()
        {
            
            key = Keyboard.GetState();
            mouse = Mouse.GetState();

            mousePosition = new Vector2(mouse.X, mouse.Y);

            //allows game to exit
            if (key.IsKeyDown(Keys.Escape))
                game.Exit();

            //allows to toggle fullscreen
            if (key.IsKeyDown(Keys.F))
                graphics.ToggleFullScreen();

            //moves camera when cursor is near edge of camera's viewport
            if (mouse.X < 50)
                camera.Pan(new Vector2(-1, 1));
            if (mouse.X > game.GraphicsDevice.DisplayMode.Width - 166 - 50 && mouse.X < game.GraphicsDevice.DisplayMode.Width - 166)
                camera.Pan(new Vector2(1, -1));
            if (mouse.Y < 50 && mouse.X < game.GraphicsDevice.DisplayMode.Width - 166)
                camera.Pan(new Vector2(-1, -1));
            if (mouse.Y > game.GraphicsDevice.DisplayMode.Height - 50 && mouse.X < game.GraphicsDevice.DisplayMode.Width - 166)
                camera.Pan(new Vector2(1, 1));

            //gets left mouse click and updates player position
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                if (mouse.X < game.GraphicsDevice.DisplayMode.Width - 166)
                {
                    playerNewPosition = mousePosition;
                    playerNewPosition = playerNewPosition - playerOffset;
                }
            }

            //player movement, tobe moved later
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

            /* old player movement
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
            //default viewport, the hud is displayed here
            game.GraphicsDevice.Viewport = defaultView;
            game.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.Draw(tHud, new Rectangle(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 166, 0, 166, 768), Color.White);
            //spriteBatch.DrawString(hud, "PUT", new Vector2(GraphicsDevice.DisplayMode.Width - 250, GraphicsDevice.DisplayMode.Height / 2 - 140), Color.White);
            //spriteBatch.DrawString(hud, "HUD", new Vector2(GraphicsDevice.DisplayMode.Width - 250, GraphicsDevice.DisplayMode.Height / 2 - 70), Color.White);
            //spriteBatch.DrawString(hud, "HERE", new Vector2(GraphicsDevice.DisplayMode.Width - 250, GraphicsDevice.DisplayMode.Height / 2), Color.White);
            spriteBatch.End();

            //camera's viewport, the game world is displayed here
            game.GraphicsDevice.Viewport = gameView;
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, camera.getTransformation(game.GraphicsDevice));
            spriteBatch.Draw(large_background, new Vector2(-large_background.Width/2, -large_background.Height/2), Color.White);
            spriteBatch.End();
            spriteBatch.Begin();
            spriteBatch.Draw(player_temp, playerPosition, Color.White);
            spriteBatch.End();
            
            //back to default viewport to draw cursor on top of everything
            game.GraphicsDevice.Viewport = defaultView;
            spriteBatch.Begin();
            spriteBatch.Draw(cursor, mousePosition, Color.White);
            spriteBatch.End();
            
        }


        //method to add characters to the game
        private void addCharacters()
        {
            //Examples of how to add characters. Info was pulled from trello
            Character Samantha = new Character();
            Samantha.name = "Samantha";
            Samantha.age = 34;
            Samantha.birthPlace = "Seward, AK";
            Samantha.occupation = "Teacher";
            Samantha.education = "Bachelor of Science in Education";
            Samantha.backstory = "Having grown up the oldest in a large family Samantha loved being around children. " +
            "Samantha was a great elementary school teacher and spent all her extra time helping underprivileged kids. " +
            "After the event Samantha has been helping the children.";
            Samantha.health = 100;
            Samantha.damage = 0;
            Samantha.dealsDamage = false;
            Samantha.isUnconscious = false;
            Samantha.isAvailable = true;
            characterList.Add(Samantha);

            Character Richard_B = new Character();
            Richard_B.name = "Richard Blase";
            Richard_B.age = 34;
            Richard_B.birthPlace = "Erie, PA";
            Richard_B.occupation = "Carpenter";
            Richard_B.education = "High School Diploma";
            Richard_B.backstory = "After losing his family to a car accident, Richard gave up on his life long love of carpentry. " +
            "He walked away from all family and friends taking up a life of solitude. " +
            "After the event Richard has been securing shelters to defend against thieves and Murders.";
            Richard_B.health = 100;
            Richard_B.damage = 0;
            Richard_B.dealsDamage = false;
            Richard_B.isUnconscious = false;
            Richard_B.isAvailable = true;
            characterList.Add(Richard_B);

            Character Leroy_J = new Character();
            Leroy_J.name = "Leroy Jenkins";
            Leroy_J.age = 20;
            Leroy_J.birthPlace = "Flag Staff, AR";
            Leroy_J.occupation = "Athlete";
            Leroy_J.education = "Unknown";
            Leroy_J.backstory = "Leroy, was a very good athlete with a bright future ahead of him but one unfortunate " +
            "injury took his career away from him while he was getting ready to join the Olympic Games. " +
            "He is now trying to prove the world that he is fully recovered and training even harder than before. " +
            "His athletic body and good health is what keeps him get out of tough situations.";
            Leroy_J.health = 100;
            Leroy_J.damage = 0;
            Leroy_J.dealsDamage = false;
            Leroy_J.isUnconscious = false;
            Leroy_J.isAvailable = true;
            characterList.Add(Leroy_J);

            //Still more characters to add, but can do that later. 
        }
    }
}
