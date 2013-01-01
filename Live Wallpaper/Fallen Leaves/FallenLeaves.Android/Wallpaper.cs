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
        Label = "@string/ApplicationName",
        Permission = "android.permission.BIND_WALLPAPER",
        Icon = "@drawable/icon")]
    [IntentFilter(new[] { "android.service.wallpaper.WallpaperService" })]
    [MetaData("android.service.wallpaper", Resource = "@xml/wallpaper")]
    public class FallenLeavesWallpaperService : GameWallpaperService<FallenLeavesGame>
    {
        private FallenLeavesPattern patterns;

        protected override void ApplyPreferences(ISharedPreferences p)
        {
            if (patterns == null)
                patterns = new FallenLeavesPattern();

            Game.StartScene("Autumn01", patterns.NewScene(
                p.GetString("sky", "4").ToInt(),
                p.GetString("layout", "0").ToInt(),
                p.GetString("wind", "0").ToInt(),
                p.GetString("fallen_leafs_count", "1").ToInt()
            )); 
        }

        public override void OnPreferenceChanged(ISharedPreferences p, string key)
        {
            base.OnPreferenceChanged(p, key);
            ApplyPreferences(p);
            AndroidGameActivity.DoResumed();
            PreferencesIsChanged = false;
        }

    }

}
