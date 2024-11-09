using RevitLookup.Abstractions.Models.AboutProgram;

namespace RevitLookup.Abstractions.ViewModels.AboutProgram;

public interface IOpenSourceViewModel
{
    List<OpenSourceSoftware> Software { get; }
}