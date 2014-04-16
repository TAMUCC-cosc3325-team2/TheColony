using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameStateManagement
{
    public class InputState
    {
        public KeyboardState CurrentKeyboardState;
        public KeyboardState LastKeyboardState;

        //constructor
        public InputState()
        {
            CurrentKeyboardState = new KeyboardState();
            LastKeyboardState = new KeyboardState();
        }

        //reads the latest state user input
        public void Update()
        {
            LastKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();
        }

        //checks if a key was pressed
        public bool IsKeyPressed(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key);
        }

        //checks if a key was newly pressed
        public bool IsNewKeyPress(Keys key)
        {
            return (CurrentKeyboardState.IsKeyDown(key) && LastKeyboardState.IsKeyUp(key));
        }
    }
}
