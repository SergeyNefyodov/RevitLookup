namespace LookupEngine.Tests.Unit.Data;

public sealed class ResolveObject
{
    public string UnsupportedMethod(int parameter)
    {
        return parameter.ToString();
    }

    public string UnsupportedDescribedMethod(int parameter)
    {
        return parameter.ToString();
    }

    public string UnsupportedMultiMethod(int parameter)
    {
        return parameter.ToString();
    }
}