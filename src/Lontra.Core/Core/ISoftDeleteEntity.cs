namespace Lontra;

/// <summary>
/// <para>
/// Specifies that the entity uses a soft-delete pattern for data deletion.
/// </para>
/// <para>
/// Whenever a non-admin user executes a delete action, rather than remove the data immediately,
/// the <see cref="IsActive"/> flag is set to false, allowing the data to be recovered.
/// </para>
/// </summary>
/// <remarks>
/// Please note that setting this flag to false will not cascade soft-deletions to other database records.
/// </remarks>
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
