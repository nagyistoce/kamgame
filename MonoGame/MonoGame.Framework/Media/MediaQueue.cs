using System;
using System.Collections.Generic;


namespace Microsoft.Xna.Framework.Media
{
    public sealed class MediaQueue
    {
        private readonly List<Song> songs = new List<Song>();
        private int _activeSongIndex;
        private readonly Random random = new Random();

        public Song ActiveSong
        {
            get
            {
                if (songs.Count == 0)
                    return null;

                return songs[_activeSongIndex];
            }
        }

        public int ActiveSongIndex { get { return _activeSongIndex; } set { _activeSongIndex = value; } }

        internal int Count { get { return songs.Count; } }

        internal IEnumerable<Song> Songs { get { return songs; } }

        internal Song GetNextSong(int direction, bool shuffle)
        {
            if (shuffle)
                _activeSongIndex = random.Next(songs.Count);
            else
                _activeSongIndex = (int)MathHelper.Clamp(_activeSongIndex + direction, 0, songs.Count - 1);

            return songs[_activeSongIndex];
        }

        internal void Clear()
        {
            Song song;
            for (; songs.Count > 0;)
            {
                song = songs[0];
#if !WINRT
                song.Stop();
#endif
                songs.Remove(song);
            }
        }

#if !WINRT
        internal void SetVolume(float volume)
        {
            var count = songs.Count;
            for (var i = 0; i < count; ++i)
                songs[i].Volume = volume;
        }
#endif

        internal void Add(Song song) { songs.Add(song); }

#if !WINRT
        internal void Stop()
        {
            var count = songs.Count;
            for (var i = 0; i < count; ++i)
                songs[i].Stop();
        }
#endif
    }
}