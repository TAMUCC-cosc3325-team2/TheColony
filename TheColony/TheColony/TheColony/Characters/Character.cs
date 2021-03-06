﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheColony
{
    class Character : DrawableGameComponent
    {
        #region character's game-related attributes

        public float health;           //character's current health
        public float damage;           //damage dealt per attack
        public bool dealsDamage;       //true if character has an attack, else false
        public bool isUnconscious;     //true when health reachs 0. if true, character cannot be controlled by player
        public bool isAvailable;       //true when character has yet to be picked by a player, false when character is selected

        public Texture2D characterSprite;

        #endregion

        #region character's lore-related attributes

        public string name;            //character's name
        public int age;                //character's age
        public string birthPlace;      //character's birth place
        public string occupation;    //character's occupation(s)
        public string education;     //character's education
        public string backstory;       //character's backstory

        #endregion

        public Character(Game game)
            : base(game)
        { 
        
        }
    }
}
