using System;
using Android.App;
using Android.Content;
using KamGame;
using KamGame.Converts;
using KamGame.Wallpapers;


namespace FallenLeaves
{

    [Service(Icon = "@drawable/icon", Label = "@string/ApplicationName", Permission = "android.permission.BIND_WALLPAPER")]
    [IntentFilter(new[] { "android.service.wallpaper.WallpaperService" })]
    [MetaData("android.service.wallpaper", Resource = "@xml/wallpaper")]
    public class FallenLeavesWallpaperService : GameWallpaperService<FallenLeavesGame>
    {
        private FallenLeavesPattern patterns;

        

        protected override void ApplyPreferences(ISharedPreferences p, string key)
        {
            //Log += "ApplyPreferences";

            if (key == "settings_on_3taps")
            {
                UseShowSettingsOnTripleTapping = p.GetBoolean(key, true);
                return;
            }
            if (key == "wind_show")
            {
                Wind.ShowBar = p.GetBoolean("wind_show", false);
                return;
            }

#if XLARGE
            FallenLeavesPattern.TreeSizeFactor = 2;
#endif

            if (patterns == null)
                patterns = new FallenLeavesPattern();

            Game.StartScene("Autumn01", patterns.NewScene(

                skyId: p.GetString("sky", "sky4"),
                cloudsCount: p.GetString("clouds_count", "1").ToFloat(),
                windId: p.GetString("wind", "0").ToInt(),
                windDirection: p.GetString("wind_dir", "0").ToInt(),

                grassCount: p.GetString("grass_count", "1").ToFloat(),

                layoutId: p.GetString("layout", "0").ToInt(),
                fallenLeafsCount: p.GetString("fallen_leafs_count", "1").ToFloat(),
                fallenLeafsScale: p.GetString("fallen_leafs_scale", "1").ToFloat()

            ));

            //Log--;
        }

        protected override Type GetSettingsActivityType()
        {
            return typeof(WallpaperSettings);
        }
    }

}
