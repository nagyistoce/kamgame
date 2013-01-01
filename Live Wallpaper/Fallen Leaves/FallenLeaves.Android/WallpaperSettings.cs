using Android.App;
using Android.OS;
using Android.Preferences;


namespace FallenLeaves
{

    [Activity(Label = "Fallen Leaves"
        , Icon = "@drawable/icon"
        , Exported = true
        , Permission = "android.permission.BIND_WALLPAPER",
        Theme = "@android:style/Theme.WallpaperSettings")]
        //Theme = "@android:style/Theme.Light.WallpaperSettings")]
    public class WallpaperSettings : PreferenceActivity 
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            AddPreferencesFromResource(Resource.Xml.preferences);
        }
    }
}
