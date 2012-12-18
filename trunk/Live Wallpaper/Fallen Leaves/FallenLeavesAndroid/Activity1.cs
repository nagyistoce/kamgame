using Android.App;
using Android.Content.PM;
using Android.OS;
using FallenLeaves;
using Microsoft.Xna.Framework;


namespace FallenLeavesAndroid
{

    [Activity(Label = "FallenLeaves"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.SensorLandscape
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)]
    public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Game.Activity = this;
            var g = new Game1();
            SetContentView(g.Window);
            g.Run();
        }
    }

}

