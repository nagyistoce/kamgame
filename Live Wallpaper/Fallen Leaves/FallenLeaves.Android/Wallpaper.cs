using System.Threading;
using Android.App;
using Android.Content;
using Android.Preferences;
using KamGame;
using KamGame.Converts;
using KamGame.Wallpapers;
using Microsoft.Xna.Framework;


namespace FallenLeaves
{

    [Service(
#if FREE_VERSION
        Label = "@string/ApplicationName_Free",
#else
        Label = "@string/ApplicationName",
#endif
        Permission = "android.permission.BIND_WALLPAPER",
        Icon = "@drawable/icon")]
    [IntentFilter(new[] { "android.service.wallpaper.WallpaperService" })]
    [MetaData("android.service.wallpaper", Resource = "@xml/wallpaper")]
    public class FallenLeavesWallpaperService : GameWallpaperService<FallenLeavesGame>
    {
        private FallenLeavesPattern patterns;

        protected override void ApplyPreferences(ISharedPreferences p)
        {
            Log += "ApplyPreferences";

            if (patterns == null)
                patterns = new FallenLeavesPattern();

            Game.StartScene("Autumn01", patterns.NewScene(

                skyId: p.GetString("sky", "sky4"),
                cloudsCount: p.GetString("clouds_count", "1").ToFloat(),
                windId: p.GetString("wind", "0").ToInt(),
                windDirection: p.GetString("wind_dir", "0").ToInt(),
                windShow: p.GetBoolean("wind_show", false),

                grassCount: p.GetString("grass_count", "1").ToFloat(),

                layoutId: p.GetString("layout", "0").ToInt(),
                fallenLeafsCount: p.GetString("fallen_leafs_count", "1").ToFloat(),
                fallenLeafsScale: p.GetString("fallen_leafs_scale", "1").ToFloat()

            ));

            Log--;
        }

    }

}
