using Android.App;
using Android.OS;
using Android.Preferences;
using KamGame;
using Microsoft.Xna.Framework;


namespace FallenLeaves
{

    [Activity(Label = "Fallen Leaves"
        , Icon = "@drawable/icon"
        , Exported = true
        , Permission = "android.permission.BIND_WALLPAPER",
        Theme = "@android:style/Theme.WallpaperSettings")]
    public class WallpaperSettings : PreferenceActivity 
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            AddPreferencesFromResource(Resource.Xml.preferences);
#if FREE_VERSION
            foreach (var name in new[] { "sky", "wind", "wind_dir", "wind_show", "fallen_leafs_count", "fallen_leafs_scale", })
            {
                var p = FindPreference(name);
                p.Enabled = false;
                p.Summary += " (available only in the paid version)";
            }
#endif
        }

        protected override void OnResume()
        {
            base.OnResume();
            GameWallpaperService.PreferenceActivityIsActive = true;
            AndroidGameActivity.DoResumed();
        }


        protected override void OnPause()
        {
            GameWallpaperService.PreferenceActivityIsActive = false;
            base.OnPause();
        }
    }
}
