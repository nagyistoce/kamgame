using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using KamGame.Converts;

namespace KamGame
{
    public static class SystemHelper
    {
        public static T With<T>(this T value, Action<T> action)
        {
            action(value);
            return value;
        }

        public static string FullMessage(this Exception ex)
        {
            string s = null;
            while (ex != null)
            {
                s += (!string.IsNullOrEmpty(s) ? "\r\n" : null) + ex.Message;
                ex = ex.InnerException;
            }
            return s;
        }

        [DebuggerStepThrough]
        public static string ToStringInvariant(this float value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }


        #region DateTime

        [DebuggerStepThrough]
        public static DateTime Add(this DateTime self, string time)
        {
            if (string.IsNullOrEmpty(time)) return self;
            var ss = time.Split(':');
            if (ss.Length == 0) return self;
            var ts = new TimeSpan(ss[0].Trim().ToInt(), ss.Length > 1 ? ss[1].Trim().ToInt() : 0, ss.Length > 2 ? ss[2].Trim().ToInt() : 0);
            return self.Add(ts);
        }

        [DebuggerStepThrough]
        public static DateTime? Add(this DateTime? self, string time)
        {
            if (self == null) return null;
            if (string.IsNullOrEmpty(time)) return self;
            var ss = time.Split(':');
            if (ss.Length == 0) return self;
            var ts = new TimeSpan(ss[0].Trim().ToInt(), ss.Length > 1 ? ss[1].Trim().ToInt() : 0, ss.Length > 2 ? ss[2].Trim().ToInt() : 0);
            return self.Value.Add(ts);
        }

        [DebuggerStepThrough]
        public static DateTime SetTime(this DateTime self, string time)
        {
            if (string.IsNullOrEmpty(time)) return self;
            var ss = time.Split(':');
            if (ss.Length == 0) return self;
            var ts = new TimeSpan(ss[0].Trim().ToInt(), ss.Length > 1 ? ss[1].Trim().ToInt() : 0, ss.Length > 2 ? ss[2].Trim().ToInt() : 0);
            return self.Date.Add(ts);
        }

        [DebuggerStepThrough]
        public static DateTime? SetTime(this DateTime? self, string time)
        {
            if (self == null) return null;
            if (string.IsNullOrEmpty(time)) return self;
            var ss = time.Split(':');
            if (ss.Length == 0) return self;
            var ts = new TimeSpan(ss[0].Trim().ToInt(), ss.Length > 1 ? ss[1].Trim().ToInt() : 0, ss.Length > 2 ? ss[2].Trim().ToInt() : 0);
            return self.Value.Date.Add(ts);
        }

        [DebuggerStepThrough]
        public static DateTime? AddDays(this DateTime? self, int value)
        {
            return self.HasValue ? (DateTime?)self.Value.AddDays(value) : null;
        }
        [DebuggerStepThrough]
        public static DateTime? AddHours(this DateTime? self, int value)
        {
            return self.HasValue ? (DateTime?)self.Value.AddHours(value) : null;
        }
        [DebuggerStepThrough]
        public static DateTime? AddMinutes(this DateTime? self, int value)
        {
            return self.HasValue ? (DateTime?)self.Value.AddMinutes(value) : null;
        }
        [DebuggerStepThrough]
        public static DateTime? AddMonths(this DateTime? self, int value)
        {
            return self.HasValue ? (DateTime?)self.Value.AddMonths(value) : null;
        }
        [DebuggerStepThrough]
        public static DateTime? AddSeconds(this DateTime? self, int value)
        {
            return self.HasValue ? (DateTime?)self.Value.AddSeconds(value) : null;
        }
        [DebuggerStepThrough]
        public static DateTime? AddYears(this DateTime? self, int value)
        {
            return self.HasValue ? (DateTime?)self.Value.AddYears(value) : null;
        }

        #endregion


    }

}


