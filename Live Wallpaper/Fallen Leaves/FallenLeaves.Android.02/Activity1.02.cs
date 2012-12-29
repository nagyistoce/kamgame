using Android.App;
using Android.Content.PM;
using Android.OS;
using Microsoft.Xna.Framework;


namespace FallenLeaves
{

    [Activity(Label = "FL.02"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.SensorLandscape
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)]
    public class Activity1 : Activity { }
    //public class Activity1 : AndroidGameActivity
    //{
    //    protected override void OnCreate(Bundle bundle)
    //    {
    //        base.OnCreate(bundle);
    //        Game.Activity.Activity = this;
    //        var g = new FallenLeavesGame();
    //        SetContentView(g.Window);
    //        g.Run();
    //    }
    //}

}

