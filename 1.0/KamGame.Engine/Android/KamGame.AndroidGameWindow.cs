using System;
using System.Collections.Generic;
using System.Text;
using Android.Views;
using OpenTK.Platform.Android;


namespace Microsoft.Xna.Framework
{

    public partial class AndroidGameWindow
    {
        public override ISurfaceHolder Holder { get { return Game.CustomHolder ?? base.Holder; } }
    }

    partial class Game
    {
        // HACK 
        // GraphicsAdapter
        //     public DisplayMode CurrentDisplayMode { get {
        //         var w = _view.Width;
        //         if (w <= 0) w = Game.SurfaceWidth;
        //         var h = _view.Height;
        //         if (h <= 0) h = Game.SurfaceHeight;
        //         return new DisplayMode(w, h, 60, SurfaceFormat.Color);
        //     }
        public static int SurfaceWidth;
        public static int SurfaceHeight;

        public static ISurfaceHolder CustomHolder { get; set; }
    }

}
