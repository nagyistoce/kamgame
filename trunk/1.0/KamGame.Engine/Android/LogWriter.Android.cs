using System;
using System.Collections;
using System.Diagnostics;
using System.Text;
using Android.Util;


namespace KamGame
{
    /// <summary>
    /// Запись лога
    /// Отступ изменяется символами "&gt;&gt;" и "&lt;&lt;" в начале или конце добавляемой строки.
    /// </summary>
    public class LogWriter
    {
        public LogWriter(string tag, string prefix = null)
        {
            Tag = tag;
            Prefix = prefix;
        }

        public LogWriter(string tag, Func<string> getPrefix)
        {
            Tag = tag;
            GetPrefix = getPrefix;
        }

        public string Tag, Prefix;
        public Func<string> GetPrefix;

        private int _Level;
        public int Level { get { return _Level; } }
        [Conditional("DEBUG")]
        public void SetLevel(int value)
        {
            Indent = "    ".Duplicate(_Level = value);
        }
        [Conditional("DEBUG")]
        public void IncLevel()
        {
            SetLevel(Level + 1);
        }
        [Conditional("DEBUG")]
        public void DecLevel()
        {
            if (_Level > 0)
                SetLevel(Level - 1);
        }

        public bool UseBraces = true;

        //private string NowFormat = "yyyy-MM-dd HH:mm:ss ";
        //private bool _ShowMilliseconds;
        //public bool ShowMilliseconds
        //{
        //    get { return _ShowMilliseconds; }
        //    set
        //    {
        //        _ShowMilliseconds = value;
        //        NowFormat = value ? "yyyy-MM-dd HH:mm:ss:fff " : "yyyy-MM-dd HH:mm:ss ";
        //    }
        //}

        [Conditional("DEBUG")]
        public void WriteLine(string text = null)
        {
            if (GetPrefix != null)
                Log.Debug(Tag, GetPrefix() + text);
            else
                Log.Debug(Tag, Prefix + text);
        }

        [Conditional("DEBUG")]
        public void Write(string text)
        {
            WriteLine(text);
        }

        public string Now()
        {
            return null;
            //return DateTime.Now.ToString(NowFormat);
        }


        /// <summary>
        /// Увеличивает/Уменьшает Level на 1 в зависимости от value - "&gt;&gt;"/"&lt;&lt;"
        /// </summary>
        /// <param name="value"></param>
        protected int OffsetLevel(string value)
        {
            switch (value)
            {
                case ">>": return Level + 1;
                case "<<": return Level > 0 ? Level - 1 : Level;
                default: return Level;
            }
        }

        public string Indent { get; private set; }

        [Conditional("DEBUG")]
        public void Add(string text, object arg0)
        {
            Add(String.Format(text, arg0));
        }
        [Conditional("DEBUG")]
        public void Add(string text, object arg0, object arg1)
        {
            Add(String.Format(text, arg0, arg1));
        }
        [Conditional("DEBUG")]
        public void Add(string text, object arg0, object arg1, object arg2)
        {
            Add(String.Format(text, arg0, arg1, arg2));
        }
        [Conditional("DEBUG")]
        public void Add(string text, params object[] args)
        {
            Add(String.Format(text, args));
        }

        [Conditional("DEBUG")]
        public void Add(string text)
        {
            if (text.no())
                WriteLine();
            else if (text.Length < 2)
                WriteLine(Now() + Indent + text);
            else
            {
                // обработать символы смещения ">>" или "<<" в НАЧАЛЕ строки
                var newLevel = OffsetLevel(text.Left(2));
                if (Level != newLevel)
                {
                    text = text.Right(text.Length - 2);
                    SetLevel(newLevel);
                }
                // обработать символы смещения ">>" или "<<" в КОНЦЕ строки
                newLevel = OffsetLevel(text.Right(2));
                if (Level != newLevel)
                {
                    text = text.Left(text.Length - 2);
                }

                WriteLine(Now() + Indent + text);

                SetLevel(newLevel);
            }
        }

        [Conditional("DEBUG")]
        public void AddValue(string name, object value)
        {
            Add(name + (value != null ? " = " + value : " = null"));
        }

        [Conditional("DEBUG")]
        public void AddObject(string name, object value, int maxDeep = 1)
        {
            var sb = new StringBuilder();
            AddObject(sb, Indent, name, value, maxDeep);
            Write(sb.ToString());
            sb.Length = 0;
        }

        [Conditional("DEBUG")]
        public static void AddObject(string tag, string name, object value, int maxDeep = 1)
        {
            var sb = new StringBuilder();
            AddObject(sb, "", name, value, maxDeep);
            Log.Debug(tag, sb.ToString());
            sb.Length = 0;
        }

        [Conditional("DEBUG")]
        public static void AddObject(StringBuilder sb, string indent, string name, object value, int maxDeep)
        {
            if (value == null)
            {
                sb.Append(indent);
                if (name != null) sb.Append(name + ": ");
                sb.AppendLine("null");
            }
            else if (value is int || value is double || value is float || value is byte || value is char || value is bool || value is string)
            {
                sb.Append(indent);
                if (name != null) sb.Append(name + ": ");
                sb.AppendLine(value.ToString());
            }
            else
            {
                var ivalues = value as IEnumerable;
                if (ivalues != null)
                {
                    if (maxDeep <= 0) return;
                    sb.Append(indent);
                    sb.AppendLine("[");
                    foreach (var ivalue in ivalues)
                    {
                        AddObject(sb, indent + "   ", null, ivalue, maxDeep);
                    }
                    sb.Append(indent);
                    sb.AppendLine("]");
                }
                else
                {
                    if (maxDeep <= 0) return;
                    sb.Append(indent);
                    sb.AppendLine("{");
                    foreach (var m in value.GetType().GetPropertiesAndFields())
                    {
                        AddObject(sb, indent + "   ", m.Name, m.GetValue(value, null), maxDeep - 1);
                    }
                    sb.Append(indent);
                    sb.AppendLine("},");
                }
            }
        }

        public object this[string name] { set { AddValue(name, value); } }
        public static LogWriter operator ++(LogWriter a) { a.Inc(); return a; }
        public static LogWriter operator --(LogWriter a) { a.Dec(); return a; }

        public static LogWriter operator &(LogWriter a, string b) { a.Concat(b); return a; }
        public static LogWriter operator &(LogWriter a, Exception b) { a.Concat(b); return a; }
        public static LogWriter operator &(LogWriter a, int b) { a.Concat(b); return a; }
        public static LogWriter operator &(LogWriter a, float b) { a.Concat(b); return a; }

        public static LogWriter operator +(LogWriter a, string b) { a.Sum(b); return a; }
        public static LogWriter operator -(LogWriter a, string b) { a.Sub(b); return a; }

    }


    public static class LogWriterHelper
    {


        [Conditional("DEBUG")]
        public static void Inc(this LogWriter a)
        {
            if (a == null) return;
            if (a.UseBraces) a.Add("{");
            a.IncLevel();
        }

        [Conditional("DEBUG")]
        public static void Dec(this LogWriter a)
        {
            if (a == null) return;
            a.DecLevel();
            if (a.UseBraces) a.Add("}");
        }

        [Conditional("DEBUG")]
        public static void Concat(this LogWriter a, string b)
        {
            if (a != null) a.Add(b);
        }

        [Conditional("DEBUG")]
        public static void Concat(this LogWriter a, object b)
        {
            if (a != null) a.Add(b == null ? "null" : b.ToString());
        }

        [Conditional("DEBUG")]
        public static void Concat(this LogWriter a, Exception b)
        {
            if (a != null)
                a.Add(b.FullMessage().Replace("\r\n", "\r\n                    " + a.Indent));
        }

        [Conditional("DEBUG")]
        public static void Sum(this LogWriter a, string b)
        {
            if (a != null)
            {
                a.Add(b + " {");
                a.IncLevel();
            }
        }

        [Conditional("DEBUG")]
        public static void Sub(this LogWriter a, string b)
        {
            if (a != null)
            {
                a.DecLevel();
                a.Add(a.UseBraces ? "} " + b : b);
            }
        }


#if DEBUG

        public static void Try(this LogWriter a, Action b)
        {
            if (a == null) { b(); return; }

            try
            {
                b();
            }
            catch (Exception ex)
            {
                a.Add(ex.FullMessage());
                throw;
            }
            finally
            {
                a.DecLevel();
                if (a.UseBraces) a.Add("}");
            }
        }

        public static T Try<T>(this LogWriter a, Func<T> b)
        {
            if (a == null) return b();

            try
            {
                return b();
            }
            catch (Exception ex)
            {
                a.Add(ex.FullMessage());
                throw;
            }
            finally
            {
                a.DecLevel();
                if (a.UseBraces) a.Add("}");
            }
        }


        public static void Try(this LogWriter a, string startMsg, Action b)
        {
            if (a == null) { b(); return; }
            a += startMsg;
            a.Try(b);
        }

        public static T Try<T>(this LogWriter a, string startMsg, Func<T> b)
        {
            if (a == null) return b();
            a += startMsg;
            return a.Try(b);
        }

#else 

        public static void Try(this LogWriter a, Action b) { b(); }
        public static T Try<T>(this LogWriter a, Func<T> b) { return b(); }
        public static void Try(this LogWriter a, string startMsg, Action b) { b(); }
        public static T Try<T>(this LogWriter a, string startMsg, Func<T> b) { return b(); }

#endif

    }
}
