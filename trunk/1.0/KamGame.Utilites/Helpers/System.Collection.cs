using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace KamGame
{
    public static class CollectionHelper
    {

        #region Array

        [DebuggerStepThrough]
        public static bool yes<T>(this T[] array)
        {
            return array != null && array.Length > 0;
        }

        [DebuggerStepThrough]
        public static bool no<T>(this T[] array)
        {
            return array == null || array.Length == 0;
        }

        [DebuggerStepThrough]
        public static bool yes<T>(this List<T> array)
        {
            return array != null && array.Count > 0;
        }

        [DebuggerStepThrough]
        public static bool no<T>(this List<T> array)
        {
            return array == null || array.Count == 0;
        }

        #endregion

        #region Collection

        public static void AddRange<T>(this Collection<T> me, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                me.Add(item);
            }
        }

        [DebuggerStepThrough]
        public static void RemoveRange<TItem>(this Collection<TItem> me, IEnumerable<TItem> items) where TItem : class
        {
            if (me == null || items == null) return;

            foreach (var item in items)
            {
                me.Remove(item);
            }
        }



        #endregion

        
        #region Dictionary

        /// <summary>
        /// Возвращает Dictionary со списком свойств объекта заданного типа.
        /// Предварительно преобразует с помощью convert
        /// </summary>
        [DebuggerStepThrough]
        public static TValue Try<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key)
        {
            TValue value;
            return dic != null && dic.TryGetValue(key, out value) ? value : default(TValue);
        }

        [DebuggerStepThrough]
        public static TValue Try<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key)
        {
            TValue value;
            return dic != null && dic.TryGetValue(key, out value) ? value : default(TValue);
        }

        [DebuggerStepThrough]
        public static TResult Try<TKey, TValue, TResult>(this Dictionary<TKey, TValue> dic, TKey key)
            where TValue : class
        {
            TValue value;
            return dic.TryGetValue(key, out value) && value is TResult ? (TResult)(object)value : default(TResult);
        }

        [DebuggerStepThrough]
        public static TResult Try<TKey, TValue, TResult>(this IDictionary<TKey, TValue> dic, TKey key)
            where TValue : class
        {
            TValue value;
            return dic.TryGetValue(key, out value) && value is TResult ? (TResult)(object)value : default(TResult);
        }


        [DebuggerStepThrough]
        public static Dictionary<TKey, TValue> AddRange<TKey, TValue>(this Dictionary<TKey, TValue> me, IDictionary<TKey, TValue> src, bool replace)
        {
            if (src == null) return me;
            foreach (var p in src)
            {
                if (!me.ContainsKey(p.Key))
                    me.Add(p.Key, p.Value);
                else if (replace)
                    me[p.Key] = p.Value;
            }

            return me;
        }

        [DebuggerStepThrough]
        public static IDictionary<TKey, TValue> AddRange<TKey, TValue>(this IDictionary<TKey, TValue> me, IDictionary<TKey, TValue> src, bool replace)
        {
            if (src == null) return me;
            foreach (var p in src)
            {
                if (!me.ContainsKey(p.Key))
                    me.Add(p.Key, p.Value);
                else if (replace)
                    me[p.Key] = p.Value;
            }

            return me;
        }

        #endregion


        #region List

        [DebuggerStepThrough]
        public static TItem Find<TItem>(this List<TItem> me, Func<TItem, bool> match)
        {
            if (me == null || match == null) return default(TItem);
            foreach (var item in me)
                if (match(item)) return item;
            return default(TItem);
        }

        [DebuggerStepThrough]
        public static int IndexOf<TItem>(this List<TItem> list, Func<TItem, bool> match)
        {
            if (list == null || match == null) return -1;
            var len = list.Count;
            for (int i = 0; i < len; ++i)
                if (match(list[i])) return i;
            return -1;
        }

        [DebuggerStepThrough]
        public static TItem Register<TItem>(this List<TItem> me, TItem item)
        {
            if (me == null) return default(TItem);
            if (!me.Contains(item))
                me.Add(item);
            return item;
        }
        [DebuggerStepThrough]
        public static void Register<TItem>(this List<TItem> me, IEnumerable<TItem> items)
        {
            if (me == null) return;
            foreach (var item in items)
            {
                if (!me.Contains(item)) me.Add(item);
            }
        }

        /// <summary>
        /// добавляет уникальный элемент в список
        /// </summary>
        /// <param name="item"></param>
        /// <param name="match">поиск существующего элемента</param>
        /// <param name="me"></param>
        /// <returns>возвращает новый или уже существующий элемент</returns>
        [DebuggerStepThrough]
        public static TItem Register<TItem>(this List<TItem> me, TItem item, Func<TItem, bool> match) where TItem : class
        {
            if (me == null) return default(TItem);
            var itm = me.Find(match);
            if (itm != null) return itm;
            me.Add(item);
            return item;
        }

        [DebuggerStepThrough]
        public static void AddRange<TItem>(this IList<TItem> me, IEnumerable<TItem> items) where TItem : class
        {
            if (me == null || items == null) return;

            foreach (var item in items)
            {
                me.Add(item);
            }
        }


        [DebuggerStepThrough]
        public static void RemoveRange<TItem>(this IList<TItem> me, IEnumerable<TItem> items) where TItem : class
        {
            if (me == null || items == null) return;

            foreach (var item in items)
            {
                me.Remove(item);
            }
        }

        [DebuggerStepThrough]
        public static void RemoveRange<TItem>(this List<TItem> me, IEnumerable<TItem> items) where TItem : class
        {
            if (me == null || items == null) return;

            me.RemoveAll(items.Contains);
        }


        #endregion

    }
}