using Android.App;
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
    }
}
