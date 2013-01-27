using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using KamGame;


namespace FallenLeaves
{
    [Activity(Label = "Rate Falling Leaves", Theme = "@android:style/Theme.Dialog")]
    public class RateActivity : KamActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.RateLayout);

            SetOnClick(Resource.Id.btnRate, () =>
            {
                ViewUri(
#if FREE_VERSION
                    "https://play.google.com/store/apps/details?id=com.divarc.fallenleaves.free"
#else
                    "https://play.google.com/store/apps/details?id=com.divarc.fallenleaves"
#endif
                );
                Finish();
            });

            SetOnClick(Resource.Id.btnRemind, Finish);
            SetOnClick(Resource.Id.btnDontShow, ()=>
            {
                var e = PreferenceManager.GetDefaultSharedPreferences(this).Edit();
                e.PutBoolean("showRate", false);
                e.Commit();
                Finish(); 
            });
        }





    }
}