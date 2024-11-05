using RevitLookup.Abstractions.Models;

namespace RevitLookup.Abstractions.ViewModels.AboutProgram;

public interface IOpenSourceViewModel
{
    List<OpenSourceSoftware> Software { get; }
}