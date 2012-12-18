using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace KamGame
{
    public static class StringHelper
    {



        /// <summary>
        /// Возвращает строку, содержащуюю count дубликатов текущего значения
        /// </summary>
        /// <param name="s"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string Duplicate(this string s, int count)
        {
            if (count <= 0) return null;
            if (count == 1) return s;
            if (count == 2) return s + s;
            if (count == 3) return s + s + s;
            var sb = new StringBuilder(count * s.Length);
            for (var i = 0; i < count; ++i)
                sb.Append(s);
            return sb.ToString();
        }


        /// <summary>
        /// Обрезает по краям строку, преобразует в нижний регистр, пустую строку - в null
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string TrimLower(this string s)
        {
            if (string.IsNullOrEmpty(s)) return null;
            s = s.Trim();
            return string.IsNullOrEmpty(s) ? null : s.ToLowerInvariant();
        }

        /// <summary>
        /// Обрезает по краям строку, преобразует в нижний регистр, пустую строку - в null
        /// </summary>
        /// <param name="s"></param>
        /// <param name="spaces"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string TrimLower(this string s, params char[] spaces)
        {
            if (string.IsNullOrEmpty(s)) return null;
            s = s.Trim(spaces);
            return string.IsNullOrEmpty(s) ? null : s.ToLowerInvariant();
        }


        /// <summary>
        /// Обрезает начало строки, если он совпадает со значением
        /// </summary>
        /// <param name="ignoreCase"></param>
        /// <param name="trimStr"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string TrimStart(this string s, bool ignoreCase, string trimStr)
        {
            if (string.IsNullOrEmpty(s)) return s;

            while (s.StartsWith(trimStr, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
                s = s.Substring(trimStr.Length);

            return s;
        }

        /// <summary>
        /// Обрезает конец строки, если он совпадает со значением
        /// </summary>
        /// <param name="s"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="trimStr"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string TrimEnd(this string s, bool ignoreCase, string trimStr)
        {
            if (string.IsNullOrEmpty(s)) return s;

            while (s.EndsWith(trimStr, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
                s = s.Substring(0, s.Length - trimStr.Length);

            return s;
        }

        /// <summary>
        /// Возвращает true, если строка равна одному из значений;
        /// </summary>
        /// <param name="value"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static bool Equals(this string value, params string[] values)
        {
            return values.Any(value.Equals);
        }


        ///// <summary>
        ///// HtmlEncode и если пуста строка - возвращает &nbsp;
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //[DebuggerStepThrough]
        //public static string ToHtml(this string value)
        //{
        //    return string.IsNullOrEmpty(value) ? "&nbsp;" : HttpUtility.HtmlEncode(value);
        //}
        //public static string ToHtml(this string value, bool mustFull)
        //{
        //    return string.IsNullOrEmpty(value) ? (mustFull ? "&nbsp;" : null) : HttpUtility.HtmlEncode(value);
        //}


        #region IsNull / IsFull

        [DebuggerStepThrough]
        public static bool IsNull(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
        [DebuggerStepThrough]
        public static bool no(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
        [DebuggerStepThrough]
        public static bool IsFull(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }
        [DebuggerStepThrough]
        public static bool yes(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        [DebuggerStepThrough]
        public static string NullIf(this string value, string nullValue)
        {
            return value != nullValue ? value : null;
        }

        [DebuggerStepThrough]
        public static string IfNull(this string value, string defValue)
        {
            return !string.IsNullOrEmpty(value) ? value : defValue;
        }

        /// <summary>
        /// Проверяет, ОТСУТСТВУЮТ ли в строке символы, кроме пробельных
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static bool IsEmptyHtml(this string value)
        {
            if (string.IsNullOrEmpty(value)) return true;
            value = value.Trim();
            return string.IsNullOrEmpty(value) || value.Equals("&nbsp;", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Проверяет, ЕСТЬ ли в строке символы, кроме пробельных
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static bool IsFullHtml(this string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            value = value.Trim();
            return !string.IsNullOrEmpty(value) && !value.Equals("&nbsp;", StringComparison.OrdinalIgnoreCase);
        }

        #endregion


        [DebuggerStepThrough]
        public static string Left(this string value, int length)
        {
            if (string.IsNullOrEmpty(value) || value.Length <= length) return value;
            return value.Substring(0, length);
        }
        [DebuggerStepThrough]
        public static string Right(this string value, int length)
        {
            if (string.IsNullOrEmpty(value) || value.Length <= length) return value;
            return value.Substring(value.Length - length);
        }


        #region StringBuilder

        [DebuggerStepThrough]
        public static void Write(this StringBuilder sb, string str)
        {
            sb.Append(str);
        }

        [DebuggerStepThrough]
        public static void Write(this StringBuilder sb, params string[] strs)
        {
            foreach (var s in strs)
                sb.Append(s);
        }

        [DebuggerStepThrough]
        public static void Write(this StringBuilder sb, params object[] values)
        {
            foreach (var s in values)
                sb.Append(s);
        }

        [DebuggerStepThrough]
        public static void WriteLine(this StringBuilder sb, params string[] strs)
        {
            foreach (var s in strs)
                sb.Append(s);
            sb.AppendLine();
        }

        [DebuggerStepThrough]
        public static void WriteLine(this StringBuilder sb, params object[] values)
        {
            foreach (var s in values)
                sb.Append(s);
            sb.AppendLine();
        }

        [DebuggerStepThrough]
        public static void WriteLines(this StringBuilder sb, params string[] strs)
        {
            foreach (var s in strs)
                sb.AppendLine(s);
        }

        [DebuggerStepThrough]
        public static void WriteLines(this StringBuilder sb, params object[] values)
        {
            foreach (var value in values)
                if (value != null)
                    sb.AppendLine(value.ToString());
        }

        #endregion


        public static string ToString(this NameValueCollection col, string separator)
        {
            var sb = new StringBuilder();
            foreach (var key in col.AllKeys)
            {
                sb.Append(key);
                sb.Append("=");
                sb.Append(col[key]);
                sb.Append(separator);
            }
            return sb.ToString();
        }

    }
}