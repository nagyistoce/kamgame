using Android.App;
using Android.Content;
using Android.Preferences;
using KamGame;

namespace FallenLeaves
{

    [Service(
        Label = "@string/ApplicationName",
        Permission = "android.permission.BIND_WALLPAPER",
        Icon = "@drawable/icon")]
    [IntentFilter(new[] { "android.service.wallpaper.WallpaperService" })]
    [MetaData("android.service.wallpaper", Resource = "@xml/wallpaper")]
    public class FallenLeavesWallpaperService : GameWallpaperService<FallenLeavesGame>
    {
        protected override void ApplyPreferences(ISharedPreferences p)
        {
            var g = Game;
            //g.UseScene01 = p.GetBoolean("scene01", true);
            //g.UseScene02 = p.GetBoolean("scene02", true);
            //Android.Util.Log.Debug("KamGame.GameWallpaper", "ApplyPreferences");
        }
    }
}
