using System;
using System.Collections.Generic;
using System.Globalization;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Preferences;
using Android.Service.Wallpaper;
using Android.Views;
using Java.Lang;
using Microsoft.Xna.Framework;
using Exception = System.Exception;
using Xna = Microsoft.Xna.Framework;


namespace KamGame
{
    //[Service(Label = "@string/ApplicationName", Permission = "android.permission.BIND_WALLPAPER")]
    //[IntentFilter(new[] { "android.service.wallpaper.WallpaperService" })]
    //[MetaData("android.service.wallpaper", Resource = "@xml/wallpaper")]
    public abstract partial class GameWallpaperService : WallpaperService
    {
//        protected LogWriter Log;

//        protected GameWallpaperService()
//        {
//#if DEBUG
//            Log = new LogWriter("KamGame.GameWallpaper");
//#endif
//        }

        public static bool PreferenceActivityIsActive;

        public override Engine OnCreateEngine()
        {
            return new GameEngine(this);
        }

        protected abstract Game2D NewGame();

        protected virtual void ApplyPreferences(ISharedPreferences p) { }

        //public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        //{
        //    return Log.Try("OnStartCommand: " + intent.Action, () => base.OnStartCommand(intent, flags, startId));
        //}

        protected virtual void ShowSettings()
        {
            var intent = new Intent(this, GetSettingsActivityType());
            intent.AddFlags(ActivityFlags.NewTask);
            StartActivity(intent);
        }

        protected abstract Type GetSettingsActivityType();
    }


    public abstract class GameWallpaperService<TGame> : GameWallpaperService
        where TGame : Game2D, new()
    {
        public TGame Game { get; private set; }

        protected override Game2D NewGame()
        {
            return Game = new TGame();
        }
    }


}