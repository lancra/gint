// <auto-generated />

using System.Resources;

#nullable enable

namespace Gint.Core.Properties
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static class Messages
    {
        private static readonly ResourceManager _resourceManager
            = new ResourceManager("Gint.Core.Properties.Messages", typeof(Messages).Assembly);

        /// <summary>
        /// Inapplicable command '{descriptor}' for current changes.
        /// </summary>
        public static string InapplicableOperationInput(object? descriptor)
            => string.Format(
                GetString("InapplicableOperationInput", nameof(descriptor)),
                descriptor);

        /// <summary>
        /// Only one letter is expected for command '{descriptor}', got '{text}'.
        /// </summary>
        public static string LongAreaAgnosticRunspecInput(object? descriptor, object? text)
            => string.Format(
                GetString("LongAreaAgnosticRunspecInput", nameof(descriptor), nameof(text)),
                descriptor, text);

        /// <summary>
        /// Only one letter and an optional area indicator are expected for command '{descriptor}', got '{text}'.
        /// </summary>
        public static string LongAreaAwareRunspecInput(object? descriptor, object? text)
            => string.Format(
                GetString("LongAreaAwareRunspecInput", nameof(descriptor), nameof(text)),
                descriptor, text);

        /// <summary>
        /// Unknown area '{area}'.
        /// </summary>
        public static string UnknownAreaInput(object? area)
            => string.Format(
                GetString("UnknownAreaInput", nameof(area)),
                area);

        /// <summary>
        /// Unknown command '{descriptor}' (use '?' for help).
        /// </summary>
        public static string UnknownOperationInput(object? descriptor)
            => string.Format(
                GetString("UnknownOperationInput", nameof(descriptor)),
                descriptor);

        private static string GetString(string name, params string[] formatterNames)
        {
            var value = _resourceManager.GetString(name)!;
            for (var i = 0; i < formatterNames.Length; i++)
            {
                value = value.Replace($"{{{formatterNames[i]}}}", $"{{{i}}}");
            }

            return value;
        }
    }
}


