using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using KamGame;
using Microsoft.Xna.Framework;

namespace FallenLeaves
{

    [Service(Label = "@string/ApplicationName", Permission = "android.permission.BIND_WALLPAPER")]
    [IntentFilter(new[] { "android.service.wallpaper.WallpaperService" })]
    [MetaData("android.service.wallpaper", Resource = "@xml/cube1")]
    public class FallenLeavesWallpaperService : GameWallpaperService
    {
        protected override GameBase NewGame()
        {
            return new FallenLeavesGame();
        }
    }
}
