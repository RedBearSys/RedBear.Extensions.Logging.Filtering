using Microsoft.Extensions.Logging.Internal;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace RedBear.Extensions.Logging.Filtering
{
    public static class FormattedLogValuesExtensions
    {
        /// <summary>Clones the FormattedLogValues instance.</summary>
        /// <param name="original">The original.</param>
        /// <returns></returns>
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public static FormattedLogValues Clone(this FormattedLogValues original)
        {
            var field = original.GetType().GetField("_values", BindingFlags.NonPublic | BindingFlags.Instance);
            var values = (object[])field.GetValue(original);

            field = original.GetType().GetField("_originalMessage", BindingFlags.NonPublic | BindingFlags.Instance);
            var originalMessage = (string)field.GetValue(original);

            field = original.GetType().GetField("_formatter", BindingFlags.NonPublic | BindingFlags.Instance);
            var formatter = (LogValuesFormatter) field.GetValue(original);

            return new FormattedLogValues(formatter?.OriginalFormat ?? originalMessage, values);
        }

        /// <summary>Gets the original string value including any placeholders.</summary>
        /// <param name="lv">The FormattedLogValues instance.</param>
        /// <returns></returns>
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public static string GetOriginalValue(this FormattedLogValues lv)
        {
            var field = lv.GetType().GetField("_originalMessage", BindingFlags.NonPublic | BindingFlags.Instance);
            var originalMessage = (string)field.GetValue(lv);

            return (string) lv.LastOrDefault().Value ?? originalMessage;
        }

        /// <summary>Gets the values.</summary>
        /// <param name="lv">The FormattedLogValues instance.</param>
        /// <returns></returns>
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public static object[] GetValues(this FormattedLogValues lv)
        {
            var field = lv.GetType().GetField("_values", BindingFlags.NonPublic | BindingFlags.Instance);
            return (object[])field.GetValue(lv);
        }

        /// <summary>  Replaces the values in the FormattedLogValues instance.</summary>
        /// <param name="lv">The FormattedLogValues instance.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static FormattedLogValues SetValues(this FormattedLogValues lv, params object[] values)
        {
            return new FormattedLogValues(lv.GetOriginalValue(), values);
        }

        /// <summary>Sets a simple value.</summary>
        /// <param name="original">The original.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static FormattedLogValues SetSimpleValue(this FormattedLogValues original, string value)
        {
            return new FormattedLogValues(value);
        }
    }
}
