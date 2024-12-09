namespace LookupEngine.Abstractions.ComponentModel;

public sealed class StringDescriptor : Descriptor
{
    public StringDescriptor(string text)
    {
        Name = text;
    }
}