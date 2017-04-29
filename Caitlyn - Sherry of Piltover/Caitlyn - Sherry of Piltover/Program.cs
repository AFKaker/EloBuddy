using System;
using EloBuddy;
using EloBuddy.SDK.Events;

namespace Caitlyn___Sherry_of_Piltover
{
    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += CaitlynLoadingComplete;
        }

        private static void CaitlynLoadingComplete(EventArgs args)
        {
            if (Player.Instance.Hero != Champion.Caitlyn)
            {
                return;
            }
            Caitlyn.Reload();
            PilMenu.Load();
        }
    }
}
