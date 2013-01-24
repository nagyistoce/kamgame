using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Lang;
using KamGame;
using Microsoft.Xna.Framework;


namespace FallenLeaves
{

    [Activity(
#if FREE_VERSION
        Label = "Fallen Leaves Free"
#else
        Label = "Fallen Leaves"
#endif
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@android:style/Theme.Holo.Dialog"
        , AlwaysRetainTaskState = true
        , LaunchMode = LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.SensorLandscape
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)]
    public class MainActivity : KamActivity
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.MainLayout);
            SetOnClick(Resource.Id.btnSetWallpaper, btnSetWallpaper_Click);

#if FREE_VERSION
            SetOnClick(Resource.Id.btnMarket, () => ViewUri("https://play.google.com/store/apps/details?id=com.divarc.fallenleaves.free"));
            SetOnClick(Resource.Id.btnGetFullVervion, () => ViewUri("https://play.google.com/store/apps/details?id=com.divarc.fallenleaves"));
#else
            SetOnClick(Resource.Id.btnMarket, () => ViewUri("https://play.google.com/store/apps/details?id=com.divarc.fallenleaves"));
#endif
        }


        public void btnSetWallpaper_Click()
        {
            var i = new Intent();

            if ((int)Build.VERSION.SdkInt > 15)
            {
                i.SetAction("android.service.wallpaper.CHANGE_LIVE_WALLPAPER");
                var cls = Class.FromType(typeof(FallenLeavesWallpaperService));

#if FREE_VERSION
                const string pname = "com.divarc.fallenleaves.free";
#else
                const string pname = "com.divarc.fallenleaves";
#endif

                i.PutExtra(
                    "android.service.wallpaper.extra.LIVE_WALLPAPER_COMPONENT",
                    new ComponentName(pname, cls.CanonicalName)
                );
            }
            else
            {
                i.SetAction(WallpaperManager.ActionLiveWallpaperChooser);
            }

            StartActivityForResult(i, 0);
            Finish();
        }

    }



    //public class MainActivity : AndroidGameActivity
    //{
    //    protected override void OnCreate(Bundle bundle)
    //    {
    //        base.OnCreate(bundle);
    //        Game.Activity.Activity = this;
    //        var g = new FallenLeavesGame { UseMouse = false, UseAccelerometer = true };
    //        SetContentView(g.Window);
    //        g.Run();
    //    }
    //}

}

