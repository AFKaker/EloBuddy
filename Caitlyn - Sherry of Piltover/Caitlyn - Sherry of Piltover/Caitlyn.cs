using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace Caitlyn___Sherry_of_Piltover
{
    internal class Caitlyn
    {
        internal static void Reload()
        {
            Spells.Loading();
            
            

        }

        private static void OnAfterAttack(AttackableUnit target, EventArgs args)
        {
            throw new NotImplementedException();
        }

        private static void OnBuffGain(Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}