using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;
using KamGame.Converts;


namespace KamGame
{

    public static class ConvertHelper
    {
        [DebuggerStepThrough]
        public static T To<T>(this object value)
        {
            var t = typeof(T);
            return t == typeof(object) ? (T)value : (T)value.To(t);
        }
        [DebuggerStepThrough]
        public static object To(this object value, Type type)
        {

            if (value == null) return null;
            if (value.GetType() == type) return value;


            if (type == typeof(string))
                return value.ToString();

            if (type == typeof(int))
                return value.ToInt();
            if (type == typeof(int?))
                return value.ToIntn();

            if (type == typeof(decimal))
                return value.ToDecimal();
            if (type == typeof(decimal?))
                return value.ToDecimaln();

            if (type == typeof(double))
                return value.ToDouble();
            if (type == typeof(double?))
                return value.ToDoublen();

            if (type == typeof(DateTime))
                return value.ToDateTime();
            if (type == typeof(DateTime?))
                return value.ToDateTimen();
            if (type == typeof(bool))
                return value.ToBool();
            if (type == typeof(bool?))
                return value.ToBooln();

            if (type == typeof(float))
                return value.ToFloat();
            if (type == typeof(float?))
                return value.ToFloatn();

            if (type == typeof(long))
                return value.ToInt64();
            if (type == typeof(long?))
                return value.ToInt64n();

            return value;
        }

    }
}

namespace KamGame.Converts
{
    public static class ConvertHelper
    {

        #region ToBool

        [DebuggerStepThrough]
        public static bool ToBool(this object value, bool defValue)
        {
            string s;
            if (value == null || Convert.IsDBNull(value) || ((s = value as string) != null && String.IsNullOrEmpty(s)))
                return defValue;
            return Convert.ToBoolean(value, CultureInfo.InvariantCulture);
        }

        [DebuggerStepThrough]
        public static bool ToBool(this object value)
        {
            string s;
            if (value == null || Convert.IsDBNull(value) || ((s = value as string) != null && String.IsNullOrEmpty(s)))
                return false;
            return Convert.ToBoolean(value, CultureInfo.InvariantCulture);
        }

        [DebuggerStepThrough]
        public static bool? ToBooln(this object value, bool? defValue)
        {
            string s;
            if (value == null || Convert.IsDBNull(value) || ((s = value as string) != null && String.IsNullOrEmpty(s)))
                return defValue;
            return Convert.ToBoolean(value, CultureInfo.InvariantCulture);
        }

        [DebuggerStepThrough]
        public static bool? ToBooln(this object value)
        {
            string s;
            if (value == null || Convert.IsDBNull(value) || ((s = value as string) != null && String.IsNullOrEmpty(s)))
                return null;
            return Convert.ToBoolean(value);
        }


        [DebuggerStepThrough]
        public static bool ToBool(this string value, bool defValue)
        {
            if (String.IsNullOrEmpty(value))
                return defValue;
            bool res;
            return bool.TryParse(value, out res) ? res : defValue;
        }

        [DebuggerStepThrough]
        public static bool ToBool(this string value)
        {
            if (String.IsNullOrEmpty(value))
                return false;
            bool res;
            return bool.TryParse(value, out res) && res;
        }

        [DebuggerStepThrough]
        public static bool? ToBooln(this string value, bool? defValue)
        {
            if (String.IsNullOrEmpty(value))
                return defValue;
            bool res;
            return bool.TryParse(value, out res) ? res : defValue;
        }

        [DebuggerStepThrough]
        public static bool? ToBooln(this string value)
        {
            if (String.IsNullOrEmpty(value))
                return null;
            bool res;
            return bool.TryParse(value, out res) ? (bool?)res : null;
        }

        [DebuggerStepThrough]
        public static string ToString(this bool? value)
        {
            return value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture) : null;
        }


        [DebuggerStepThrough]
        public static bool[] ToBool(this object[] values)
        {
            if (values == null) return null;
            var len = values.Length;
            var res = new bool[len];
            for (var i = 0; i < len; ++i)
                res[i] = values[i].ToBool();
            return res;
        }
        [DebuggerStepThrough]
        public static bool?[] ToBooln(this object[] values)
        {
            if (values == null) return null;
            var len = values.Length;
            var res = new bool?[len];
            for (var i = 0; i < len; ++i)
                res[i] = values[i].ToBooln();
            return res;
        }

        [DebuggerStepThrough]
        public static bool[] ToBool(this string[] values)
        {
            if (values == null) return null;
            var len = values.Length;
            var res = new bool[len];
            for (var i = 0; i < len; ++i)
                res[i] = values[i].ToBool();
            return res;
        }
        [DebuggerStepThrough]
        public static bool?[] ToBooln(this string[] values)
        {
            if (values == null) return null;
            var len = values.Length;
            var res = new bool?[len];
            for (var i = 0; i < len; ++i)
                res[i] = values[i].ToBooln();
            return res;
        }


        #endregion


        #region ToDateTime

        [DebuggerStepThrough]
        public static DateTime ToDateTime(this object value, DateTime defValue)
        {
            string s;
            if (value == null || Convert.IsDBNull(value) || ((s = value as string) != null && String.IsNullOrEmpty(s)))
                return defValue;
            return Convert.ToDateTime(value, CultureInfo.InvariantCulture);
        }

        [DebuggerStepThrough]
        public static DateTime ToDateTime(this object value)
        {
            string s;
            if (value == null || Convert.IsDBNull(value) || ((s = value as string) != null && String.IsNullOrEmpty(s)))
                return DateTime.MinValue;
            return Convert.ToDateTime(value, CultureInfo.InvariantCulture);
        }

        [DebuggerStepThrough]
        public static DateTime? ToDateTimen(this object value, DateTime? defValue)
        {
            string s;
            if (value == null || Convert.IsDBNull(value) || ((s = value as string) != null && String.IsNullOrEmpty(s)))
                return defValue;
            return Convert.ToDateTime(value, CultureInfo.InvariantCulture);
        }

        [DebuggerStepThrough]
        public static DateTime? ToDateTimen(this object value)
        {
            string s;
            if (value == null || Convert.IsDBNull(value) || ((s = value as string) != null && String.IsNullOrEmpty(s)))
                return null;
            return Convert.ToDateTime(value);
        }

        [DebuggerStepThrough]
        public static DateTime ToDateTime(this string value, DateTime defValue)
        {
            if (String.IsNullOrEmpty(value))
                return defValue;
            DateTime res;
            return DateTime.TryParse(value, out res) ? res : defValue;
        }

        [DebuggerStepThrough]
        public static DateTime ToDateTime(this string value)
        {
            if (String.IsNullOrEmpty(value))
                return DateTime.MinValue;
            DateTime res;
            return DateTime.TryParse(value, out res) ? res : DateTime.MinValue;
        }

        [DebuggerStepThrough]
        public static DateTime? ToDateTimen(this string value, DateTime? defValue)
        {
            if (String.IsNullOrEmpty(value))
                return defValue;
            DateTime res;
            return DateTime.TryParse(value, out res) ? res : defValue;
        }

        [DebuggerStepThrough]
        public static DateTime? ToDateTimen(this string value)
        {
            if (String.IsNullOrEmpty(value))
                return null;
            DateTime res;
            return DateTime.TryParse(value, out res) ? (DateTime?)res : null;
        }

        [DebuggerStepThrough]
        public static string ToString(this DateTime? value)
        {
            return value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture) : null;
        }
        [DebuggerStepThrough]
        public static string ToString(this DateTime? value, string format)
        {
            return value.HasValue ? value.Value.ToString(format, CultureInfo.InvariantCulture) : null;
        }
        [DebuggerStepThrough]
        public static string ToString(this DateTime? value, IFormatProvider provider)
        {
            return value.HasValue ? value.Value.ToString(provider) : null;
        }
        [DebuggerStepThrough]
        public static string ToString(this DateTime? value, string format, IFormatProvider provider)
        {
            return value.HasValue ? value.Value.ToString(format, provider) : null;
        }


        [DebuggerStepThrough]
        public static DateTime[] ToDateTime(this object[] values)
        {
            if (values == null) return null;
            var len = values.Length;
            var res = new DateTime[len];
            for (var i = 0; i < len; ++i)
                res[i] = values[i].ToDateTime();
            return res;
        }
        [DebuggerStepThrough]
        public static DateTime?[] ToDateTimen(this object[] values)
        {
            if (values == null) return null;
            var len = values.Length;
            var res = new DateTime?[len];
            for (var i = 0; i < len; ++i)
                res[i] = values[i].ToDateTimen();
            return res;
        }

        [DebuggerStepThrough]
        public static DateTime[] ToDateTime(this string[] values)
        {
            if (values == null) return null;
            var len = values.Length;
            var res = new DateTime[len];
            for (var i = 0; i < len; ++i)
                res[i] = values[i].ToDateTime();
            return res;
        }
        [DebuggerStepThrough]
        public static DateTime?[] ToDateTimen(this string[] values)
        {
            if (values == null) return null;
            var len = values.Length;
            var res = new DateTime?[len];
            for (var i = 0; i < len; ++i)
                res[i] = values[i].ToDateTimen();
            return res;
        }

        #endregion


        #region ToDouble

        [DebuggerStepThrough]
        public static double ToDouble(this object value, double defValue)
        {
            string s;
            if (value == null || Convert.IsDBNull(value) || ((s = value as string) != null && String.IsNullOrEmpty(s)))
                return defValue;
            return Convert.ToDouble(value, CultureInfo.InvariantCulture);
        }

        [DebuggerStepThrough]
        public static double ToDouble(this object value)
        {
            string s;
            if (value == null || Convert.IsDBNull(value) || ((s = value as string) != null && String.IsNullOrEmpty(s)))
                return 0;
            return Convert.ToDouble(value, CultureInfo.InvariantCulture);
        }

        [DebuggerStepThrough]
        public static double? ToDoublen(this object value, double? defValue)
        {
            string s;
            if (value == null || Convert.IsDBNull(value) || ((s = value as string) != null && String.IsNullOrEmpty(s)))
                return defValue;
            return Convert.ToDouble(value, CultureInfo.InvariantCulture);
        }

        [DebuggerStepThrough]
        public static double? ToDoublen(this object value)
        {
            string s;
            if (value == null || Convert.IsDBNull(value) || ((s = value as string) != null && String.IsNullOrEmpty(s)))
                return null;
            return Convert.ToDouble(value);
        }


        [DebuggerStepThrough]
        public static double ToDouble(this string value, double defValue)
        {
            if (String.IsNullOrEmpty(value))
                return defValue;
            double res;
            return double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out res) ? res : defValue;
        }

        [DebuggerStepThrough]
        public static double ToDouble(this string value)
        {
            if (String.IsNullOrEmpty(value))
                return 0;
            double res;
            return double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out res) ? res : 0;
        }

        [DebuggerStepThrough]
        public static double? ToDoublen(this string value, double? defValue)
        {
            if (String.IsNullOrEmpty(value))
                return defValue;
            double res;
            return double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out res) ? res : defValue;
        }

        [DebuggerStepThrough]
        public static double? ToDoublen(this string value)
        {
            if (String.IsNullOrEmpty(value))
                return null;
            double res;
            return double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out res) ? (double?)res : null;
        }

        [DebuggerStepThrough]
        public static string ToString(this double? value)
        {
            return value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture) : null;
        }
        [DebuggerStepThrough]
        public static string ToString(this double? value, string format)
        {
            return value.HasValue ? value.Value.ToString(format, CultureInfo.InvariantCulture) : null;
        }
        [DebuggerStepThrough]
        public static string ToString(this double? value, IFormatProvider provider)
        {
            return value.HasValue ? value.Value.ToString(provider) : null;
        }
        [DebuggerStepThrough]
        public static string ToString(this double? value, string format, IFormatProvider provider)
        {
            return value.HasValue ? value.Value.ToString(format, provider) : null;
        }


        [DebuggerStepThrough]
        public static double[] ToDouble(this object[] values)
        {
            if (values == null) return null;
            var len = values.Length;
            var res = new double[len];
            for (var i = 0; i < len; ++i)
                res[i] = values[i].ToDouble();
            return res;
        }
        [DebuggerStepThrough]
        public static double?[] ToDoublen(this object[] values)
        {
            if (values == null) return null;
            var len = values.Length;
            var res = new double?[len];
            for (var i = 0; i < len; ++i)
                res[i] = values[i].ToDoublen();
            return res;
        }

        [DebuggerStepThrough]
        public static double[] ToDouble(this string[] values)
        {
            if (values == null) return null;
            var len = values.Length;
            var res = new double[len];
            for (var i = 0; i < len; ++i)
                res[i] = values[i].ToDouble();
            return res;
        }
        [DebuggerStepThrough]
        public static double?[] ToDoublen(this string[] values)
        {
            if (values == null) return null;
            var len = values.Length;
            var res = new double?[len];
            for (var i = 0; i < len; ++i)
                res[i] = values[i].ToDoublen();
            return res;
        }


        #endregion


        #region ToDecimal

        [DebuggerStepThrough]
        public static decimal ToDecimal(this object value, decimal defValue)
        {
            string s;
            if (value == null || Convert.IsDBNull(value) || ((s = value as string) != null && String.IsNullOrEmpty(s)))
                return defValue;
            return Convert.ToDecimal(value, CultureInfo.InvariantCulture);
        }

        [DebuggerStepThrough]
        public static decimal ToDecimal(this object value)
        {
            string s;
            if (value == null || Convert.IsDBNull(value) || ((s = value as string) != null && String.IsNullOrEmpty(s)))
                return 0;
            return Convert.ToDecimal(value, CultureInfo.InvariantCulture);
        }

        [DebuggerStepThrough]
        public static decimal? ToDecimaln(this object value, decimal? defValue)
        {
            string s;
            if (value == null || Convert.IsDBNull(value) || ((s = value as string) != null && String.IsNullOrEmpty(s)))
                return defValue;
            return Convert.ToDecimal(value, CultureInfo.InvariantCulture);
        }

        [DebuggerStepThrough]
        public static decimal? ToDecimaln(this object value)
        {
            string s;
            if (value == null || Convert.IsDBNull(value) || ((s = value as string) != null && String.IsNullOrEmpty(s)))
                return null;
            return Convert.ToDecimal(value);
        }


        [DebuggerStepThrough]
        public static decimal ToDecimal(this string value, decimal defValue)
        {
            if (String.IsNullOrEmpty(value))
                return defValue;
            decimal res;
            return decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out res) ? res : defValue;
        }

        [DebuggerStepThrough]
        public static decimal ToDecimal(this string value)
        {
            if (String.IsNullOrEmpty(value))
                return 0;
            decimal res;
            return decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out res) ? res : 0;
        }

        [DebuggerStepThrough]
        public static decimal? ToDecimaln(this string value, decimal? defValue)
        {
            if (String.IsNullOrEmpty(value))
                return defValue;
            decimal res;
            return decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out res) ? res : defValue;
        }

        [DebuggerStepThrough]
        public static decimal? ToDecimaln(this string value)
        {
            if (String.IsNullOrEmpty(value))
                return null;
            decimal res;
            return decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out res) ? (decimal?)res : null;
        }

        [DebuggerStepThrough]
        public static string ToString(this decimal? value)
        {
            return value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture) : null;
        }
        [DebuggerStepThrough]
        public static string ToString(this decimal? value, string format)
        {
            return value.HasValue ? value.Value.ToString(format, CultureInfo.InvariantCulture) : null;
        }
        [DebuggerStepThrough]
        public static string ToString(this decimal? value, IFormatProvider provider)
        {
            return value.HasValue ? value.Value.ToString(provider) : null;
        }
        [DebuggerStepThrough]
        public static string ToString(this decimal? value, string format, IFormatProvider provider)
        {
            return value.HasValue ? value.Value.ToString(format, provider) : null;
        }


        [DebuggerStepThrough]
        public static decimal[] ToDecimal(this object[] values)
        {
            if (values == null) return null;
            var len = values.Length;
            var res = new decimal[len];
            for (var i = 0; i < len; ++i)
                res[i] = values[i].ToDecimal();
            return res;
        }
        [DebuggerStepThrough]
        public static decimal?[] ToDecimaln(this object[] values)
        {
            if (values == null) return null;
            var len = values.Length;
            var res = new decimal?[len];
            for (var i = 0; i < len; ++i)
                res[i] = values[i].ToDecimaln();
            return res;
        }

        [DebuggerStepThrough]
        public static decimal[] ToDecimal(this string[] values)
        {
            if (values == null) return null;
            var len = values.Length;
            var res = new decimal[len];
            for (var i = 0; i < len; ++i)
                res[i] = values[i].ToDecimal();
            return res;
        }
        [DebuggerStepThrough]
        public static decimal?[] ToDecimaln(this string[] values)
        {
            if (values == null) return null;
            var len = values.Length;
            var res = new decimal?[len];
            for (var i = 0; i < len; ++i)
                res[i] = values[i].ToDecimaln();
            return res;
        }

        #endregion


        #region ToFloat

        [DebuggerStepThrough]
        public static float ToFloat(this object value, float defValue)
        {
            string s;
            if (value == null || Convert.IsDBNull(value) || ((s = value as string) != null && String.IsNullOrEmpty(s)))
                return defValue;
            return (float)Convert.ToDouble(value, CultureInfo.InvariantCulture);
        }

        [DebuggerStepThrough]
        public static float ToFloat(this object value)
        {
            string s;
            if (value == null || Convert.IsDBNull(value) || ((s = value as string) != null && String.IsNullOrEmpty(s)))
                return 0;
            return (float)Convert.ToDouble(value, CultureInfo.InvariantCulture);
        }

        [DebuggerStepThrough]
        public static float? ToFloatn(this object value, float? defValue)
        {
            string s;
            if (value == null || Convert.IsDBNull(value) || ((s = value as string) != null && String.IsNullOrEmpty(s)))
                return defValue;
            return (float)Convert.ToDouble(value, CultureInfo.InvariantCulture);
        }

        [DebuggerStepThrough]
        public static float? ToFloatn(this object value)
        {
            string s;
            if (value == null || Convert.IsDBNull(value) || ((s = value as string) != null && String.IsNullOrEmpty(s)))
                return null;
            return (float)Convert.ToDouble(value);
        }


        [DebuggerStepThrough]
        public static float ToFloat(this string value, float defValue)
        {
            if (String.IsNullOrEmpty(value))
                return defValue;
            float res;
            return float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out res) ? res : defValue;
        }

        [DebuggerStepThrough]
        public static float ToFloat(this string value)
        {
            if (String.IsNullOrEmpty(value))
                return 0;
            float res;
            return float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out res) ? res : 0;
        }

        [DebuggerStepThrough]
        public static float? ToFloatn(this string value, float? defValue)
        {
            if (String.IsNullOrEmpty(value))
                return defValue;
            float res;
            return float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out res) ? res : defValue;
        }

        [DebuggerStepThrough]
        public static float? ToFloatn(this string value)
        {
            if (String.IsNullOrEmpty(value))
                return null;
            float res;
            return float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out res) ? (float?)res : null;
        }

        [DebuggerStepThrough]
        public static string ToString(this float? value)
        {
            return value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture) : null;
        }
        [DebuggerStepThrough]
        public static string ToString(this float? value, string format)
        {
            return value.HasValue ? value.Value.ToString(format, CultureInfo.InvariantCulture) : null;
        }
        [DebuggerStepThrough]
        public static string ToString(this float? value, IFormatProvider provider)
        {
            return value.HasValue ? value.Value.ToString(provider) : null;
        }
        [DebuggerStepThrough]
        public static string ToString(this float? value, string format, IFormatProvider provider)
        {
            return value.HasValue ? value.Value.ToString(format, provider) : null;
        }


        [DebuggerStepThrough]
        public static float[] ToFloat(this object[] values)
        {
            if (values == null) return null;
            var len = values.Length;
            var res = new float[len];
            for (var i = 0; i < len; ++i)
                res[i] = values[i].ToFloat();
            return res;
        }
        [DebuggerStepThrough]
        public static float?[] ToFloatn(this object[] values)
        {
            if (values == null) return null;
            var len = values.Length;
            var res = new float?[len];
            for (var i = 0; i < len; ++i)
                res[i] = values[i].ToFloatn();
            return res;
        }

        [DebuggerStepThrough]
        public static float[] ToFloat(this string[] values)
        {
            if (values == null) return null;
            var len = values.Length;
            var res = new float[len];
            for (var i = 0; i < len; ++i)
                res[i] = values[i].ToFloat();
            return res;
        }
        [DebuggerStepThrough]
        public static float?[] ToFloatn(this string[] values)
        {
            if (values == null) return null;
            var len = values.Length;
            var res = new float?[len];
            for (var i = 0; i < len; ++i)
                res[i] = values[i].ToFloatn();
            return res;
        }


        #endregion


        #region ToInt

        [DebuggerStepThrough]
        public static int ToInt(this object value, int defValue)
        {
            string s;
            if (value == null || Convert.IsDBNull(value) || ((s = value as string) != null && String.IsNullOrEmpty(s)))
                return defValue;
            return Convert.ToInt32(value, CultureInfo.InvariantCulture);
        }

        [DebuggerStepThrough]
        public static int ToInt(this object value)
        {
            string s;
            if (value == null || Convert.IsDBNull(value) || ((s = value as string) != null && String.IsNullOrEmpty(s)))
                return 0;
            return Convert.ToInt32(value, CultureInfo.InvariantCulture);
        }

        [DebuggerStepThrough]
        public static int? ToIntn(this object value, int? defValue)
        {
            string s;
            if (value == null || Convert.IsDBNull(value) || ((s = value as string) != null && String.IsNullOrEmpty(s)))
                return defValue;
            return Convert.ToInt32(value, CultureInfo.InvariantCulture);
        }

        [DebuggerStepThrough]
        public static int? ToIntn(this object value)
        {
            string s;
            if (value == null || Convert.IsDBNull(value) || ((s = value as string) != null && String.IsNullOrEmpty(s)))
                return null;
            return Convert.ToInt32(value);
        }


        [DebuggerStepThrough]
        public static int ToInt(this string value, int defValue)
        {
            if (String.IsNullOrEmpty(value))
                return defValue;
            int res;
            return int.TryParse(value, out res) ? res : defValue;
        }

        [DebuggerStepThrough]
        public static int ToInt(this string value)
        {
            if (String.IsNullOrEmpty(value))
                return 0;
            int res;
            return int.TryParse(value, out res) ? res : 0;
        }

        [DebuggerStepThrough]
        public static int? ToIntn(this string value, int? defValue)
        {
            if (String.IsNullOrEmpty(value))
                return defValue;
            int res;
            return int.TryParse(value, out res) ? res : defValue;
        }

        [DebuggerStepThrough]
        public static int? ToIntn(this string value)
        {
            if (String.IsNullOrEmpty(value))
                return null;
            int res;
            return int.TryParse(value, out res) ? (int?)res : null;
        }

        [DebuggerStepThrough]
        public static string ToString(this int? value)
        {
            return value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture) : null;
        }
        [DebuggerStepThrough]
        public static string ToString(this int? value, string format)
        {
            return value.HasValue ? value.Value.ToString(format, CultureInfo.InvariantCulture) : null;
        }
        [DebuggerStepThrough]
        public static string ToString(this int? value, IFormatProvider provider)
        {
            return value.HasValue ? value.Value.ToString(provider) : null;
        }
        [DebuggerStepThrough]
        public static string ToString(this int? value, string format, IFormatProvider provider)
        {
            return value.HasValue ? value.Value.ToString(format, provider) : null;
        }

        [DebuggerStepThrough]
        public static int[] ToInt(this object[] values)
        {
            if (values == null) return null;
            var len = values.Length;
            var res = new int[len];
            for (var i = 0; i < len; ++i)
                res[i] = values[i].ToInt();
            return res;
        }
        [DebuggerStepThrough]
        public static int?[] ToIntn(this object[] values)
        {
            if (values == null) return null;
            var len = values.Length;
            var res = new int?[len];
            for (var i = 0; i < len; ++i)
                res[i] = values[i].ToIntn();
            return res;
        }

        [DebuggerStepThrough]
        public static int[] ToInt(this string[] values)
        {
            if (values == null) return null;
            var len = values.Length;
            var res = new int[len];
            for (var i = 0; i < len; ++i)
                res[i] = values[i].ToInt();
            return res;
        }
        [DebuggerStepThrough]
        public static int?[] ToIntn(this string[] values)
        {
            if (values == null) return null;
            var len = values.Length;
            var res = new int?[len];
            for (var i = 0; i < len; ++i)
                res[i] = values[i].ToIntn();
            return res;
        }


        #endregion


        #region ToInt64

        [DebuggerStepThrough]
        public static Int64 ToInt64(this object value, Int64 defValue)
        {
            string s;
            if (value == null || Convert.IsDBNull(value) || ((s = value as string) != null && String.IsNullOrEmpty(s)))
                return defValue;
            return Convert.ToInt64(value, CultureInfo.InvariantCulture);
        }

        [DebuggerStepThrough]
        public static Int64 ToInt64(this object value)
        {
            string s;
            if (value == null || Convert.IsDBNull(value) || ((s = value as string) != null && String.IsNullOrEmpty(s)))
                return 0;
            return Convert.ToInt64(value, CultureInfo.InvariantCulture);
        }

        [DebuggerStepThrough]
        public static Int64? ToInt64n(this object value, Int64? defValue)
        {
            string s;
            if (value == null || Convert.IsDBNull(value) || ((s = value as string) != null && String.IsNullOrEmpty(s)))
                return defValue;
            return Convert.ToInt64(value, CultureInfo.InvariantCulture);
        }

        [DebuggerStepThrough]
        public static Int64? ToInt64n(this object value)
        {
            string s;
            if (value == null || Convert.IsDBNull(value) || ((s = value as string) != null && String.IsNullOrEmpty(s)))
                return null;
            return Convert.ToInt64(value);
        }


        [DebuggerStepThrough]
        public static Int64 ToInt64(this string value, Int64 defValue)
        {
            if (String.IsNullOrEmpty(value))
                return defValue;
            Int64 res;
            return Int64.TryParse(value, out res) ? res : defValue;
        }

        [DebuggerStepThrough]
        public static Int64 ToInt64(this string value)
        {
            if (String.IsNullOrEmpty(value))
                return 0;
            Int64 res;
            return Int64.TryParse(value, out res) ? res : 0;
        }

        [DebuggerStepThrough]
        public static Int64? ToInt64n(this string value, Int64? defValue)
        {
            if (String.IsNullOrEmpty(value))
                return defValue;
            Int64 res;
            return Int64.TryParse(value, out res) ? res : defValue;
        }

        [DebuggerStepThrough]
        public static Int64? ToInt64n(this string value)
        {
            if (String.IsNullOrEmpty(value))
                return null;
            Int64 res;
            return Int64.TryParse(value, out res) ? (Int64?)res : null;
        }

        [DebuggerStepThrough]
        public static string ToString(this Int64? value)
        {
            return value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture) : null;
        }
        [DebuggerStepThrough]
        public static string ToString(this Int64? value, string format)
        {
            return value.HasValue ? value.Value.ToString(format, CultureInfo.InvariantCulture) : null;
        }
        [DebuggerStepThrough]
        public static string ToString(this Int64? value, IFormatProvider provider)
        {
            return value.HasValue ? value.Value.ToString(provider) : null;
        }
        [DebuggerStepThrough]
        public static string ToString(this Int64? value, string format, IFormatProvider provider)
        {
            return value.HasValue ? value.Value.ToString(format, provider) : null;
        }

        [DebuggerStepThrough]
        public static Int64[] ToInt64(this object[] values)
        {
            if (values == null) return null;
            var len = values.Length;
            var res = new Int64[len];
            for (var i = 0; i < len; ++i)
                res[i] = values[i].ToInt64();
            return res;
        }
        [DebuggerStepThrough]
        public static Int64?[] ToInt64n(this object[] values)
        {
            if (values == null) return null;
            var len = values.Length;
            var res = new Int64?[len];
            for (var i = 0; i < len; ++i)
                res[i] = values[i].ToInt64n();
            return res;
        }

        [DebuggerStepThrough]
        public static Int64[] ToInt64(this string[] values)
        {
            if (values == null) return null;
            var len = values.Length;
            var res = new Int64[len];
            for (var i = 0; i < len; ++i)
                res[i] = values[i].ToInt64();
            return res;
        }
        [DebuggerStepThrough]
        public static Int64?[] ToInt64n(this string[] values)
        {
            if (values == null) return null;
            var len = values.Length;
            var res = new Int64?[len];
            for (var i = 0; i < len; ++i)
                res[i] = values[i].ToInt64n();
            return res;
        }


        #endregion

    }
}