namespace Lontra.Core.Reflection;

/// <summary>
/// <para>
/// Type-agnostic Entity Identifier and Base Type for all Identifiers.
/// </para>
/// <para>
/// The purpose of this class is to help with Reflection in cases where we don't care about
/// the specific type in use.
/// </para>
/// </summary>
public abstract record IIdentifier();
