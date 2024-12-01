using LookupEngine.Abstractions;
using RevitLookup.Abstractions.ObservableModels.Decomposition;
using Riok.Mapperly.Abstractions;

namespace RevitLookup.UI.Playground.Mappers;

[Mapper]
public static partial class DecompositionResultMapper
{
    public static partial ObservableDecomposedObject Convert(DecomposedObject decomposedObject);
}