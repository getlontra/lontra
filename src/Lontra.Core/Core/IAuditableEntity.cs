namespace Lontra;

public interface IAuditableEntity
{
    public UserId CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public UserId UpdatedBy { get; set; }

    public DateTime UpdatedOn { get; set; }
}
