using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace KieranCoppins.GenericHelpers
{
    // A class of static generic helpers that help with dealing with generics
    public static class GenericHelpers
    {
        /// <summary>
        /// Useful binding flags to get variables regardless of access modifiers
        /// </summary>
        public static readonly BindingFlags GetFieldFlags = BindingFlags.Default
            | BindingFlags.Public
            | BindingFlags.NonPublic
            | BindingFlags.Instance;

        /// <summary>
        /// Check if type is derived from generic where generic is an unbound generic class
        /// </summary>
        /// <param name="generic">The base generic type</param>
        /// <param name="toCheck">The type to check</param>
        /// <returns></returns>
        public static bool IsSubClassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                    return true;
                toCheck = toCheck.BaseType;
            }
            return false;
        }

        /// <summary>
        /// Finds the first base generic it can find
        /// </summary>
        /// <param name="obj">The object to extract the generic from</param>
        /// <returns>The type inside the generic</returns>
        public static Type GetGenericType(object obj)
        {
            Type type = null;

            Type current = obj.GetType();
            while (type == null && current != typeof(object))
            {
                if (current.IsGenericType)
                {
                    type = current.GetGenericTypeDefinition().GetGenericArguments()[0].GetType();
                }
                current = current.BaseType;
            }
            return type;
        }

        /// <summary>
        /// Splits a camel cased string
        /// </summary>
        /// <param name="str">The camel cased string to split</param>
        /// <returns>A normal string seperated at capital letters</returns>
        public static string SplitCamelCase(this string str)
        {
            return Regex.Replace(
                Regex.Replace(
                    str,
                    @"(\P{Ll})(\P{Ll}\p{Ll})",
                    "$1 $2"
                ),
                @"(\p{Ll})(\P{Ll})",
                "$1 $2"
            ).Replace("( Clone )", "");
        }

        /// <summary>
        /// Sets a variable of an object regardless of access modifiers or if it is a field or a property
        /// </summary>
        /// <param name="obj">The object that is being changed</param>
        /// <param name="value">The value to assign the variable</param>
        /// <param name="varName">The name of the variable</param>
        public static void SetVariable(object obj, object value, string varName)
        {
            var field = obj.GetType().GetField(varName, GetFieldFlags);
            if (field != null)
                field.SetValue(obj, value);
            else
                obj.GetType().GetProperty(varName, GetFieldFlags).SetValue(obj, value);
        }
    }

    public enum Operators
    {
        LessThan,
        GreaterThan,
        EqualTo
    }
}
