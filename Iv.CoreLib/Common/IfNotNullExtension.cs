using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Iv.Common
{
    public static class IfNotNullExtension
    {
        public static TResult IfNotNull<T, TResult>(
            this T subject,
            Func<T, TResult> expression)
            where T : class
        {
            if (subject == null) return default(TResult);
            return expression(subject);
        }

        public static TResult IfNotNull<T, TResult>(
            this T subject, Func<T, TResult> expression,
            TResult defaultValue)
            where T : class
        {
            if (subject == null) return defaultValue;
            return expression(subject);
        }

        public static void IfNotNull<T>(this T subject, Action<T> expression)
            where T : class
        {
            if (subject != null)
            {
                expression(subject);
            }
        }

    }
}
