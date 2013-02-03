using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Provider;
using Android.Widget;
using KamGame;
using LicenseVerificationLibrary;
using LicenseVerificationLibrary.Obfuscator;
using LicenseVerificationLibrary.Policy;
using Microsoft.Xna.Framework;

using Uri = Android.Net.Uri;



namespace FallenLeaves
{

    [Activity(
#if FREE_VERSION
        Label = "Falling Leaves Free Settings"
#else
        Label = "Falling Leaves Settings"
#endif
        , Icon = "@drawable/icon"
        , Exported = true
        , Permission = "android.permission.BIND_WALLPAPER",
        Theme = "@android:style/Theme.WallpaperSettings")]
    public class WallpaperSettings : PreferenceActivity, ILicenseCheckerCallback
    {
        private const string Base64PublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAmKR5M7FnWcekAgMIlW66i2s8Q6TO8H9xS+08LDt6C6YGeT8okuw4vrAPw1CpayUUU/RCc3f4Tx74xPfX866GdKd+VU0q0att93Q37eoDpOFohlEXSYBp/Q/9H4QU4vuYrHrjduCBq3L87SC7XpCuVnEk75QemKqYMzPTgaK6efP24MfL6+bXtCQ11yPWDNTqYQpGdtXdTBRryT9QsSO4Bp18VBJ5HxXp6OLg3ONmvHkej7Z60mZXZGd9ihltq22x9rMAOU/eKve4yamDluGi6MXdgfcmbM1MNPJvZKuXkDTo6SuUWw65YFc07G6y3kK4dZlbsAX5wi9midMHXxTBmwIDAQAB";


        //static LogWriter Log = new LogWriter("KamGame");

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

#if FREE_VERSION
            AddPreferencesFromResource(Resource.Xml.preferences_free);

            foreach (var p in new[] { "wind", "wind_dir", "wind_show", "layout", "fallen_leafs_scale" }.Select(FindPreference))
            {
                p.Enabled = false;
                p.Summary = "(It's available only in the full version!)";
            }

#else
            AddPreferencesFromResource(Resource.Xml.preferences);
#endif

            foreach (var key in new[] { "textureQuality", "sky", "clouds_count", "wind", "wind_dir", "wind_show", "grass_count", "layout", "fallen_leafs_count", "fallen_leafs_scale" })
            {
                //Log += key;
                var p = FindPreference(key);
                var lp = p as ListPreference;
                if (lp == null) continue;
                lp.Summary = (lp.GetEntry(lp.SharedPreferences.GetString(key, "")) ?? "<unknown>") + " " + lp.Summary;
                //Log &= lp.Summary;

                if (lp.Enabled)
                    lp.PreferenceChange += (sender, args) =>
                    {
                        var q = (ListPreference)args.Preference;
                        q.Summary = q.GetEntry(args.NewValue) ?? "<unknown>";
                    };
                //Log--;
            }

#if !FREE_VERSION
            // Try to use more data here. ANDROID_ID is a single point of attack.
            var deviceId = Settings.Secure.GetString(ContentResolver, Settings.Secure.AndroidId);

            // Construct the LicenseChecker with a policy.
            var obfuscator = new AesObfuscator(Salt, PackageName, deviceId);
            var policy = new ServerManagedPolicy(this, obfuscator);
            checker = new LicenseChecker(this, policy, Base64PublicKey);

            DoCheck();
#endif
        }

        protected override void OnDestroy()
        {
            checker.OnDestroy();
            base.OnDestroy();
        }

        private static int ResumeCount;
        private LicenseChecker checker;
        private readonly byte[] Salt = new byte[] { 46, 65, 88, 95, 128, 103, 30, 64, 58, 117, 36, 113, 57, 71, 45, 77, 11, 32, 64, 89 };


        protected override void OnResume()
        {
            base.OnResume();
            GameWallpaperService.PreferenceActivityIsActive = true;
            AndroidGameActivity.DoResumed();

            if ((ResumeCount++ % 8) != 1) return;

            if (FallenLeavesGame.NotLicensed) return;
            var p = PreferenceManager.GetDefaultSharedPreferences(this);
            if (p.GetBoolean("showRate", true))
                StartActivity(typeof(RateActivity));
        }


        protected override void OnPause()
        {
            GameWallpaperService.PreferenceActivityIsActive = false;
            base.OnPause();
            //Finish();
        }




        protected override Dialog OnCreateDialog(int id)
        {
            var retry = id == 1;

            EventHandler<DialogClickEventArgs> eventHandler = (sender, args) =>
            {
                if (retry)
                {
                    DoCheck();
                }
                else
                {
                    var uri = Uri.Parse("http://market.android.com/details?id=" + PackageName);
                    var marketIntent = new Intent(Intent.ActionView, uri);
                    StartActivity(marketIntent);
                    Finish();
                }
            };

            var message = retry ? Resource.String.unlicensed_dialog_retry_body : Resource.String.unlicensed_dialog_body;
            var ok = retry ? Resource.String.retry_button : Resource.String.buy_button;

            return new AlertDialog.Builder(this) // builder
                .SetTitle(Resource.String.unlicensed_dialog_title) // title
                .SetMessage(message) // message
                .SetPositiveButton(ok, eventHandler) // ok
                .SetNegativeButton(Resource.String.quit_button, delegate { Finish(); }) // cancel
                .Create(); // create dialog now
        }

        private void DoCheck()
        {
            SetProgressBarIndeterminateVisibility(true);
            checker.CheckAccess(this);
        }

        private void DisplayResult(string result)
        {
            RunOnUiThread(() =>
            {
                Toast.MakeText(this, result, ToastLength.Long).Show();
                SetProgressBarIndeterminateVisibility(false);
            });
        }

        private void DisplayDialog(bool showRetry)
        {
            RunOnUiThread(() =>
            {
                SetProgressBarIndeterminateVisibility(false);
                ShowDialog(showRetry ? 1 : 0);
            });
        }


        public void Allow(PolicyServerResponse response)
        {
            if (IsFinishing) return;
            DisplayResult(GetString(Resource.String.allow));
        }

        public void DontAllow(PolicyServerResponse response)
        {
            if (IsFinishing) return;
            FallenLeavesGame.NotLicensed |= response == PolicyServerResponse.NotLicensed;
            DisplayResult(GetString(Resource.String.dont_allow));
            DisplayDialog(response == PolicyServerResponse.Retry);
        }

        public void ApplicationError(CallbackErrorCode errorCode)
        {
            if (IsFinishing) return;
            var errorString = GetString(Resource.String.application_error);
            DisplayResult(string.Format(errorString, errorCode));
        }

    }


}
