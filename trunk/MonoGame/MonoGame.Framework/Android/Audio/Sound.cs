using System;
using Android.Content;
using Android.Content.Res;
using Android.Media;

namespace Microsoft.Xna.Framework.Audio
{
    internal class Sound : IDisposable
    {
        private const int MAX_SIMULTANEOUS_SOUNDS = 10;
        private static readonly SoundPool s_soundPool = new SoundPool(MAX_SIMULTANEOUS_SOUNDS, Stream.Music, 0);
        private int _soundId;
        private bool disposed;

        public Sound(string filename, float volume, bool looping)
        {
            using (AssetFileDescriptor fd = Game.Activity.Assets.OpenFd(filename))
                _soundId = s_soundPool.Load(fd.FileDescriptor, fd.StartOffset, fd.Length, 1);

            Looping = looping;
            Volume = volume;
        }

        public Sound(byte[] audiodata, float volume, bool looping)
        {
            _soundId = 0;
            //throw new NotImplementedException();
        }

        internal static SoundPool SoundPool
        {
            get { return s_soundPool; }
        }

        public float Volume { get; set; }
        public bool Looping { get; set; }
        public float Rate { get; set; }
        public float Pan { get; set; }

        public double Duration
        {
            get { return 0; } // cant get this from soundpool.
        }

        public double CurrentPosition
        {
            get { return 0; } // cant get this from soundpool.
        }

        public bool Playing
        {
            get { return false; // cant get this from soundpool.
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal static void PauseAll()
        {
            s_soundPool.AutoPause();
        }

        internal static void ResumeAll()
        {
            s_soundPool.AutoResume();
        }

        ~Sound()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (_soundId != 0)
                    s_soundPool.Unload(_soundId);
                _soundId = 0;

                disposed = true;
            }
        }

        public void Resume(int streamId)
        {
            s_soundPool.Resume(streamId);
        }

        public int Play()
        {
            if (_soundId == 0)
                return -1;

            float panRatio = (Pan + 1.0f)/2.0f;
            float volumeTotal = SoundEffect.MasterVolume*Volume;
            float volumeLeft = volumeTotal*(1.0f - panRatio);
            float volumeRight = volumeTotal*panRatio;

            var rate = (float) Math.Pow(2, Rate);
            rate = Math.Max(Math.Min(rate, 2.0f), 0.5f);

            return s_soundPool.Play(_soundId, volumeLeft, volumeRight, 1, Looping ? -1 : 0, rate);
        }

        public void Pause(int streamId)
        {
            s_soundPool.Pause(streamId);
        }

        public void Stop(int streamId)
        {
            s_soundPool.Stop(streamId);
        }

        internal static void IncreaseMediaVolume()
        {
            var audioManager = (AudioManager) Game.Activity.GetSystemService(Context.AudioService);

            audioManager.AdjustStreamVolume(Stream.Music, Adjust.Raise, VolumeNotificationFlags.ShowUi);
        }

        internal static void DecreaseMediaVolume()
        {
            var audioManager = (AudioManager) Game.Activity.GetSystemService(Context.AudioService);

            audioManager.AdjustStreamVolume(Stream.Music, Adjust.Lower, VolumeNotificationFlags.ShowUi);
        }
    }
}