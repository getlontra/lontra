namespace Lontra;

public interface ISoftDeleteEntity
{
    /// <summary>
    /// Soft-delete flag. If set to false, this record has been deleted.
    /// </summary>
    /// <remarks>
    /// Each entity may define its own logic for when records should be permanently deleted.
    /// </remarks>
    public bool IsActive { get; set; }
}
