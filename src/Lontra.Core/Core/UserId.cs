using Lontra.Core.Implementation;

namespace Lontra;

/// <summary>
/// <see cref="User"/> identifier.
/// </summary>
public record UserId(long Value) : Identifier<long>(Value);
