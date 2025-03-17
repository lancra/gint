using Gint.Core.Changes;
using Gint.Core.Properties;

namespace Gint.Core.Operations;

/// <summary>
/// Provides the user specification for running an operation.
/// </summary>
/// <param name="Descriptor">The operation to run.</param>
/// <param name="Area">The area to run the operation in.</param>
public record Runspec(OperationDescriptor Descriptor, ChangeArea? Area)
{
    /// <summary>
    /// Parses a <see cref="Runspec"/> from text.
    /// </summary>
    /// <param name="text">The text to parse.</param>
    /// <param name="descriptors">The available operations to run.</param>
    /// <returns>The result of parsing the <see cref="Runspec"/>.</returns>
    public static RunspecParseResult Parse(string? text, IReadOnlyCollection<OperationDescriptor> descriptors)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return RunspecParseResult.Error(string.Empty);
        }

        var descriptorValue = text[0];
        if (!OperationDescriptor.TryFromValue(descriptorValue, out var descriptor))
        {
            return RunspecParseResult.Error(Messages.UnknownOperationInput(descriptorValue));
        }

        if (!descriptors.Contains(descriptor))
        {
            return RunspecParseResult.Error(Messages.InapplicableOperationInput(descriptorValue));
        }

        var area = default(ChangeArea?);
        if (text.Length > 1)
        {
            if (descriptor.Areas.Count <= 1)
            {
                return RunspecParseResult.Error(Messages.LongAreaAgnosticRunspecInput(descriptorValue, text));
            }

            if (text.Length > 2)
            {
                return RunspecParseResult.Error(Messages.LongAreaAwareRunspecInput(descriptorValue, text));
            }

            var areaValue = text[1];
            if (!ChangeArea.TryFromValue(areaValue, out area))
            {
                return RunspecParseResult.Error(Messages.UnknownAreaInput(areaValue));
            }
        }

        var runspec = new Runspec(descriptor, area);
        return RunspecParseResult.Success(runspec);
    }
}
