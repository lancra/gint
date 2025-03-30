namespace Gint.Core.Operations;

internal class OperationAccessor : IOperationAccessor
{
    public IReadOnlyCollection<OperationDescriptor> Filter(OperationFilter filter)
        => OperationDescriptor.List.Where(descriptor => descriptor.Scopes.Contains(filter.Scope))
        .Where(descriptor =>
            descriptor.Areas.Count == 0 ||
            descriptor.Areas.Any(area => filter.Changes.HasActionableFiles(area)))
        .Where(descriptor =>
            descriptor.Indicators.Count == 0 ||
            descriptor.Indicators.Any(indicator => filter.Changes.HasIndicator(indicator)))
        .Where(descriptor => descriptor.OnFilter(filter))
        .OrderBy(descriptor => descriptor.Kind == OperationKind.Control)
        .ThenBy(descriptor => !char.IsLetter(descriptor.Value))
        .ThenBy(descriptor => descriptor.Value)
        .ToArray();
}
