using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace TheColony
{
    class Samantha : Character
    {
        public Samantha(Game1 game)
            : base(game)
        {
            name = "Samantha";
            age = 34;
            birthPlace = "Seward, AK";
            occupation = "Teacher";
            education = "Bachelor of Science in Education";
            backstory = "Having grown up the oldest in a large family Samantha loved being around children. " +
            "Samantha was a great elementary school teacher and spent all her extra time helping underprivileged kids. " +
            "After the event Samantha has been helping the children.";
            health = 100;
            damage = 0;
            dealsDamage = false;
            isUnconscious = false;
            isAvailable = true;
            characterSprite = game.Content.Load<Texture2D>("radSprite");
        }
    }
}
