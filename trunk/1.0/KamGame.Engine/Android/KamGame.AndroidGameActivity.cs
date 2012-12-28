using System;
using System.Collections.Generic;
using System.Text;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.Hardware;
using Android.Views;


namespace Microsoft.Xna.Framework
{


    partial class AndroidGameActivity
    {

        public static void DoPaused()
        {
            if (Paused != null)
                Paused(null, EventArgs.Empty);
        }

        public static void DoResumed()
        {
            if (Resumed != null)
                Resumed(null, EventArgs.Empty);
        }

    }


    /// <summary>
    /// Заменить тип свойства Activity в классе Game на:
    ///     public static readonly AndroidGameActivityProxy Activity = new AndroidGameActivityProxy();
    /// </summary>
    public class AndroidGameActivityProxy
    {
        public AndroidGameActivity Activity
        {
            get { return _Activity; }
            set
            {
                _Activity = value;
                Window.Activity = value;
            }
        }

        public static implicit operator AndroidGameActivity(AndroidGameActivityProxy a) { return a.Activity; }


        public class WindowProxy
        {
            public AndroidGameActivity Activity { get; set; }
            public void SetFlags(WindowManagerFlags flags, WindowManagerFlags mask)
            {
                //if (Activity != null) Activity.Window.SetFlags(flags, mask);
            }
        }

        public WindowProxy Window = new WindowProxy();
        private AndroidGameActivity _Activity;


        public ScreenOrientation RequestedOrientation
        {
            get
            {
                return Activity != null ? Activity.RequestedOrientation : ScreenOrientation.Landscape;
            }
            set { if ( Activity != null) Activity.RequestedOrientation = value; }
        }

        public AssetManager Assets
        {
            get { return Activity != null ? Activity.Assets : Game.Context.Assets; }
        }

        public void RunOnUiThread(Action action)
        {
            if (Activity != null) Activity.RunOnUiThread(action);
        }

        public void RegisterReceiver(BroadcastReceiver batteryStatusReceiver, IntentFilter intentFilter)
        {
            if (Activity != null) 
                Activity.RegisterReceiver(batteryStatusReceiver, intentFilter);
            else if (Game.Context != null)
                Game.Context.RegisterReceiver(batteryStatusReceiver, intentFilter);
        }

        public object GetSystemService(string name)
        {
            return Activity != null ? Activity.GetSystemService(name) : Game.Context.GetSystemService(name);
        }

        public void Finish()
        {
            if (Activity != null) Activity.Finish();
        }

    }


}
