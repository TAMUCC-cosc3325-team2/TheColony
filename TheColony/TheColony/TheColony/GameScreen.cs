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
    public class GameScreen : Screen
    {
        private Game game;
        private KeyboardState lastKeyboardState;
        private MouseState lastMouseState;
        private Texture2D tHud;
        private Texture2D player_temp;
        private Texture2D background0;
        private Texture2D background1;
        private Texture2D background2;
        private Texture2D background3;
        //private Texture2D background;
        private Texture2D cursor;
        private Texture2D hilight;
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
        private Vector2 hilightPosition;

        private Rectangle tileArea;

        private int TILE_WIDTH;
        private int TILE_HEIGHT;
        private int OFFSET_X;
        private int OFFSET_Y;

        //NOTE: Possibly make a Characters class that has all our character objects instead of using a list
        //it would basically function like a list but can include any needed methods
        private List<Character> characterList;

        public GameScreen()
        {

        }

        public override void Activate()
        {
            game = ScreenManager.game;
            //load textures to be used in game
            tHud = game.Content.Load<Texture2D>(@"UI\gameHUD");
            player_temp = game.Content.Load<Texture2D>(@"Sprite\radSprite_temp");   //temporary sprite to test movement
            cursor = game.Content.Load<Texture2D>(@"UI\Pointer");                   //sprite for cursor
            //background = game.Content.Load<Texture2D>(@"Background\background");    //image for background
            background0 = game.Content.Load<Texture2D>(@"Background\background0");  //larger, better looking background
            background1 = game.Content.Load<Texture2D>(@"Background\background1");  //larger, better looking background
            background2 = game.Content.Load<Texture2D>(@"Background\background2");  //larger, better looking background
            background3 = game.Content.Load<Texture2D>(@"Background\background3");  //larger, better looking background
            hilight = game.Content.Load<Texture2D>(@"UI\TileHilight");              //hilight texture
            debugFont = game.Content.Load<SpriteFont>(@"Font\DebugFont");           //font for debug info
            
            //currently not used but will probably be used eventually
            lastKeyboardState = Keyboard.GetState();
            //currently not used but will probably be used eventually
            lastMouseState = Mouse.GetState();

            //camera object used for viewing game world
            camera = new Camera();
            spriteBatch = ScreenManager.SpriteBatch;

            //different viewports to display different game elements
            defaultView = game.GraphicsDevice.Viewport;
            gameView = defaultView; 
            gameView.Width = gameView.Width - tHud.Width; //viewport for player's camera


            TILE_HEIGHT = 256;
            TILE_WIDTH = 256;
            OFFSET_X = -1360;
            OFFSET_Y = -1242;

            tileArea = new Rectangle(OFFSET_X, OFFSET_Y, TILE_WIDTH * 10, TILE_HEIGHT * 10);

            #region temporary player attributes just to test player movement 
            playerPosition = new Vector2(700,600);
            playerNewPosition = playerPosition;
            playerOffset = new Vector2(-32, -145);
            //playerPosition -= playerOffset;
            #endregion 

            characterList = new List<Character>();
            addCharacters();        //Adds characters to the game(unfinished)
        }

        public override void Unload() { }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
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
            #region pan cursor
            if (mouse.X < 50)
                camera.Pan(new Vector2(-1, 1));
            else if (mouse.X > gameView.Width - 50 && mouse.X < gameView.Width)
                camera.Pan(new Vector2(1, -1));
            if (mouse.Y < 50 && mouse.X < gameView.Width)
                camera.Pan(new Vector2(-1, -1));
            else if (mouse.Y > gameView.Height - 50 && mouse.X < gameView.Width)
                camera.Pan(new Vector2(1, 1));
            #endregion

            //gets left mouse click and updates player position
            if (mouse.LeftButton == ButtonState.Pressed && mouse.X < gameView.Width)
            {
                playerNewPosition = Vector2.Transform(mousePosition, Matrix.Invert(camera.getTransformation(game.GraphicsDevice)));
                playerNewPosition = playerNewPosition + playerOffset;
            }


            //update tile hilight position
            if (tileArea.Contains((int)mousePosition.X, (int)mousePosition.Y))
            {
                hilightPosition = tileEngine(Vector2.Transform(mousePosition, Matrix.Invert(camera.getTransformation(game.GraphicsDevice))));
                hilightPosition.X = hilightPosition.X * TILE_WIDTH + tileArea.X;
                hilightPosition.Y = hilightPosition.Y * TILE_HEIGHT + tileArea.Y;
            }


            //player movement
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

            base.Update(gameTime, otherScreenHasFocus, false);
        }


        public override void Draw(GameTime gameTime)
        {
            //default viewport, the hud is displayed here
            game.GraphicsDevice.Viewport = defaultView;
            game.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            #region draw hud
            spriteBatch.Draw(tHud, new Rectangle(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 166, 0, 166, 768), Color.White);
            #endregion
            spriteBatch.End();

            //camera's viewport, the game world is displayed here
            game.GraphicsDevice.Viewport = gameView;
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, camera.getTransformation(game.GraphicsDevice));
            #region draw background

            //                                           ______________ ______________
            //                                          |              |              |
            //                                          | background0  |  background1 |
            //backgrounds are displayed in this order:  |______________|______________|
            //Each background is 4096x4096              |              |              |
            //Total the world is 8192x8192              | background2  |  background3 |
            //                                          |______________|______________|
            //
            spriteBatch.Draw(background0, new Vector2(-background0.Width, -background0.Height), Color.White);
            spriteBatch.Draw(background1, new Vector2(0, -background1.Height), Color.White);
            spriteBatch.Draw(background2, new Vector2(-background2.Width, 0), Color.White);
            spriteBatch.Draw(background3, new Vector2(0, 0), Color.White);
            //spriteBatch.Draw(background, new Vector2(-background.Width / 2, -background.Height / 2), Color.White);
            #endregion
            #region draw tile hilight
            if (tileArea.Contains((int)mousePosition.X, (int)mousePosition.Y))
            {
                spriteBatch.Draw(hilight, hilightPosition, Color.White);
            }
            #endregion
            spriteBatch.End();
            spriteBatch.Begin();
            #region draw player
            spriteBatch.Draw(player_temp, Vector2.Transform(playerPosition, camera.getTransformation(game.GraphicsDevice)), Color.White);
            #endregion
            spriteBatch.End();

            //back to default viewport to draw cursor on top of everything
            //debug info is also drawn here
            game.GraphicsDevice.Viewport = defaultView;
            spriteBatch.Begin();
            #region draw debug info
            spriteBatch.DrawString(debugFont, "Mouse Position: " + mouse.ToString(), new Vector2(0, 0), Color.Red);
            spriteBatch.DrawString(debugFont, "Mouse's World Position: " + Vector2.Transform(mousePosition, Matrix.Invert(camera.getTransformation(game.GraphicsDevice))), new Vector2(0, 12), Color.Red);
            spriteBatch.DrawString(debugFont, "Character Position: " + playerPosition, new Vector2(0, 24), Color.Red);
            spriteBatch.DrawString(debugFont, "Tile #: " + tileEngine(Vector2.Transform(mousePosition, Matrix.Invert(camera.getTransformation(game.GraphicsDevice)))), new Vector2(0, 36), Color.Red);
            //spriteBatch.DrawString(debugFont, "TileStartPos: " + Vector2.Transform(new Vector2(737, 1991), camera.getTransformation(game.GraphicsDevice)), new Vector2(0, 36), Color.Red);
            #endregion
            #region draw cursor
            spriteBatch.Draw(cursor, mousePosition, Color.White);
            #endregion
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

        //Temporary "tile engine"
        //unfinished
        //don't judge me
        private Vector2 tileEngine(Vector2 position)
        {
            return new Vector2((int)((position.X - OFFSET_X) / TILE_WIDTH), (int)((position.Y - OFFSET_Y) / TILE_HEIGHT));
        }
    }
}
