using Android.App;
using Android.Content.PM;
using Android.OS;
using Microsoft.Xna.Framework;


namespace FallenLeaves
{

#if DEBUG
    [Activity(Label = "Fallen Leaves"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , AlwaysRetainTaskState = true
        , LaunchMode = LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.SensorLandscape
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)]
    public class Activity1 : Activity { }
#endif
    //public class Activity1 : AndroidGameActivity
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

