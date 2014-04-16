using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TheColony
{
    class Leroy_Jenkins : Character
    {
        public Leroy_Jenkins(Game game)
            : base(game)
        {
            name = "Leroy Jenkins";
            age = 20;
            birthPlace = "Flag Staff, AR";
            occupation = "Athlete";
            education = "Unknown";
            backstory = "Leroy, was a very good athlete with a bright future ahead of him but one unfortunate " +
            "injury took his career away from him while he was getting ready to join the Olympic Games. " +
            "He is now trying to prove the world that he is fully recovered and training even harder than before. " +
            "His athletic body and good health is what keeps him get out of tough situations.";
            health = 100;
            damage = 0;
            dealsDamage = false;
            isUnconscious = false;
            isAvailable = true;
            characterSprite = game.Content.Load<Texture2D>(@"Sprite\Characters\radSprite");
        }
    }
}
