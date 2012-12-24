using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework
{
    /// <summary>
    ///     Interface used to add an object to be loaded on the primary thread
    /// </summary>
    internal interface IPrimaryThreadLoaded
    {
        bool Load();
    }

    /// <summary>
    ///     Static class that is called before every draw to load resources that need to finish loading on the primary thread
    /// </summary>
    internal static class PrimaryThreadLoader
    {
        private static readonly object ListLockObject = new object();
        private static readonly List<IPrimaryThreadLoaded> NeedToLoad = new List<IPrimaryThreadLoaded>();
        private static readonly List<IPrimaryThreadLoaded> RemoveList = new List<IPrimaryThreadLoaded>();
        private static DateTime _lastUpdate = DateTime.Now;

        public static void AddToList(IPrimaryThreadLoaded primaryThreadLoaded)
        {
            lock (ListLockObject)
            {
                NeedToLoad.Add(primaryThreadLoaded);
            }
        }

        public static void RemoveFromList(IPrimaryThreadLoaded primaryThreadLoaded)
        {
            lock (ListLockObject)
            {
                NeedToLoad.Remove(primaryThreadLoaded);
            }
        }

        public static void RemoveFromList(List<IPrimaryThreadLoaded> primaryThreadLoadeds)
        {
            lock (ListLockObject)
            {
                foreach (IPrimaryThreadLoaded primaryThreadLoaded in primaryThreadLoadeds)
                {
                    NeedToLoad.Remove(primaryThreadLoaded);
                }
            }
        }

        public static void Clear()
        {
            lock (ListLockObject)
            {
                NeedToLoad.Clear();
            }
        }

        /// <summary>
        ///     Loops through list and loads the item.  If successful, it is removed from the list.
        /// </summary>
        public static void DoLoads()
        {
            if ((DateTime.Now - _lastUpdate).Milliseconds < 250) return;

            _lastUpdate = DateTime.Now;
            lock (ListLockObject)
            {
                for (int i = 0; i < NeedToLoad.Count; i++)
                {
                    IPrimaryThreadLoaded primaryThreadLoaded = NeedToLoad[i];
                    if (primaryThreadLoaded.Load())
                    {
                        RemoveList.Add(primaryThreadLoaded);
                    }
                }

                for (int i = 0; i < RemoveList.Count; i++)
                {
                    IPrimaryThreadLoaded primaryThreadLoaded = RemoveList[i];
                    NeedToLoad.Remove(primaryThreadLoaded);
                }

                RemoveList.Clear();
            }
        }
    }
}