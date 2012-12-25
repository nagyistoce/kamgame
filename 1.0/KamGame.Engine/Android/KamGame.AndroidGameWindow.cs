using System;
using System.Collections.Generic;
using System.Text;
using Android.Views;
using OpenTK.Platform.Android;


namespace Microsoft.Xna.Framework
{


    public partial class AndroidGameWindow
    {
        public static ISurfaceHolder CustomHolder { get; set; }
        public override ISurfaceHolder Holder { get { return CustomHolder ?? base.Holder; } }
    }


}
