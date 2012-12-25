using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using KamGame;

namespace FallenLeaves
{

    [Service(Label = "@string/ApplicationName", Permission = "android.permission.BIND_WALLPAPER")]
    [IntentFilter(new[] { "android.service.wallpaper.WallpaperService" })]
    [MetaData("android.service.wallpaper", Resource = "@xml/cube1")]
    public class GameWallpaperService : GameWallpaperService<FallenLeavesGame>
    {
        
    }

}
