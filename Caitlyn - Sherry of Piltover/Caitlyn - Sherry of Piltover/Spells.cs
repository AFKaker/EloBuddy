using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using System;

namespace Caitlyn___Sherry_of_Piltover
{
    internal class Spells
    {
        public static Spell.Skillshot Q, W, E;
        public static Spell.Targeted R;

        internal static void Loading()
        {

            Q = new Spell.Skillshot(SpellSlot.Q, 1200, SkillShotType.Linear, 625, 2200, 90)
            {
                AllowedCollisionCount = -1
            };
            W = new Spell.Skillshot(SpellSlot.W, 800, SkillShotType.Circular, 500, int.MaxValue, 20);
            E = new Spell.Skillshot(SpellSlot.E, 800, SkillShotType.Linear, 150, 1600, 80)
            {
                AllowedCollisionCount = 0
            };
            R = new Spell.Targeted(SpellSlot.R, 2000);
        }
    }
}