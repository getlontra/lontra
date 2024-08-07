namespace Lontra;

/// <summary>
/// <see cref="Tenant"/> identifier.
/// </summary>
public record TenantId(long Value) : Identifier<long>(Value);
