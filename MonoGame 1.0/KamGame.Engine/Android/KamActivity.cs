using System;
using System.Collections.Generic;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;


namespace KamGame
{

    public class KamActivity : Activity
    {

        protected void SetOnClick(int id, EventHandler action)
        {
            FindViewById<View>(id).Click += action;
        }
        protected void SetOnClick(int id, Action action)
        {
            FindViewById<View>(id).Click += (sender, args) => action();
        }

        public void ViewUri(string uri)
        {
            StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse(uri)));
        }

    }

}
