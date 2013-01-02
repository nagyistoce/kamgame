using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Android.App;
using Android.Database;



namespace System.Linq
{

    public static class CursorLinqHelper
    {


        #region Cursor

        [DebuggerStepThrough]
        public static IEnumerable<ICursor> AsEnumerable(this ICursor cursor)
        {
            if (cursor == null)
                throw new ArgumentNullException("cursor");
            return new CursorEnumerable(cursor);
        }


        [DebuggerStepThrough]
        public static IEnumerable<TResult> Select<TResult>(this ICursor cursor, Func<ICursor, TResult> selector)
        {
            return cursor.AsEnumerable().Select(selector);
        }

        [DebuggerStepThrough]
        public static IEnumerable<TResult> Select<TResult>(this ICursor cursor, string[] columns, Func<ICursor, int[], TResult> selector)
        {
            if (columns == null)
                throw new ArgumentNullException("columns");

            var columnIndexes = new int[columns.Length];
            for (int i = 0, len = columns.Length; i < len; ++i)
            {
                columnIndexes[i] = cursor.GetColumnIndex(columns[i]);
            }

            return cursor.AsEnumerable().Select(a => selector(a, columnIndexes));
        }

        #endregion



        #region Select from Activity

        [DebuggerStepThrough]
        public static IEnumerable<TResult> Select<TResult>(
            this Activity activity, Android.Net.Uri uri, string[] projection,
            string selection, string[] selectionArgs, string sortOrder,
            Func<ICursor, int[], TResult> selector)
        {
            if (activity == null)
                throw new ArgumentNullException("activity");

            return activity
                .ManagedQuery(uri, projection, selection, selectionArgs, sortOrder)
                .Select(projection, selector);
        }

        [DebuggerStepThrough]
        public static IEnumerable<TResult> Select<TResult>(
            this Activity activity, Android.Net.Uri uri, string[] projection,
            string selection, string[] selectionArgs, 
            Func<ICursor, int[], TResult> selector)
        {
            return activity.Select(uri, projection, selection, selectionArgs, null, selector);
        }

        [DebuggerStepThrough]
        public static IEnumerable<TResult> Select<TResult>(
            this Activity activity, Android.Net.Uri uri, string[] projection,
            string selection, 
            Func<ICursor, int[], TResult> selector)
        {
            return activity.Select(uri, projection, selection, null, null, selector);
        }

        [DebuggerStepThrough]
        public static IEnumerable<TResult> Select<TResult>(
            this Activity activity, Android.Net.Uri uri, string[] projection,
            Func<ICursor, int[], TResult> selector)
        {
            return activity.Select(uri, projection, null, null, null, selector);
        }


        [DebuggerStepThrough]
        public static IEnumerable<TResult> Select<TResult>(
            this Activity activity,Android.Net.Uri uri, string[] projection, 
            string selection, string[] selectionArgs, string sortOrder,
            Func<ICursor, TResult> selector)
        {
            if (activity == null)
                throw new ArgumentNullException("activity");

            return activity
                .ManagedQuery(uri, projection, selection, selectionArgs, sortOrder)
                .Select(selector);
        }

        [DebuggerStepThrough]
        public static IEnumerable<TResult> Select<TResult>(
            this Activity activity, Android.Net.Uri uri, string[] projection, 
            string selection, string[] selectionArgs,
            Func<ICursor, TResult> selector)
        {
            return activity.Select(uri, projection, selection, selectionArgs, null, selector);

        }

        [DebuggerStepThrough]
        public static IEnumerable<TResult> Select<TResult>(
            this Activity activity, Android.Net.Uri uri, string[] projection, 
            string selection,
            Func<ICursor, TResult> selector)
        {
            return activity.Select(uri, projection, selection, null, null, selector);

        }

        [DebuggerStepThrough]
        public static IEnumerable<TResult> Select<TResult>(
            this Activity activity, Android.Net.Uri uri, string[] projection,
            Func<ICursor, TResult> selector)
        {
            return activity.Select(uri, projection, null, null, null, selector);

        }

        #endregion


    }


    public class CursorEnumerable : IEnumerable<ICursor>
    {
        readonly ICursor Cursor;

        public CursorEnumerable(ICursor cursor)
        {
            Cursor = cursor;
        }

        public IEnumerator<ICursor> GetEnumerator()
        {
            return new CursorEnumerator(Cursor);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }


    public class CursorEnumerator : IEnumerator<ICursor>
    {
        readonly ICursor Cursor;

        public CursorEnumerator(ICursor cursor)
        {
            Cursor = cursor;
        }

        public void Dispose() { }

        public bool MoveNext()
        {
            return Cursor.IsBeforeFirst ? Cursor.MoveToFirst() : Cursor.MoveToNext();
        }

        public void Reset()
        {
            Cursor.MoveToFirst();
        }

        public ICursor Current { get { return Cursor; } }
        object IEnumerator.Current { get { return Cursor; } }

    }


}
