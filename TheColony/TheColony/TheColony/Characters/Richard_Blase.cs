using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TheColony
{
    class Richard_Blase : Character
    {
        public Richard_Blase(Game game)
            : base(game)
        {
            name = "Richard Blase";
            age = 34;
            birthPlace = "Erie, PA";
            occupation = "Carpenter";
            education = "High School Diploma";
            backstory = "After losing his family to a car accident, Richard gave up on his life long love of carpentry. " +
            "He walked away from all family and friends taking up a life of solitude. " +
            "After the event Richard has been securing shelters to defend against thieves and Murders.";
            health = 100;
            damage = 0;
            dealsDamage = false;
            isUnconscious = false;
            isAvailable = true;
            characterSprite = game.Content.Load<Texture2D>(@"Sprite\radSprite");
        }
    }
}
