﻿#region License

/*
MIT License
Copyright © 2006 The Mono.Xna Team

All rights reserved.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

#endregion License

using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework
{
    public class CurveKeyCollection : ICollection<CurveKey>, IEnumerable<CurveKey>, IEnumerable
    {
        #region Private Fields

        private readonly List<CurveKey> innerlist;
        private bool isReadOnly = false;

        #endregion Private Fields

        #region Properties

        public CurveKey this[int index]
        {
            get { return innerlist[index]; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                if (index >= innerlist.Count)
                    throw new IndexOutOfRangeException();

                if (innerlist[index].Position == value.Position)
                    innerlist[index] = value;
                else
                {
                    innerlist.RemoveAt(index);
                    innerlist.Add(value);
                }
            }
        }

        public int Count
        {
            get { return innerlist.Count; }
        }

        public bool IsReadOnly
        {
            get { return isReadOnly; }
        }

        #endregion Properties

        #region Constructors

        public CurveKeyCollection()
        {
            innerlist = new List<CurveKey>();
        }

        #endregion Constructors

        #region Public Methods

        public void Add(CurveKey item)
        {
            if (item == null)
                throw new ArgumentNullException();

            if (innerlist.Count == 0)
            {
                innerlist.Add(item);
                return;
            }

            for (int i = 0; i < innerlist.Count; i++)
            {
                if (item.Position < innerlist[i].Position)
                {
                    innerlist.Insert(i, item);
                    return;
                }
            }

            innerlist.Add(item);
        }

        public void Clear()
        {
            innerlist.Clear();
        }

        public bool Contains(CurveKey item)
        {
            return innerlist.Contains(item);
        }

        public void CopyTo(CurveKey[] array, int arrayIndex)
        {
            innerlist.CopyTo(array, arrayIndex);
        }

        public IEnumerator<CurveKey> GetEnumerator()
        {
            return innerlist.GetEnumerator();
        }

        public bool Remove(CurveKey item)
        {
            return innerlist.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return innerlist.GetEnumerator();
        }

        public CurveKeyCollection Clone()
        {
            var ckc = new CurveKeyCollection();
            foreach (CurveKey key in innerlist)
                ckc.Add(key);
            return ckc;
        }

        public int IndexOf(CurveKey item)
        {
            return innerlist.IndexOf(item);
        }

        public void RemoveAt(int index)
        {
            innerlist.RemoveAt(index);
        }

        #endregion Public Methods
    }
}