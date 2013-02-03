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

        protected override void ApplyPreferences(ISharedPreferences p, string key)
        {
            //Log += "ApplyPreferences";

            if (key == null)
                FallenLeavesPattern.RecreateScene = () => ApplyPreferences(p, null);

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

            var scene = FallenLeavesPattern.NewScene(

                textureQuality: p.GetString("textureQuality", "0").ToInt(),

                skyId: p.GetString("sky", "sky4"),
                cloudsCount: p.GetString("clouds_count", "1").ToFloat(),
                windId: p.GetString("wind", "0").ToInt(),
                windDirection: p.GetString("wind_dir", "0").ToInt(),

                grassCount: p.GetString("grass_count", "1").ToFloat(),

                layoutId: p.GetString("layout", "0").ToInt(),
                fallenLeafsCount: p.GetString("fallen_leafs_count", "1").ToFloat(),
                fallenLeafsScale: p.GetString("fallen_leafs_scale", "1").ToFloat()

            );

            Game.StartScene("Autumn01", scene);

            //Log--;
        }

        protected override Type GetSettingsActivityType()
        {
            return typeof(WallpaperSettings);
        }
    }

}
