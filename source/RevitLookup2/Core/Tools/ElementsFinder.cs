namespace RevitLookup2.Core.Tools;

public static class ElementsFinder
{
    [Pure]
    public static List<Element> SearchElements(string searchText)
    {
        var activeDocument = Context.ActiveDocument;
        if (activeDocument is null) return [];

        var rows = searchText.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);
        var items = ParseRawRequest(rows);
        var results = new List<Element>(items.Count);

        foreach (var rawId in items)
        {
#if REVIT2024_OR_GREATER
            if (long.TryParse(rawId, out var id))
            {
                var element = activeDocument.GetElement(new ElementId(id));
#else
            if (int.TryParse(rawId, out var id))
            {
                var element = activeDocument.GetElement(new ElementId(id));
#endif
                if (element is not null) results.Add(element);
            }
            else if (rawId.Length == 45 && rawId.Count(c => c == '-') == 5)
            {
                var element = activeDocument.GetElement(rawId);
                if (element is not null) results.Add(element);
            }
            else if (rawId.Length == 22 && rawId.Count(c => c == ' ') == 0)
            {
                var elements = SearchByIfcGuid(rawId, activeDocument);
                results.AddRange(elements);
            }
            else
            {
                var elements = SearchByName(rawId, activeDocument);
                results.AddRange(elements);
            }
        }

        return results;
    }

    private static List<string> ParseRawRequest(string[] rows)
    {
        var items = new List<string>(rows.Length);
        var delimiters = new[] {'\t', ';', ',', ' '};
        foreach (var row in rows)
        {
            for (var i = 0; i < delimiters.Length; i++)
            {
                var delimiter = delimiters[i];
                var split = row.Split([delimiter], StringSplitOptions.RemoveEmptyEntries);
                if (split.Length > 1 || i == delimiters.Length - 1 || split.Length == 1 && split[0] != row)
                {
                    items.AddRange(split);
                    break;
                }
            }
        }

        return items;
    }

    private static IEnumerable<Element> SearchByName(string rawId, Document document)
    {
        var elementTypes = document.GetElements().WhereElementIsElementType();
        var elementInstances = document.GetElements().WhereElementIsNotElementType();
        return elementTypes
            .UnionWith(elementInstances)
            .Where(element => element.Name.Contains(rawId, StringComparison.OrdinalIgnoreCase));
    }

    private static IList<Element> SearchByIfcGuid(string rawId, Document document)
    {
        var guidProvider = new ParameterValueProvider(new ElementId(BuiltInParameter.IFC_GUID));
        var typeGuidProvider = new ParameterValueProvider(new ElementId(BuiltInParameter.IFC_TYPE_GUID));
#if REVIT2022_OR_GREATER
        var filterRule = new FilterStringRule(guidProvider, new FilterStringEquals(), rawId);
        var typeFilterRule = new FilterStringRule(typeGuidProvider, new FilterStringEquals(), rawId);
#else
        var filterRule = new FilterStringRule(guidProvider, new FilterStringEquals(), rawId, true);
        var typeFilterRule = new FilterStringRule(typeGuidProvider, new FilterStringEquals(), rawId, true);
#endif
        var elementFilter = new ElementParameterFilter(filterRule);
        var typeElementFilter = new ElementParameterFilter(typeFilterRule);

        var typeGuidsCollector = document
            .GetElements()
            .WherePasses(typeElementFilter);

        return document
            .GetElements()
            .WherePasses(elementFilter)
            .UnionWith(typeGuidsCollector)
            .ToElements();
    }
}