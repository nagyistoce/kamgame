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
#if FREE_VERSION
            AddPreferencesFromResource(Resource.Xml.preferences_free);
#else
            AddPreferencesFromResource(Resource.Xml.preferences);
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
            //Finish();
        }
    }
}
