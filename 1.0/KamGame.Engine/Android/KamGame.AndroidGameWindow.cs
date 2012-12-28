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
        public static ISurfaceHolder CustomHolder { get; set; }
    }

}
