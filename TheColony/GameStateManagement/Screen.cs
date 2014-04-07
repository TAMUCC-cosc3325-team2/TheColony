using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameStateManagement
{
    #region screen states enumeration

    public enum ScreenState
    {
        Active,
        Hidden,
    }

    #endregion

    public abstract class Screen
    {
        //get/set current screen state
        ScreenState screenState = ScreenState.Active;

        public ScreenState ScreenState
        {
            get { return screenState; }
            protected set { screenState = value; }
        }

        //checks if scene is exiting
        bool isExiting = false;
        
        public bool IsExiting
        {
            get { return isExiting; }
            set { isExiting = value; }
        }

        //checks if screen has focus
        bool otherScreenHasFocus;
        
        public bool IsActive
        {
            get { return !otherScreenHasFocus && screenState == ScreenState.Active; }
        }

        //holds screen manager it belongs to
        ScreenManager screenManager;

        public ScreenManager ScreenManager
        {
            get { return screenManager; }
            set { screenManager = value; }
        }

        //called when screen is added
        public virtual void Activate() { }

        //deactivate screen. called when the game is paused
        public virtual void Deactivate() { }

        //unload content when the screen is removed from the screen manager
        public virtual void Unload() { }

        
        //called regardless if screen is active or hidden
        public virtual void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            this.otherScreenHasFocus = otherScreenHasFocus;

            //remove if screen is exiting
            if (isExiting)
            {
                ScreenManager.RemoveScreen(this);
            }
            //hide if screen is covered
            else if (coveredByOtherScreen)
            {
                screenState = ScreenState.Hidden;
            }
            //becomes active
            else
            {
                screenState = ScreenState.Active;
            }
        }

        //allows screen to handle user input. called when the screen is active and has focus
        public virtual void HandleInput(GameTime gameTime, InputState input) { }

        public virtual void Draw(GameTime gameTime) { }
    }
}
