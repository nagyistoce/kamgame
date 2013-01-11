using System;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

namespace KamGame
{
    public static class XElementHelper
    {

        [DebuggerStepThrough]
        public static string Attr(this XElement el, XName attrName)
        {
            if (el == null) return null;
            var attr = el.Attribute(attrName);
            return attr == null ? null : attr.Value;
        }

        [DebuggerStepThrough]
        public static T Attr<T>(this XElement el, XName attrName, T defaultValue)
        {
            if (el == null) return defaultValue;
            var attr = el.Attribute(attrName);
            return attr == null ? defaultValue : attr.Value.To<T>();
        }

        [DebuggerStepThrough]
        public static T EnumAttr<T>(this XElement el, XName attrName)
        {
            if (el == null) return default(T);
            var attr = el.Attribute(attrName);
            if (attr == null) return default(T);

            var t = typeof(T);
            return Enum.IsDefined(t, attr.Value) ? (T)Enum.Parse(t, attr.Value) : default(T);
        }

        [DebuggerStepThrough]
        public static bool TryAttr(this XElement el, XName attrName, out string value)
        {
            value = null;
            if (el == null) return false;
            var attr = el.Attribute(attrName);
            if (attr != null)
            {
                value = attr.Value;
                return true;
            }
            return false;
        }

        [DebuggerStepThrough]
        public static bool TryAttr(this XElement el, XName attrName, out bool value)
        {
            string s;
            value = false;
            return (TryAttr(el, attrName, out s) && bool.TryParse(s, out value));
        }

        [DebuggerStepThrough]
        public static bool TryAttr(this XElement el, XName attrName, out float value)
        {
            string s;
            value = 0;
            return (TryAttr(el, attrName, out s) && float.TryParse(s, out value));
        }

        [DebuggerStepThrough]
        public static bool TryAttr(this XElement el, XName attrName, out int value)
        {
            string s;
            value = 0;
            return (TryAttr(el, attrName, out s) && int.TryParse(s, out value));
        }

        [DebuggerStepThrough]
        public static bool TryAttr(this XElement el, XName attrName, out DateTime value)
        {
            string s;
            value = DateTime.MinValue;
            return (TryAttr(el, attrName, out s) && DateTime.TryParse(s, out value));
        }


        [DebuggerStepThrough]
        public static string ElAttr(this XElement el, XName elName, XName attrName)
        {
            if (el == null) return null;
            var el2 = el.Element(elName);
            if (el2 == null) return null;
            var attr = el2.Attribute(attrName);
            return attr == null ? null : attr.Value;
        }

        [DebuggerStepThrough]
        public static bool TryElAttr(this XElement el, XName elName, XName attrName, out string value)
        {
            value = null;
            if (el == null) return false;
            var el2 = el.Element(elName);
            if (el2 == null) return false;
            var attr = el2.Attribute(attrName);
            if (attr != null)
            {
                value = attr.Value;
                return true;
            }
            return false;
        }

        [DebuggerStepThrough]
        public static bool TryElAttr(this XElement el, XName elName, XName attrName, out float value)
        {
            string s;
            value = 0;
            return (TryElAttr(el, elName, attrName, out s) && float.TryParse(s, out value));
        }

        [DebuggerStepThrough]
        public static bool TryElAttr(this XElement el, XName elName, XName attrName, out int value)
        {
            string s;
            value = 0;
            return (TryElAttr(el, elName, attrName, out s) && int.TryParse(s, out value));
        }

        [DebuggerStepThrough]
        public static bool TryElAttr(this XElement el, XName elName, XName attrName, out DateTime value)
        {
            string s;
            value = DateTime.MinValue;
            return (TryElAttr(el, elName, attrName, out s) && DateTime.TryParse(s, out value));
        }


        [DebuggerStepThrough]
        public static string ElElAttr(this XElement el, XName elName1, XName elName2, XName attrName)
        {
            if (el == null) return null;
            var el1 = el.Element(elName1);
            if (el1 == null) return null;
            var el2 = el1.Element(elName2);
            if (el2 == null) return null;
            var attr = el2.Attribute(attrName);
            return attr == null ? null : attr.Value;
        }

        [DebuggerStepThrough]
        public static bool TryElElAttr(this XElement el, XName elName1, XName elName2, XName attrName, out string value)
        {
            value = null;
            if (el == null) return false;
            var el1 = el.Element(elName1);
            if (el1 == null) return false;
            var el2 = el1.Element(elName2);
            if (el2 == null) return false;
            var attr = el2.Attribute(attrName);
            if (attr != null)
            {
                value = attr.Value;
                return true;
            }
            return false;
        }

        [DebuggerStepThrough]
        public static bool TryElElAttr(this XElement el, XName elName1, XName elName2, XName attrName, out float value)
        {
            string s;
            value = 0;
            return (TryElElAttr(el, elName1, elName2, attrName, out s) && float.TryParse(s, out value));
        }

        [DebuggerStepThrough]
        public static bool TryElElAttr(this XElement el, XName elName1, XName elName2, XName attrName, out int value)
        {
            string s;
            value = 0;
            return (TryElElAttr(el, elName1, elName2, attrName, out s) && int.TryParse(s, out value));
        }

        [DebuggerStepThrough]
        public static bool TryElElAttr(this XElement el, XName elName1, XName elName2, XName attrName, out DateTime value)
        {
            string s;
            value = DateTime.MinValue;
            return (TryElElAttr(el, elName1, elName2, attrName, out s) && DateTime.TryParse(s, out value));
        }

        [DebuggerStepThrough]
        public static XElement ElEl(this XElement el, XName elName1, XName elName2)
        {
            if (el == null) return null;
            var el1 = el.Element(elName1);
            return el1 == null ? null : el1.Element(elName2);
        }

        [DebuggerStepThrough]
        public static XElement Els(this XElement el, params XName[] elNames)
        {
            if (el == null) return null;
            return elNames.Any(name => (el = el.Element(name)) == null) ? null : el;
        }


        [DebuggerStepThrough]
        public static string ElValue(this XElement el, XName elName)
        {
            if (el == null) return null;
            var el1 = el.Element(elName);
            return el1 == null ? null : el1.Value;
        }

        [DebuggerStepThrough]
        public static string ElElValue(this XElement el, XName elName, XName elName2)
        {
            if (el == null) return null;
            var el1 = el.Element(elName);
            if (el1 == null) return null;
            var el2 = el1.Element(elName2);
            return el2 == null ? null : el2.Value;
        }

    }
}
