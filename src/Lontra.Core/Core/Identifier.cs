using Lontra.Core.Reflection;

namespace Lontra;

/// <summary>
/// Record identifier.
/// </summary>
/// <typeparam name="TIdValue"></typeparam>
/// <param name="Value"></param>
public abstract record Identifier<TIdValue>(TIdValue Value) : IIdentifier
{
    public sealed override string ToString()
    {
        return Value?.ToString() ?? "";
    }
}
