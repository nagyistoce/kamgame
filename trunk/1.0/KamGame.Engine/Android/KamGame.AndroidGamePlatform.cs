using System;
using System.Collections.Generic;
using Android.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Microsoft.Xna.Framework
{
    partial class AndroidGamePlatform
    {
        // HACK Добавить в AndroidGamePlatform(Game game)
        protected AndroidGameWindow NewWindow(Game game)
        {
            //return new AndroidGameWindow(Game.Activity, game);
            return new AndroidGameWindow(Game.Context ?? Game.Activity, game);
        }

        public void Finish()
        {
            AndroidGameActivity.Paused -= Activity_Paused;
            AndroidGameActivity.Resumed -= Activity_Resumed;
        }
    }

    partial class Game
    {
        public static Context Context;

        public void Finish()
        {
            AndroidGameActivity.DoPaused();
            Exit();
            Dispose();
            var platform = (AndroidGamePlatform)Platform;
            platform.Finish();
            if (CustomHolder != null)
                CustomHolder.RemoveCallback(platform.Window);
            Effect.FlushCache();
            Context = null;
            CustomHolder = null;
            _instance = null;
        }

    }

}
