using System;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Preferences;
using KamGame;
using Microsoft.Xna.Framework;


namespace FallenLeaves
{

    [Activity(
#if FREE_VERSION
        Label = "Fallen Leaves Free Settings"
#else
        Label = "Fallen Leaves Settings"
#endif
        , Icon = "@drawable/icon"
        , Exported = true
        , Permission = "android.permission.BIND_WALLPAPER",
        Theme = "@android:style/Theme.WallpaperSettings")]
    public class WallpaperSettings : PreferenceActivity
    {

        static LogWriter Log = new LogWriter("KamGame");

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

#if FREE_VERSION
            AddPreferencesFromResource(Resource.Xml.preferences_free);

            foreach (var p in new[] { "wind", "wind_dir", "wind_show", "layout", "fallen_leafs_scale" }.Select(FindPreference))
            {
                p.Enabled = false;
                p.Summary = "(It's available only in the full version!)";
            }

#else
            AddPreferencesFromResource(Resource.Xml.preferences);
#endif

            foreach (var key in new[] { "textureQuality", "sky", "clouds_count", "wind", "wind_dir", "wind_show", "grass_count", "layout", "fallen_leafs_count", "fallen_leafs_scale" })
            {
                Log += key;
                var p = FindPreference(key);
                var lp = p as ListPreference;
                if (lp != null)
                {
                    lp.Summary = (lp.GetEntry(lp.SharedPreferences.GetString(key, "")) ?? "<unknown>") + " " + lp.Summary;
                    Log &= lp.Summary;

                    if (lp.Enabled)
                        lp.PreferenceChange += (sender, args) =>
                        {
                            var q = (ListPreference)args.Preference;
                            q.Summary = q.GetEntry(args.NewValue) ?? "<unknown>";
                        };
                }
                Log--;
            }

        }


        private int ResumeCount;

        protected override void OnResume()
        {
            base.OnResume();
            GameWallpaperService.PreferenceActivityIsActive = true;
            AndroidGameActivity.DoResumed();

            if (ResumeCount++ != 1) return;

            var p = PreferenceManager.GetDefaultSharedPreferences(this);
            if (p.GetBoolean("showRate", true))
                StartActivity(typeof(RateActivity));
        }


        protected override void OnPause()
        {
            GameWallpaperService.PreferenceActivityIsActive = false;
            base.OnPause();
            //Finish();
        }
    }


}
