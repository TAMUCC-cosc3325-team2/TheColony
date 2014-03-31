using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameStateManagement
{
    //enumerates transition states
    public enum ScreenState
    {
        TransitionOn,
        Active,
        TransitionOff,
        Hidden,
    }

    public abstract class GameScreen
    {
        TimeSpan transitionOnTime = TimeSpan.Zero;  //transition on timer
        TimeSpan transitionOffTime = TimeSpan.Zero; //transition ff timer
        float transitionPosition = 1;
        ScreenState screenState = ScreenState.TransitionOn;
        bool isExiting = false; //checks if scene is exiting
        bool otherScreenHasFocus;   //checks if screen has focus
        ScreenManager screenManager;    //holds screen manager it belongs to

        //load content when screen is added
        public virtual void Load()
        {

        }

        //unload content when the screen is removed from the screen manager
        public virtual void Unload()
        {

        }

        //called when screen is active, hidden, or transitioning
        public virtual void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            this.otherScreenHasFocus = otherScreenHasFocus;

            //transition off if screen is exiting
            if (isExiting)
            {
                screenState = ScreenState.TransitionOff;

                if (!UpdateTransition(gameTime, transitionOffTime, 1))
                {
                    //remove the screen when the transition finishes
                    ScreenManager.RemoveScreen(this);
                }
            }
            //transition off if the screen is covered
            else if (coveredByOtherScreen)
            {
                if (UpdateTransition(gameTime, transitionOffTime, 1))
                {
                    //transitioning
                    screenState = ScreenState.TransitionOff;
                }
                else
                {
                    //transition finished
                    screenState = ScreenState.Hidden;
                }
            }
            //transition on and become active
            else
            {
                if (UpdateTransition(gameTime, transitionOnTime, -1))
                {
                    //transitioning
                    screenState = ScreenState.TransitionOn;
                }
                else
                {
                    //transition finished
                    screenState = ScreenState.Active;
                }
            }
        }

        public virtual void Draw(GameTime gameTime)
        {

        }

        //gets the manager that this screen belongs to
        public ScreenManager ScreenManager
        {
            get
            {
                return screenManager;
            }

            set
            {
                screenManager = value;
            }
        }

        //how long screen takes to transition on when activated
        public TimeSpan TransitionOnTime
        {
            get
            {
                return transitionOnTime;
            }

            set
            {
                transitionOnTime = value;
            }
        }

        //how long screen takes to transition off when deactivated
        public TimeSpan TransitionOffTime
        {
            get
            {
                return transitionOffTime;
            }

            set
            {
                transitionOffTime = value;
            }
        }

        //get position of the screen transition, from 0 (fully active) to 1 (transitioned fully off)
        public float TransitionPosition
        {
            get
            {
                return transitionPosition;
            }

            set
            {
                transitionPosition = value;
            }
        }

        //get current alpha of the screen transition, from 1 (fully active) to 0 (transitioned fully off)
        public float TransitionAlpha
        {
            get
            {
                return 1f - TransitionPosition;
            }
        }

        //get current screen transition state
        public ScreenState ScreenState
        {
            get
            {
                return screenState;
            }

            set
            {
                screenState = value;
            }
        }

        //indicates whether the screen is exiting
        public bool IsExiting
        {
            get
            {
                return isExiting;
            }

            set
            {
                isExiting = value;
            }
        }

        public bool IsActive
        {
            get
            {
                return !otherScreenHasFocus && (screenState == ScreenState.TransitionOn || screenState == ScreenState.Active);
            }
        }

        //deactivate screen. called when the game is deactivated
        public virtual void Deactivate()
        {

        }

        //updates screen transition position
        bool UpdateTransition(GameTime gameTime, TimeSpan time, int direction)
        {
            //move rate
            float transitionDelta;

            if (time == TimeSpan.Zero)
            {
                transitionDelta = 1;
            }
            else
            {
                transitionDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds / time.TotalMilliseconds);
            }

            //update transition position
            transitionPosition += transitionDelta * direction;

            //check for end of transition
            if (((direction < 0) && (transitionPosition <= 0)) || ((direction > 0) && (transitionPosition >= 1)))
            {
                transitionPosition = MathHelper.Clamp(transitionPosition, 0, 1);
                //transition finished
                return false;
            }

            //still transitioning
            return true;
        }

        //allows screen to handle user input. called when the screen is active and has focus
        public virtual void HandleInput(GameTime gameTime, InputState input)
        {

        }

        //removes screen
        public void ExitScreen()
        {
            if (TransitionOffTime == TimeSpan.Zero)
            {
                //remove it if transition time is 0
                ScreenManager.RemoveScreen(this);
            }
            else
            {
                //flag that it is transition off and exiting
                isExiting = true;
            }
        }
    }
}
