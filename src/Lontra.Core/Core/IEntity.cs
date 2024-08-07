using Lontra.Core.Reflection;

namespace Lontra;

public interface IEntity<TId> : IIEntity
{
    /// <summary>
    /// Unique record identifier, typically auto-generated on creation.
    /// </summary>
    public TId Id { get; set; }
}
