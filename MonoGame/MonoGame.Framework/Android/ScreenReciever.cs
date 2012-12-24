using Android.App;
using Android.Content;
using Android.Util;
using Microsoft.Xna.Framework.Media;

namespace Microsoft.Xna.Framework
{
    internal class ScreenReceiver : BroadcastReceiver
    {
        public static bool ScreenLocked;

        public override void OnReceive(Context context, Intent intent)
        {
            Log.Info("MonoGame", intent.Action);
            if (intent.Action == Intent.ActionScreenOff)
            {
                ScreenLocked = true;
                MediaPlayer.IsMuted = true;
            }
            else if (intent.Action == Intent.ActionScreenOn)
            {
                // If the user turns the screen on just after it has automatically turned off, 
                // the keyguard will not have had time to activate and the ActionUserPreset intent
                // will not be broadcast. We need to check if the lock is currently active
                // and if not re-enable the game related functions.
                // http://stackoverflow.com/questions/4260794/how-to-tell-if-device-is-sleeping
                var keyguard = (KeyguardManager) context.GetSystemService(Context.KeyguardService);
                if (!keyguard.InKeyguardRestrictedInputMode())
                {
                    ScreenLocked = false;
                    MediaPlayer.IsMuted = false;
                }
            }
            else if (intent.Action == Intent.ActionUserPresent)
            {
                // This intent is broadcast when the user unlocks the phone
                ScreenLocked = false;
                MediaPlayer.IsMuted = false;
            }
        }
    }
}