using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KamGame
{

    public static class SystemReflection
    {

        public static T GetAttribute<T>(this MemberInfo member, bool inherit = true) where T : Attribute
        {
            return member.GetCustomAttributes(typeof(T), inherit).FirstOrDefault() as T;
        }

        public static IEnumerable<T> GetAttributes<T>(this MemberInfo member, bool inherit = true) where T : Attribute
        {
            return member.GetCustomAttributes(typeof(T), inherit).Cast<T>();
        }

        public static bool HasAttribute<T>(this MemberInfo member, bool inherit = true) where T : Attribute
        {
            return member.GetCustomAttributes(typeof(T), inherit).FirstOrDefault() != null;
        }


        public static MemberInfo[] GetPropertiesAndFields(this Type type)
        {
            var p = (MemberInfo[])type.GetProperties();
            var f = (MemberInfo[])type.GetFields();
            return p.Concat(f).ToArray();
        }
        public static MemberInfo GetPropertyOrField(this Type type, string name)
        {
            return (MemberInfo)type.GetProperty(name) ?? type.GetField(name);
        }


        public static Type ResultType(this Type type, string name)
        {
            var prop = type.GetProperty(name);
            if (prop != null)
                return prop.PropertyType;

            var field = type.GetField(name);
            if (field != null)
                return field.FieldType;

            throw new ArgumentException("Can't find property or field '" + name + "' in type " + type.Name, "name");
        }

        public static Type ResultType(this MemberInfo member)
        {
            var prop = member as PropertyInfo;
            if (prop != null)
                return prop.PropertyType;

            var field = member as FieldInfo;
            if (field != null)
                return field.FieldType;

            return null;
        }

        public static bool IsNullable(this Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        public static object GetValue(this Type type, object obj, string name, object[] index)
        {
            var prop = type.GetProperty(name);
            if (prop != null) return prop.GetValue(obj, index);

            var field = type.GetField(name);
            if (field != null) return field.GetValue(obj);

            throw new ArgumentException("Can't find field or property '" + name + "' in type " + type.Name, "name");
        }

        public static bool SetValue(this Type type, object obj, string name, object value, object[] index)
        {
            var prop = type.GetProperty(name);
            if (prop != null)
            {
                if (!prop.CanWrite) return false;
                prop.SetValue(obj, value.To(prop.PropertyType), index);
                return true;
            }
            var field = type.GetField(name);
            if (field != null)
            {
                field.SetValue(obj, value.To(field.FieldType));
                return true;
            }

            return false;
        }


        public static object GetValue(this MemberInfo member, object obj, object[] index)
        {
            var propInfo = member as PropertyInfo;
            if (propInfo != null)
                return propInfo.GetValue(obj, index);

            var fieldInfo = member as FieldInfo;
            if (fieldInfo != null)
                return fieldInfo.GetValue(obj);

            throw new ArgumentException(
                "Can't return member value of '" + member.Name + "'" +
                (member.DeclaringType != null ? " in type " + member.DeclaringType.Name : null)
            );
        }

        public static void SetValue(this MemberInfo member, object obj, object value, object[] index)
        {
            var prop = member as PropertyInfo;
            if (prop != null)
            {
                if (!prop.CanWrite) return;
                prop.SetValue(obj, value.To(prop.PropertyType), index);
                return;
            }

            var field = member as FieldInfo;
            if (field != null)
            {
                field.SetValue(obj, value.To(field.FieldType));
                return;
            }

            throw new ArgumentException(
                "Can't set member value of '" + member.Name + "'" +
                (member.DeclaringType != null ? " in type " + member.DeclaringType.Name : null)
            );
        }


        public static object ToMemberType(this object value, Type srcType, string prop)
        {
            return value == null ? null : value.To(srcType.ResultType(prop));
        }

        public static object ToMemberType<TSource>(this object value, string prop)
        {
            return ToMemberType(value, typeof(TSource), prop);
        }


        public static void CopyTo<T>(this IDictionary<string, object> src, T dest)
            where T : class
        {
            if (src == null) return;
            var t = typeof(T);
            foreach (var psrc in src)
            {
                try
                {
                    t.SetValue(dest, psrc.Key, psrc.Value, null);
                }
                catch (Exception ex)
                {
                    throw new Exception("IDictionary.Copy: property " + psrc.Key + ". " + ex.FullMessage());
                }
            }
        }
    }


    namespace Converts
    {
        public static class SystemReflection
        {
            public static object ToMemberType(this object value, Type srcType, string prop)
            {
                return value == null ? null : value.To(srcType.ResultType(prop));
            }

            public static object ToMemberType<TSource>(this object value, string prop)
            {
                return ToMemberType(value, typeof(TSource), prop);
            }
        }
    }

}


