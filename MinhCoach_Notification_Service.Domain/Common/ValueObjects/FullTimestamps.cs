namespace MinhCoach_Notification_Service.Domain.Common.ValueObjects;

public class FullTimestamps : BaseTimestamps
{
    public DateTime? DeletedAt { get; private set; }


    public FullTimestamps(
        DateTime createdAt, 
        DateTime? updatedAt = null, 
        DateTime? deletedAt = null)
        : base(createdAt, updatedAt)
    {
        DeletedAt = deletedAt;
    }
    
    public FullTimestamps UpdateTimestamp()
    {
        return new FullTimestamps(CreatedAt, DateTime.UtcNow);
    }
    
    public FullTimestamps MarkAsDeleted()
    {
        return new FullTimestamps(
            CreatedAt,
            UpdatedAt, 
            DateTime.UtcNow);
    }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return CreatedAt;
        yield return UpdatedAt;
        yield return DeletedAt;
    }
}