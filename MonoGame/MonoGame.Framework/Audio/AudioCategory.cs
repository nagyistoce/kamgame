using System;
using System.IO;

namespace Microsoft.Xna.Framework.Audio
{
    public struct AudioCategory : IEquatable<AudioCategory>
    {
        private readonly string name;
        private AudioEngine engine;
        internal float fadeIn;
        internal float fadeOut;
        internal CrossfadeType fadeType;
        internal MaxInstanceBehaviour instanceBehaviour;
        internal bool instanceLimit;

        internal bool isBackgroundMusic;
        internal bool isPublic;

        internal int maxInstances;
        internal float volume;

        //insatnce limiting behaviour


        internal AudioCategory(AudioEngine audioengine, string name, BinaryReader reader)
        {
            this.name = name;
            engine = audioengine;

            maxInstances = reader.ReadByte();
            instanceLimit = maxInstances != 0xff;

            fadeIn = (reader.ReadUInt16()/1000f);
            fadeOut = (reader.ReadUInt16()/1000f);

            byte instanceFlags = reader.ReadByte();
            fadeType = (CrossfadeType) (instanceFlags & 0x7);
            instanceBehaviour = (MaxInstanceBehaviour) (instanceFlags >> 3);

            reader.ReadUInt16(); //unkn

            byte vol = reader.ReadByte(); //volume in unknown format
            //lazy 4-param fitting:
            //0xff 6.0
            //0xca 2.0
            //0xbf 1.0
            //0xb4 0.0
            //0x8f -4.0
            //0x5a -12.0
            //0x14 -38.0
            //0x00 -96.0
            double a = -96.0;
            double b = 0.432254984608615;
            double c = 80.1748600297963;
            double d = 67.7385212334047;
            volume = (float) (((a - d)/(1 + (Math.Pow(vol/c, b)))) + d);

            byte visibilityFlags = reader.ReadByte();
            isBackgroundMusic = (visibilityFlags & 0x1) != 0;
            isPublic = (visibilityFlags & 0x2) != 0;
        }

        public string Name
        {
            get { return name; }
        }

        public bool Equals(AudioCategory other)
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Resume()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void SetVolume(float volume)
        {
            throw new NotImplementedException();
        }

        internal enum CrossfadeType
        {
            Linear,
            Logarithmic,
            EqualPower,
        }

        internal enum MaxInstanceBehaviour
        {
            FailToPlay,
            Queue,
            ReplaceOldest,
            ReplaceQuietest,
            ReplaceLowestPriority,
        }
    }
}