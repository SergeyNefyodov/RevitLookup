namespace LookupEngine.Abstractions.Descriptors.System;

public sealed class StringDescriptor : Descriptor
{
    public StringDescriptor(string text)
    {
        Name = text;
    }
}