using System;
using System.Collections.Generic;
using System.Text;
using Android.Content;


namespace Microsoft.Xna.Framework
{
    partial class AndroidGamePlatform
    {
        // Добавить в AndroidGamePlatform(Game game)
        protected AndroidGameWindow NewWindow(Game game)
        {
            return new AndroidGameWindow(Game.Activity, game);
            //return new AndroidGameWindow(Game.Context ?? Game.Activity, game);
        }
    }

    partial class Game
    {
        public static Context Context;
    }

}
