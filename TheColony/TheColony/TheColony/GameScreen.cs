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
        private Texture2D background0;
        private Texture2D background1;
        private Texture2D background2;
        private Texture2D background3;
        private Texture2D background;
        private Texture2D cursor;
        private SpriteFont debugFont;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Viewport defaultView;
        private Viewport gameView;
        private Camera camera;

        private KeyboardState key;
        private MouseState mouse;


        //private SpriteFont fHud;

        private Vector2 mousePosition;
        private Vector2 playerPosition;
        private Vector2 playerOffset;
        private Vector2 playerNewPosition;
        private Matrix isoProjectionMatrix;

        //NOTE: Possibly make a Characters class that has all our character objects instead of using a list
        //it would basically function like a list but can include any needed 
        private List<Character> characterList;
        
        public GameScreen(Game1 game)
        {
            this.game = game;

            //load textures to be used in game
            tHud = game.Content.Load<Texture2D>("gameHUD");
            player_temp = game.Content.Load<Texture2D>("radSprite_temp");       //temporary sprite to test movement
            cursor = game.Content.Load<Texture2D>("Pointer");                   //sprite for cursor
            background = game.Content.Load<Texture2D>("background");            //image for background
            background0 = game.Content.Load<Texture2D>("background0"); //larger, better looking background
            background1 = game.Content.Load<Texture2D>("background1"); //larger, better looking background
            background2 = game.Content.Load<Texture2D>("background2"); //larger, better looking background
            background3 = game.Content.Load<Texture2D>("background3"); //larger, better looking background
            debugFont = game.Content.Load<SpriteFont>("DebugFont");
            
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
            playerOffset = new Vector2(-32, -145);
            //playerPosition -= playerOffset;
            #endregion 

            //projection Matrix used for uh... I forgot. Will probably remove soon
            isoProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4, 2.0f / 3.0f, 1.0f, 10000f);

            characterList = new List<Character>();
            addCharacters();        //Adds characters to the game(unfinished)
        }

        public void Update()
        {
            
            key = Keyboard.GetState();
            mouse = Mouse.GetState();

            mousePosition = new Vector2(mouse.X, mouse.Y);

            //allows game to exit
            if (key.IsKeyDown(Keys.Escape))
                game.Exit();

            //allows to toggle fullscreen       **will crash. Probably a problem with the camera...maybe
            //if (key.IsKeyDown(Keys.F))
            //    graphics.ToggleFullScreen();

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
                    playerNewPosition = Vector2.Transform(mousePosition, Matrix.Invert(camera.getTransformation(game.GraphicsDevice)));
                    playerNewPosition = playerNewPosition + playerOffset;
                }
            }


            //old player movement
            if ((playerPosition - playerNewPosition).LengthSquared() > 25)
            {
                Vector2 n = new Vector2();
                n = (playerNewPosition - playerPosition);
                n.Normalize();
                playerPosition += n * 4.5f;
            }
            else
                playerPosition = playerNewPosition;


            //lastKeyboardState = currentKeyboardState;
            //lastMouseState = currentMouseState;
        }

        public void Draw()
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
            //background0-background3 don't look like shit
            //                                           ______________ ______________
            //                                          |              |              |
            //                                          | background0  |  background1 |
            //backgrounds are displayed in this order:  |______________|______________|
            //                                          |              |              |
            //                                          | background2  |  background3 |
            //                                          |______________|______________|   
            //
            //Each background image is 4096x4096, making this a 8192x8192 world. 
            //
            spriteBatch.Draw(background0, new Vector2(-background0.Width, -background0.Height), Color.White);
            spriteBatch.Draw(background1, new Vector2(0, -background1.Height), Color.White);
            spriteBatch.Draw(background2, new Vector2(-background2.Width, 0), Color.White);
            spriteBatch.Draw(background3, new Vector2(0, 0), Color.White);
            //background looks like shit
            //spriteBatch.Draw(background, new Vector2(-background.Width / 2, -background.Height / 2), Color.White);
            spriteBatch.End();
            spriteBatch.Begin();
            spriteBatch.Draw(player_temp, Vector2.Transform(playerPosition, camera.getTransformation(game.GraphicsDevice)), Color.White);
            spriteBatch.End();
            
            //back to default viewport to draw cursor on top of everything
            game.GraphicsDevice.Viewport = defaultView;
            spriteBatch.Begin();
            spriteBatch.DrawString(debugFont, "Mouse Position: " + mouse.ToString(), new Vector2(0, 0), Color.Red);
            spriteBatch.DrawString(debugFont, "Mouse's World Position: " + Vector2.Transform(playerPosition, camera.getTransformation(game.GraphicsDevice)), new Vector2(0, 12), Color.Red);
            spriteBatch.DrawString(debugFont, "Character Position: " + playerPosition.ToString(), new Vector2(0, 24), Color.Red);

            spriteBatch.Draw(cursor, mousePosition, Color.White);
            spriteBatch.End();
            
        }


        //method to add characters to the game
        private void addCharacters()
        {
            characterList.Clear();      //empties list

            //Examples of how to add characters.
            characterList.Add(new Samantha(game));           
            characterList.Add(new Richard_Blase(game));
            characterList.Add(new Leroy_Jenkins(game));

            //Still more characters to add, but can do that later.
            //Possibly get character info from a .dat file
        }
    }
}
